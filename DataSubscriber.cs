using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace LaptopOrchestra.Kinect
{
    public class DataSubscriber
    {
        /// <summary>
        /// Configuration Flags Object
        /// </summary>
        private readonly Dictionary<JointType, bool> _configurationFlags;

        /// <summary>
        /// OSC message builder
        /// </summary>
        private readonly OscBuilder _oscBuilder;

        /// <summary>
        /// Bodies that will containt the data from the Kinect
        /// </summary>
        private Body[] _bodies;


        public DataSubscriber(Dictionary<JointType, bool> configurationFlags, KinectProcessor kinectProcessor)
        {
            _oscBuilder = new OscBuilder();

            _configurationFlags = configurationFlags;

            UDP.ConfigureIpAndPort("127.0.0.1", 8000);

            kinectProcessor.Reader.MultiSourceFrameArrived += MultiSourceFrameHandler;
        }

        public void MultiSourceFrameHandler(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Acquire skeleton data
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame == null) return;

                _bodies = new Body[frame.BodyFrameSource.BodyCount];

                frame.GetAndRefreshBodyData(_bodies);

                foreach (var body in _bodies)
                {
                    if (body == null || !body.IsTracked) continue;

                    foreach (var joint in body.Joints)
                    {
                        if (!_configurationFlags[joint.Key]) continue;

                        var jointMessage = _oscBuilder.BuildJointMessage(joint.Value);
                        UDP.SendMessage(jointMessage);
                    }
                }
            }
        }
    }

}
