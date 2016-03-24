using LaptopOrchestra.Kinect.ViewModel;
using System.ComponentModel;

namespace LaptopOrchestra.Kinect.Model
{
    public class MainWindowModel : ViewModelBase
    {
        #region Properties
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

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

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
        #endregion

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