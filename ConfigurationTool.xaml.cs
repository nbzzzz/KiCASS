using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{
    public partial class ConfigurationTool : Window
    {
        /// <summary>
        ///     Configuration Items selected
        /// </summary>
        private readonly Dictionary<JointType, bool> _configurationFlags;

        /// <summary>
        ///     List of bodies
        /// </summary>
        private IList<Body> _bodies;

        /// <summary>
        ///     Used to fixed alignment issue between skeleton positional data and color image
        /// </summary>
        private CoordinateMapper _coordinateMapper;

        public ConfigurationTool(Dictionary<JointType, bool> configurationFlags, KinectProcessor kinectProcessor)
        {
            InitializeComponent();

            _configurationFlags = configurationFlags;
            _coordinateMapper = kinectProcessor.CoordinateMapper;

            var jointTypes = Enum.GetValues(typeof (JointType));
            foreach (JointType jt in jointTypes)
            {
                lvJoints.Items.Add(jt);
                configurationFlags[jt] = false;
            }

            kinectProcessor.Reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
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
                    Camera.Source = frame.ToBitmap();
                }
            }

            // Acquire skeleton data as well
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame == null) return;

                Canvas.Children.Clear();

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


                    // Draw skeleton.
                    Canvas.DrawSkeleton(body, alignedJointPoints, _configurationFlags);
                }
            }
        }
    }
}