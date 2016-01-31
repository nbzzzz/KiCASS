using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using LaptopOrchestra.Kinect.Properties;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect.Model
{
    /// <summary>
    /// Represents a single tabbed session with its list of joints.
    /// It is wrapped by the TabsViewModel class, which enables it to
    /// be easily displayed and edited by a WPF user interface.
    /// </summary>
    public class Tab
    {
        #region Properties

        public string Header { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        //public int ItemListWidth { get; set; }
        public List<String> Items { get; set; }
        public Dictionary<JointType, bool> DisplayFlags { get; set; }
        public bool Active { get; private set; }

        #endregion //Properties

        #region Contructor?

        public static Tab CreateNewTab()
        {
            return new Tab();
        }

        public static Tab CreateTab(
            string header,
            int height,
            int width,
            List<String> items,
            Dictionary<JointType, bool> displayFlags,
            bool active)
        {
            return new Tab
            {
                Header = header,
                Height = height,
                Width = width,
                Items = items,
                DisplayFlags = displayFlags,
                Active = active
            };
        }

        protected Tab()
        {
        }

        #endregion // Creation
    }
}