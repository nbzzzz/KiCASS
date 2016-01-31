using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using LaptopOrchestra.Kinect.ViewModel;
using LaptopOrchestra.Kinect.Properties;
using System.Threading;
using System.Timers;
using Microsoft.Kinect.Input;

namespace LaptopOrchestra.Kinect.Model
{
    public class State : ViewModelBase
    {
        #region Properties

        //Copy of session manager to get open connections
        private SessionManager _sessionManager;

        //Timer
        private System.Timers.Timer _timer;

        //List of tabs in gui
        private TabList _tabList;

        //Local list of tabs
        List<TabData> _localSessions;

        private string _statusTitle;
        public string StatusTitle
        {
            get { return _statusTitle; }
            set
            {
                if (value != _statusTitle)
                {
                    _statusTitle = value;
                    OnPropertyChanged("StatusTitle");
                }
            }
        }

        private string _statusColor;
        public string StatusColor
        {
            get { return _statusColor; }
            set
            {
                if (value != _statusColor)
                {
                    _statusColor = value;
                    OnPropertyChanged("StatusColor");
                }
            }
        }

        private string[] _sessionsList;
        public string[] SessionsList
        {
            get { return _sessionsList; }
            set
            {
                if (value != _sessionsList)
                {
                    _sessionsList = value;
                    OnPropertyChanged("SessionsList");
                }
            }
        }

        private string[] _jointList;
        public string[] JointList
        {
            get { return _jointList; }
            set
            {
                if (value != _jointList)
                {
                    _jointList = value;
                    OnPropertyChanged("JointList");
                }
            }
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value != _selectedTab)
                {
                    _selectedTab = value;
                    OnPropertyChanged("SelectedTab");
                }
            }
        }

        private bool _cameraToggleFlag;
        public bool CameraToggleFlag
        {
            get { return _cameraToggleFlag; }
            set
            {
                if (value != _cameraToggleFlag)
                {
                    _cameraToggleFlag = value;
                    OnPropertyChanged("CameraToggleFlag");
                }
            }
        }

        private int _imageOrientationFlag;
        public int ImageOrientationFlag
        {
            get { return _imageOrientationFlag; }
            set
            {
                if (value != _imageOrientationFlag)
                {
                    _imageOrientationFlag = value;
                    OnPropertyChanged("ImageOrientationFlag");
                }
            }
        }

        private string _cameraButtonText;
        public string CameraButtonText
        {
            get { return _cameraButtonText; }
            set
            {
                if (value != _cameraButtonText)
                {
                    _cameraButtonText = value;
                    OnPropertyChanged("CameraButtonText");
                }
            }
        }

        private string _orientationButtonText;
        public string OrientationButtonText
        {
            get { return _orientationButtonText; }
            set
            {
                if (value != _orientationButtonText)
                {
                    _orientationButtonText = value;
                    OnPropertyChanged("OrientationButtonText");
                }
            }
        }

        private string _backgroundThreadStatus;
        public string BackgroundThreadStatus
        {
            get { return _backgroundThreadStatus; }
            set
            {
                if (value != _backgroundThreadStatus)
                {
                    _backgroundThreadStatus = value;
                    OnPropertyChanged("BackgroundThreadStatus");
                }
            }
        }

        #endregion //Properties

        #region Functions

        public static State CreateNewState()
        {
            return new State
            {
                StatusColor = "Orange",
                StatusTitle = "LOADING",
                SessionsList = null,
                JointList = null,
                SelectedTab = 0,
                CameraToggleFlag = true,
                ImageOrientationFlag = 1,
                CameraButtonText = "Camera: SHOW",
                OrientationButtonText = "Orientation: MIRRORED"
            };
        }
    
        protected State() { }

        public void UpdateState(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _localSessions = new List<TabData>();
            _tabList = new TabList();

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            updateFlags();
        }

        private void updateFlags()
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            var handTypes = Enum.GetValues(typeof(HandType));

            // Copy session workers so iteration doesnt break the collection
            SessionWorker[] workers = new SessionWorker[_sessionManager.OpenConnections.Count];
            _sessionManager.OpenConnections.CopyTo(workers);

            foreach (SessionWorker sw in workers)
            {
                // Get the port and IP of this session
                string id = sw.Ip + ":" + sw.Port.ToString();

                // Copy the flags
                ConfigFlags flags = sw.ConfigFlags;

                // Get the list of active joints
                List<string> jointList = new List<string>();
                foreach (JointType jt in jointTypes)
                {
                    if (flags.JointFlags[jt])
                    {
                        jointList.Add(jt.ToString());
                    }
                }

                //Get the active Handstate flags
                if (flags.HandStateFlag[HandType.LEFT]) jointList.Add("LeftHandState");
                if (flags.HandStateFlag[HandType.RIGHT]) jointList.Add("RightHandState");

                // If this session already exists update the flags
                if (_localSessions.Exists(tab => tab.Header == id))
                {
                    _localSessions.Find(tab => tab.Header.Equals(id)).displayFlags = flags.JointFlags;
                    _localSessions.Find(tab => tab.Header.Equals(id)).Items = jointList;
                    _localSessions.Find(tab => tab.Header.Equals(id)).Active = true;
                }
                else
                {
                    TabData tabData = new TabData(id, jointList, flags.JointFlags, true);
                    _localSessions.Add(tabData);
                }
            }
        }

        #endregion
    }
}