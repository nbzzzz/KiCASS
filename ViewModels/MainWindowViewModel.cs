using LaptopOrchestra.Kinect.Model;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

//UI THREAD
namespace LaptopOrchestra.Kinect.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Public Properties
        public EventHandler RequestClose { get; internal set; }
        private CoordinateMapper _coordinateMapper;
        private SessionManager _sessionManager;
        private Dictionary<JointType, bool> _configurationFlags;
        private KinectProcessor _kinectProcessor;
        private UDPReceiver _udpRec;
        private IList<Body> _bodies;

        private Canvas _myCanvas;
        public Canvas MyCanvas
        {
            get { return _myCanvas; }
            set
            {
                if (_myCanvas != value)
                {
                    _myCanvas = value;
                    NotifyPropertyChanged("MyCanvas");
                }
            }
        }

        private System.Windows.Media.ImageSource _myFrame;
        public System.Windows.Media.ImageSource MyFrame
        {
            get { return _myFrame; }
            set
            {
                if (_myFrame != value)
                {
                    _myFrame = value;
                    NotifyPropertyChanged("MyFrame");
                }
            }
        }

        private Image _myImage;
        public Image MyImage
        {
            get { return _myImage; }
            set
            {
                if (_myImage != value)
                {
                    _myImage = value;
                    NotifyPropertyChanged("MyImage");
                }
            }
        }

        private MainWindowModel _currentState;
        public MainWindowModel CurrentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    NotifyPropertyChanged("CurrentState");
                }
            }
        }

        private TabWindowViewModel _myTabWindowViewModel;
        public TabWindowViewModel MyTabWindowViewModel
        {
            get { return _myTabWindowViewModel; }
            set
            {
                if (_myTabWindowViewModel != value)
                {
                    _myTabWindowViewModel = value;
                    NotifyPropertyChanged("MyTabWindowViewModel");
                }
            }
        }

        private static string[] photo = {
            "/Assets/sensor-off.jpg",
            "/Assets/sensor-on.jpg",
            "/Assets/sensor-on-flip.jpg",
            "/Assets/sensor-tracking.jpg",
            "/Assets/sensor-tracking-flip.jpg"
        };

        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            //Initialize everything
            _configurationFlags = new Dictionary<JointType, bool>();
            _kinectProcessor = new KinectProcessor();
            _sessionManager = new SessionManager();
            _udpRec = new UDPReceiver(8080, _sessionManager, _kinectProcessor);
            _coordinateMapper = _kinectProcessor.CoordinateMapper;
            CurrentState = new MainWindowModel();
            MyCanvas = new Canvas();
            SetState(0);

            //Start the background thread for updating tabs.
            //MyTabWindowViewModel = new TabWindowViewModel(_sessionManager);

            //Start the UI thread for updating the UI. //Debug.WriteLine("\n starting UI thread \n");
            _kinectProcessor.Reader.MultiSourceFrameArrived += UI_Thread;
            _kinectProcessor.Sensor.IsAvailableChanged += UpdateSensorStatus;
        }
        #endregion

        #region Helper Functions
        private void UI_Thread(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            //Debug.WriteLine("\nUI thread hit");
            var reference = e.FrameReference.AcquireFrame();
            var mainWin = System.Windows.Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            #region draw image
            // Draw the Image from the Camera
            var frame = reference.ColorFrameReference.AcquireFrame();
            if (frame != null)
            {
                using (frame)
                {
                    if (frame != null)
                    {
                        SetState(2);
                        mainWin.XAMLImage.Source = frame.ToBitmap();
                    }
                }
            }
            else SetState(1);
            #endregion draw bodies

            #region draw skeleton
            // Acquire skeleton data
            var bodyFrame = reference.BodyFrameReference.AcquireFrame();
            if (bodyFrame == null)
                return;
            else
            {
                using (bodyFrame)
                {
                    MyCanvas.Children.Clear();
                    _bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    bool isFirst = true;

                    foreach (var body in _bodies)
                    {
                        if (body == null || !body.IsTracked) continue;
                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                        // convert the joint points to depth (display) space
                        Dictionary<JointType, System.Windows.Point> alignedJointPoints = new Dictionary<JointType, System.Windows.Point>();

                        foreach (JointType jointType in joints.Keys)
                        {
                            // sometimes the depth(Z) of an inferred joint may show as negative
                            // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                            CameraSpacePoint position = joints[jointType].Position;
                            if (position.Z < 0)
                                position.Z = 0.01f;

                            ColorSpacePoint colorPoint = _coordinateMapper.MapCameraPointToColorSpace(position);
                            alignedJointPoints[jointType] = new System.Windows.Point(colorPoint.X, colorPoint.Y);
                        }

                        SetState(3);
                        //MyCanvas.DrawSkeleton(body, alignedJointPoints, isFirst);
                        isFirst = false;
                    }
                }
            }
            #endregion draw skeleton
                        
        }

        private void UpdateSensorStatus(object sender, IsAvailableChangedEventArgs e)
        {
            SetState(0);
        }

        protected void SetState(int state)
        {
            //State 0: Initializing
            if (state == 0)
            {
                //update MainWindow-Model Values
                CurrentState.StatusColor = "Red";
                CurrentState.StatusTitle = "INITIALIZING...";
                CurrentState.CameraToggleFlag = true;
                CurrentState.ImageOrientationFlag = 1;
                CurrentState.OrientationButtonText = "Orientation: MIRRORED";
                CurrentState.Status = photo[0];
            }
            //State 1: Initialized; Kinect Unplugged
            else if (state == 1)
            {
                CurrentState.StatusColor = "Red";
                CurrentState.StatusTitle = "KINECT UNPLUGGED!";
                CurrentState.Status = photo[0];
            }
            //State 2: Initialized; Kinect On, No Bodies Tracked
            else if (state == 2)
            {
                CurrentState.StatusColor = "Peru";
                CurrentState.StatusTitle = "STANDBY";
                CurrentState.Status = photo[1 + (((CurrentState.ImageOrientationFlag)-1)/(-2))];
            }
            //State 3: Initialized; Kinect On, Tracking
            else if (state == 3)
            {
                CurrentState.StatusColor = "Lime";
                CurrentState.StatusTitle = "TRACKING";
                CurrentState.Status = photo[3 + (((CurrentState.ImageOrientationFlag) - 1) / (-2))];
            }
        }

        private void Close()
        {
            _sessionManager.CloseAllConnections();
            _udpRec.Close();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Commands
        //Commands for the menu and buttons
        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(param => this.ExitCommandLogic());
                }
                return _exitCommand;
            }
        }
        private void ExitCommandLogic()
        {
            this.Close();
            //System.Windows.Application.Current.Shutdown();
        }

        private RelayCommand _flipCameraCommand;
        public RelayCommand FlipCameraCommand
        {
            get
            {
                if (_flipCameraCommand == null)
                {
                    _flipCameraCommand = new RelayCommand(param => this.FlipCameraCommandLogic());
                }
                return _flipCameraCommand;
            }
        }
        private void FlipCameraCommandLogic()
        {
            if (CurrentState.ImageOrientationFlag == 1)
            {
                CurrentState.ImageOrientationFlag = -1;
                //orientation_button.Style = Resources["MetroButton2"] as Style;
            }   
            else
            {
                CurrentState.ImageOrientationFlag = 1;
                //orientation_button.Style = Resources["MetroButton"] as Style;
            }
        }

        private RelayCommand _openGitHubCommand;
        public RelayCommand OpenGitHubCommand
        {
            get
            {
                if (_openGitHubCommand == null)
                {
                    _openGitHubCommand = new RelayCommand(param => this.OpenGitHubCommandLogic());
                }
                return _openGitHubCommand;
            }
        }
        private void OpenGitHubCommandLogic()
        {
            Process.Start("https://msarge2015.github.io/KiCASS");
        }

        private RelayCommand _aboutCommand;
        public RelayCommand AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                {
                    _aboutCommand = new RelayCommand(param => this.AboutCommandLogic());
                }
                return _aboutCommand;
            }
        }
        private void AboutCommandLogic()
        {
            //inserting about command logic here
            const string message = "KiCASS was created and developed by students from the University of British Columbia, Canada."
                + "\n\nRead more about the project at their blog: http://www.ubcimpart.wordpress.com."
                + "\n\nKiCASS ver.0.29. Released Jan 29, 2016 on the MIT License.";
            const string caption = "About KiCASS";
            var result = System.Windows.Forms.MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion // Commands

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}