using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LaptopOrchestra.Kinect
{
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectSimulator
    /// </summary>
    public partial class KinectSimulator : Window
    {

        /// <summary>
        /// Queue to communicate between Kinect Simulator and Data Comsumer
        /// </summary>
        private Queue<IReadOnlyDictionary<JointType, Joint>> queue = null;

        /// <summary>
        /// IDictionaryObject representing our body
        /// </summary>
        private IDictionary<JointType, Joint> joints = null;

        public KinectSimulator(Queue<IReadOnlyDictionary<JointType, Joint>> queue)
        {
            // Set the communication queue
            this.queue = queue;

            // Initalize our skeleton
            this.joints = new Dictionary<JointType, Joint>();

            //var JointTypes = Enum.GetValues(typeof(JointType));
            var JointTypes = Enum.GetValues(typeof(JointType)).Cast<JointType>(); ;

            foreach (var jt in JointTypes)
            {
                CameraSpacePoint position = new CameraSpacePoint();
                position.X = 10;
                position.Y = 10;
                position.Z = 10;

                Joint joint = new Joint();
                joint.JointType = jt;
                joint.TrackingState = TrackingState.Tracked;
                joint.Position = position;

                joints.Add(jt, joint);
            }

            InitializeComponent();
        }

        private void LeftHandLeft(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandLeft];
            joint.Position.X--;
            joints[JointType.HandLeft] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void LeftHandRight(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandLeft];
            joint.Position.X++;
            joints[JointType.HandLeft] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void LeftHandUp(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandLeft];
            joint.Position.Y++;
            joints[JointType.HandLeft] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void LeftHandDown(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandLeft];
            joint.Position.Y--;
            joints[JointType.HandLeft] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void RightHandLeft(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandRight];
            joint.Position.X--;
            joints[JointType.HandRight] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void RightHandRight(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandRight];
            joint.Position.X++;
            joints[JointType.HandRight] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void RightHandUp(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandRight];
            joint.Position.Y++;
            joints[JointType.HandRight] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }

        private void RightHandDown(object sender, RoutedEventArgs e)
        {
            var joint = joints[JointType.HandRight];
            joint.Position.Y--;
            joints[JointType.HandRight] = joint;

            queue.Enqueue((IReadOnlyDictionary<JointType, Joint>)joints);
        }
    }
}