using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.ComponentModel;

//BACKGROUND THREAD
namespace LaptopOrchestra.Kinect.Model
{
    public class BackgroundThread //: INotifyPropertyChanged
    {
        #region Properties/Fields

        //List of tabs in gui
        private TabList _tabList;
        
        //Local list of tabs
        List<TabData> _localSessions;
        
        //Used to fixed alignment issue between skeleton positional data and color image
        //private CoordinateMapper _coordinateMapper;
        
        //Copy of session manager to get open connections
        private SessionManager _sessionManager;
        
        //Timer
        private System.Timers.Timer _timer;

        #endregion

        #region Contructor

        public BackgroundThread(SessionManager sessionManager, KinectProcessor kinectProcessor)
        {
            ///*
            _sessionManager = sessionManager;

            //NEEDED??
            _localSessions = new List<TabData>();
            _tabList = new TabList();

            // Start timer for flag updating thread
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            //*/
            
        }

        #endregion

        #region Helper Functions

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

        /*
        private void updateTabs()
        {
            
            this.Dispatcher.Invoke((Action)(() =>
            {
                // Maintain the current tab selection
                int currentSelected = 0;
                if (_tabList.getTabs().Count > 1)
                {
                    //currentSelected = tabControl.SelectedIndex;
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
                //tabControl.ItemsSource = _tabList;

                // Restore selection
                //tabControl.SelectedIndex = currentSelected;
            }));
            */
    }

    #endregion
}
