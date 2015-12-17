using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;

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
        ///     Index of tabs using ip and port as key
        /// </summary>
        private Dictionary<String, int> _tabIndex;
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
            _tabIndex = new Dictionary<string, int>();
            _tabList = new TabList();
            // Start timer for flag updating thread
            _timer = new Timer(TimerTick,null, 0, 1000);
            _thread = new Thread(updateFlags);
        }

        private void TimerTick(object state)
        {
            if (!_thread.IsAlive)
            {
                _thread = new Thread(updateFlags);
                _thread.Start();
            }
        
        }

        private void updateFlags()
        {
            List<String> jointList = new List<string>();
            var jointTypes = Enum.GetValues(typeof(JointType));
            int currentSelected = 0;
            if (_tabIndex.Count > 0)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {

                    currentSelected = tabControl.SelectedIndex;
                }));
            }
            
           //lock(_sessionManager)
           //{
                foreach (SessionWorker sw in _sessionManager.OpenConnections)
                {
                    string id = sw.Ip + ":" + sw.Port.ToString();
                    Dictionary<JointType, bool> configurationFlags = sw.ConfigFlags;
                    jointList.Clear();
                    foreach (JointType jt in jointTypes)
                    {
                        if (configurationFlags[jt])
                        {
                            jointList.Add(jt.ToString());
                        }
                    }

                    TabData tabData = new TabData(id, jointList, sw.ConfigFlags);
                    if (_tabIndex.ContainsKey(id))
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            _tabList.getTabs().RemoveAt(_tabIndex[id]);
                            _tabList.getTabs().Insert(_tabIndex[id], tabData);
                        }));
                    }
                    else
                    {                        
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            _tabList.getTabs().Add(tabData);
                        }));

                        _tabIndex.Add(id, _tabList.getIndex(tabData));
                    }
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    tabControl.ItemsSource = _tabList;
                    tabControl.SelectedIndex = currentSelected;                    
                }));
           //}      
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
                        XAMLCanvas.DrawSkeleton(body, alignedJointPoints, _tabList.getTabs()[_tabIndex[id]].displayFlags);
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



    }
    
}