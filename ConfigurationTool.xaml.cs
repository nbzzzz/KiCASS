using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{

    public partial class ConfigurationTool : Window
    {
        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        IList<Body> _bodies;
        Queue<IDictionary<JointType, Joint>> queue;

        Dictionary<JointType, bool> configurationFlags;

        bool _displayBody = false;
        // END: KinectProcess vars

        public ConfigurationTool(Queue<IDictionary<JointType, Joint>> queue, Dictionary<JointType, bool> configurationFlags) // TODO: remove arg and place into KinectProcessor
        {
            InitializeComponent();
            this.configurationFlags = configurationFlags;

            var jointTypes = Enum.GetValues(typeof(JointType));

            foreach (JointType jt in jointTypes) {
                lvJoints.Items.Add(jt);
                configurationFlags[jt] = false;
            }
            
            this.queue = queue;
            startKinect();

        }

        #region UI Event Listeners

        private void lvJoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = false;
            }

            foreach ( var item in lvJoints.SelectedItems)
            {
                JointType jt = (JointType)Enum.Parse(typeof(JointType), item.ToString(), true);
                configurationFlags[jt] = true;
            }

        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = true;
            }
            lvJoints.SelectAll();
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            var jointTypes = Enum.GetValues(typeof(JointType));
            foreach (JointType jt in jointTypes)
            {
                configurationFlags[jt] = false;
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

            }
        }

        #endregion


        // TODO: Start of KinectProcessor -- must be moved into its own class once GUI can be sorted

        #region Event handlers

        private void startKinect()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _displayBody = true;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
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
                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body != null && body.IsTracked)
                        {
                            IDictionary<JointType, Joint> newJoint = new Dictionary<JointType, Joint>();

                            foreach (var joint in body.Joints)
                            {
                                newJoint.Add(joint.Value.JointType, joint.Value);
                            }

                            this.queue.Enqueue(newJoint);

                            if (body.IsTracked)
                            {
                                // Draw skeleton.
                                if (_displayBody)
                                {
                                    canvas.DrawSkeleton(body, configurationFlags);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
