using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{

    public partial class ConfigurationTool : Window
    {
        /// <summary>
        /// List of bodies
        /// </summary>
        private IList<Body> _bodies;

        /// <summary>
        /// Configuration Items selected
        /// </summary>
        private readonly Dictionary<JointType, bool> _configurationFlags;

        public ConfigurationTool(Dictionary<JointType, bool> configurationFlags, KinectProcessor kinectProcessor)
        {
            InitializeComponent();

            _configurationFlags = configurationFlags;

            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes) {
                lvJoints.Items.Add(jt);
                configurationFlags[jt] = false;
           }

            kinectProcessor.Reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

        }

        #region UI Event Listeners

        private void lvJoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                _configurationFlags[jt] = false;
            }

            foreach ( var item in lvJoints.SelectedItems)
            {
                JointType jt = (JointType)Enum.Parse(typeof(JointType), item.ToString(), true);
                _configurationFlags[jt] = true;
            }

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                _configurationFlags[jt] = true;
            }
            lvJoints.SelectAll();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                _configurationFlags[jt] = false;
            }
            lvJoints.UnselectAll();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var ip = IP.Text;
            try {
                var port = int.Parse(Port.Text);
                UDP.ConfigureIpAndPort(ip, port);
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            //TODO subscribe KinectProcess.Stop() and UDP.stop() to this event; maybe this can go inside App.xaml.cs instead
        }

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Draw the Image from the camera
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }

            // Acquire skeleton data as well
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame == null) return;

                canvas.Children.Clear();

                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

                foreach (var body in _bodies)
                {
                    if (body == null || !body.IsTracked) continue;

                    IDictionary<JointType, Joint> newJoint = new Dictionary<JointType, Joint>();

                    foreach (var joint in body.Joints)
                    {
                        newJoint.Add(joint.Value.JointType, joint.Value);
                    }

                    if (!body.IsTracked) continue;

                    // Draw skeleton.
                    canvas.DrawSkeleton(body, _configurationFlags);
                }
            }
        }
    }
}
