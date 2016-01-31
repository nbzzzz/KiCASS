using LaptopOrchestra.Kinect.Model;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

//UI THREAD
namespace LaptopOrchestra.Kinect.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Class Attributes

        protected static string[][] allStates =
            new string[][] {
                new string[] { "LOADING", "Orange", "Red" },
                new string[] { "STANDBY", "Yellow" },
                new string[] { "ACTIVE", "Chartreuse" },
                new string[] { "KINECT UNPLUGGED", "Red" }
            };

        protected static string[] allSessions = new string[]
        {
            "Camera: SHOW",
            "Camera: HIDE",
            "Orientation: MIRRORED",
            "Orientation: TRUE",
            "111111111111:8000",
            "222222222222:8000"
        };

        protected static string[][] allJointLists =
            new string[][] {
                new string[] { "Joint1", "Joint2", "Joint3" },
                new string[] { "Joint1", "Joint2", "Joint3" }
            };

        #endregion

        #region Public Properties

        //Used to fixed alignment issue between skeleton positional data and color image
        private CoordinateMapper _coordinateMapper;

        //Copy of session manager to get open connections
        private SessionManager _sessionManager;

        //List of bodies
        private IList<Body> _bodies;
        
        protected System.Windows.Media.ImageSource _myFrame;
        public System.Windows.Media.ImageSource MyFrame
        {
            get { return _myFrame; }
            set
            {
                if (_myFrame != value)
                {
                    _myFrame = value;
                    //UpdateJointList();
                    NotifyPropertyChanged("MyFrame");
                }
            }
        }

        private State currentState;
        public State CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                NotifyPropertyChanged("CurrentState");
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel(SessionManager sessionManager, KinectProcessor kinectProcessor)
        {
            //Initialize the default window state
            CurrentState = State.CreateNewState();

            //Start BackgroundThread
            //CurrentState.UpdateState();

            //UI Thread
            _sessionManager = sessionManager;
            _coordinateMapper = kinectProcessor.CoordinateMapper;
            kinectProcessor.Reader.MultiSourceFrameArrived += UI_Thread;
        }

        #endregion

        #region Helper Functions

        protected void UpdateJointList()
        {
            /*
            int selectedTabVariable;
            for (selectedTabVariable = 0; selectedTabVariable < allSessions.Length; selectedTabVariable++)
            {
                if (allSessions[selectedTabVariable] == SelectedTab)
                    break;
            }

            if (selectedTabVariable == allSessions.Length)// just in case of a bug
                CurrentJointList = null;
            else
                CurrentJointList = null;
                //CurrentJointList = AllJointLists[selectedTabVariable];
                */
        }

        private void UI_Thread(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            //UpdateJointList();
            //hacky line of code to enable editing element from different xaml file
            var mainWin = System.Windows.Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            var reference = e.FrameReference.AcquireFrame();

            //Draw image only if Toggle is ON.
            if (CurrentState.CameraToggleFlag)
            {
                // Draw the Image from the Camera
                using (var frame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        //CurrentState.StatusTitle = "STANDBY";
                        CurrentState.StatusColor = "Peru";
                        mainWin.XAMLImage.Source = frame.ToBitmap();
                        //MyFrame = frame.ToBitmap();
                    }
                }

            }

            //If Toggle is OFF, clear frame from sight.
            else
            {
                mainWin.XAMLImage.Source = null;
                //MyFrame = null;
            }

            // Acquire skeleton data as well
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame == null) return;

                //mainWin.XAMLCanvas.Children.Clear();

                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

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
                        {
                            position.Z = 0.01f;
                        }

                        ColorSpacePoint colorPoint = _coordinateMapper.MapCameraPointToColorSpace(position);

                        alignedJointPoints[jointType] = new System.Windows.Point(colorPoint.X, colorPoint.Y);
                    }

                    //mainWin.XAMLCanvas.DrawSkeleton(body, alignedJointPoints, isFirst);
                }
            }
        }

        protected void SetStatusColor(int color)
        {
            if (color == 0)
            {
                //this.??
                //textBlock0.Background = Brushes.OrangeRed;
                //textBlock1.Background = Brushes.OrangeRed;
                //textBlock2.Background = Brushes.OrangeRed;
            }
            else if (color == 1)
            {
                //this.??
                //textBlock0.Background = Brushes.OrangeRed;
                //textBlock1.Background = Brushes.OrangeRed;
                //textBlock2.Background = Brushes.OrangeRed;
            }
            else if (color == 2)
            {
                //this.??
                //textBlock0.Background = Brushes.OrangeRed;
                //textBlock1.Background = Brushes.OrangeRed;
                //textBlock2.Background = Brushes.OrangeRed;
            }
        }

        #endregion

        #region Commands

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
            //CurrentState.UpdateState();
            System.Windows.Application.Current.Shutdown();
        }

        private RelayCommand _toggleCameraCommand;
        public RelayCommand ToggleCameraCommand
        {
            get
            {
                if (_toggleCameraCommand == null)
                {
                    _toggleCameraCommand = new RelayCommand(param => this.ToggleCameraCommandLogic());
                }
                return _toggleCameraCommand;
            }
        }
        private void ToggleCameraCommandLogic()
        {
            if (CurrentState.CameraToggleFlag)
            {
                CurrentState.CameraButtonText = allSessions[1];
                //camera_button.Style = Resources["MetroButton2"] as Style;
            }
            else
            {
                CurrentState.CameraButtonText = allSessions[0];
                //camera_button.Content = Properties.Resources.CameraStateShow;
                //camera_button.Style = Resources["MetroButton"] as Style;
            }
            CurrentState.CameraToggleFlag = !CurrentState.CameraToggleFlag;

            //CurrentState.UpdateState();
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
                CurrentState.OrientationButtonText = allSessions[3];
                //current_orientation_state_string = Properties.Resources.OrientStateTrue;
                //orientation_button.Content = Properties.Resources.OrientStateTrue;
                //orientation_button.Style = Resources["MetroButton2"] as Style;
            }
            else
            {
                CurrentState.ImageOrientationFlag = 1;
                CurrentState.OrientationButtonText = allSessions[2];
                //current_orientation_state_string = Properties.Resources.OrientStateMirror;
                //orientation_button.Content = Properties.Resources.OrientStateMirror;
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
            Process.Start("https://github.com/ijcheng/CapstoneKinectLaptopOrchestra#kicass-kinect-controlled-artistic-sensing-system");
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