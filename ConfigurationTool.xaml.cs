using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Interop;

namespace LaptopOrchestra.Kinect
{
    public partial class ConfigurationTool : Window
    {
        
        /// <summary>
        ///     List of bodies
        /// </summary>
        private IList<Body>        _bodies;

        /// <summary>
        ///     List of tabs in gui
        /// </summary>
        private TabList _tabList;

        /// <summary>
        ///     Local list of tabs
        /// </summary>
        List<TabData> _localSessions;
        /// <summary>
        ///     Used to fixed alignment issue between skeleton positional data and color image
        /// </summary>
        private CoordinateMapper _coordinateMapper;
        /// <summary>
        ///     Copy of session manager to get open connections
        /// </summary>
        private SessionManager _sessionManager;

        private Timer _timer;
        private Thread _thread;

        public ConfigurationTool(SessionManager sessionManager, KinectProcessor kinectProcessor)
        {
            InitializeComponent();

            _sessionManager = sessionManager;

            _coordinateMapper = kinectProcessor.CoordinateMapper;
            kinectProcessor.Reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            _localSessions = new List<TabData>();
            _tabList = new TabList();

            // Start timer for flag updating thread
            _timer = new Timer(TimerTick, null, 0, 100);
            _thread = new Thread(updateFlags);
        }

        private void TimerTick(object state)
        {
            // If flag updating thread is still running skip
            if (!_thread.IsAlive)            {
                _thread = new Thread(updateFlags);
                _thread.Start();
            }
        
        }

        private void updateFlags()
        {
            
            var jointTypes = Enum.GetValues(typeof(JointType));

            // Copy session workers so iteration doesnt break the collection
            SessionWorker[] workers = new SessionWorker[_sessionManager.OpenConnections.Count];
            _sessionManager.OpenConnections.CopyTo(workers);

            foreach (SessionWorker sw in workers)
            {
                
                // Get the port and IP of this session
                string id = sw.Ip + ":" + sw.Port.ToString();

                // Copy the flags
                Dictionary<JointType, bool> flags;
                flags = sw.ConfigFlags;                

                // Get the list of active joints
                List<String> jointList = new List<string>();
                foreach (JointType jt in jointTypes)
                {
                    if (flags[jt])
                    {
                        jointList.Add(jt.ToString());
                    }
                }
                    
                // If this session already exists update the flags
                if (_localSessions.Exists(tab => tab.Header == id))
                {
                    _localSessions.Find(tab => tab.Header.Equals(id)).displayFlags = flags;
                    _localSessions.Find(tab => tab.Header.Equals(id)).Items = jointList;
                    _localSessions.Find(tab => tab.Header.Equals(id)).Active = true;
                } else
                {
                    TabData tabData = new TabData(id, jointList, flags, true);
                    _localSessions.Add(tabData);
                }                    
            }
            updateTabs();

        }      

        private void updateTabs()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                // Maintain the current tab selection
                int currentSelected = 0;
                if (_tabList.getTabs().Count > 1)
                {
                    currentSelected = tabControl.SelectedIndex;
                }

                // Update the list of tabs in the gui
                _tabList.Clear();
                foreach (TabData tab in _localSessions)
                {
                    if (tab.Active)
                    {
                        _tabList.getTabs().Add(tab);
                    }
                    tab.Active = false;
                }
                tabControl.ItemsSource = _tabList;

                // Restore selection
                tabControl.SelectedIndex = currentSelected;
            }));
        }

        
        private void Window_Closed(object sender, EventArgs e)
        {
            //TODO subscribe KinectProcess.Stop() and UDP.stop() to this event; maybe this can go inside App.xaml.cs instead
        }
        

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Draw the Image from the Camera
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    XAMLImage.Source = frame.ToBitmap();
                }
            }

            // Acquire skeleton data as well
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame == null) return;

                XAMLCanvas.Children.Clear();

                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

                foreach (var body in _bodies)
                {
                    if (body == null || !body.IsTracked) continue;


                    IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                    // convert the joint points to depth (display) space
                    Dictionary<JointType, Point> alignedJointPoints = new Dictionary<JointType, Point>();

                    foreach (JointType jointType in joints.Keys)
                    {
                        // sometimes the depth(Z) of an inferred joint may show as negative
                        // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                        CameraSpacePoint position = joints[jointType].Position;
                        if (position.Z < 0)
                        {
                            position.Z = 0.01f;
                        }

                        ColorSpacePoint colorPoint = _coordinateMapper.MapCameraPointToColorSpace(position);

                        alignedJointPoints[jointType] = new Point(colorPoint.X, colorPoint.Y);
                    }



                    TabData ti = tabControl.SelectedItem as TabData;
                    if ( ti != null )
                    {                       
                        string id = ti.Header;
                        XAMLCanvas.DrawSkeleton(body, alignedJointPoints, _localSessions[_localSessions.IndexOf(ti)].displayFlags);
                    } else                  
                    {
                        var jointTypes = Enum.GetValues(typeof(JointType));
                        Dictionary<JointType, bool> displayFlags = new Dictionary<JointType, bool>();
                        foreach (JointType jt in jointTypes)
                        {                            
                            displayFlags[jt] = true;
                        }
                        XAMLCanvas.DrawSkeleton(body, alignedJointPoints, displayFlags);
                    }
                   

                }
            }
        }

        // This code came from the following url:        
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/b0df3d1f-e211-4f54-a079-09af0096410e/keeping-a-windows-aspect-ratio-during-resize?forum=wpf
        // Thanks to Marco Zhou
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = HwndSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                source.AddHook(new HwndSourceHook(WinProc));
            }
        }

        public const Int32 WM_EXITSIZEMOVE = 0x0232;
        private IntPtr WinProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            IntPtr result = IntPtr.Zero;
            switch (msg)
            {
                case WM_EXITSIZEMOVE:
                    {
                        // This ratio corresponds to 1080:2120
                        this.Height = this.Width * 0.5094;
                        break;
                    }
            }

            return result;
        }



    }

    
}