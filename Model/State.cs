using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using LaptopOrchestra.Kinect.ViewModel;
using LaptopOrchestra.Kinect.Properties;

namespace LaptopOrchestra.Kinect.Model
{
    public class State : ViewModelBase
    {
        #region Properties

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

        #endregion //Properties

        #region Creation

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

        public void UpdateState()
        {
            //start thread
            StatusTitle = "thread runnning";
        }

        protected State() { }

        #endregion // Creation
    }
}