using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using Rug.Osc;

namespace LaptopOrchestra.Kinect
{
    public class DataSubscriber
    {
        /// <summary>
        ///     Configuration Flags Object
        /// </summary>
        private readonly ConfigFlags _configurationFlags;

        /// <summary>
        ///     Bodies that will containt the data from the Kinect
        /// </summary>
        private Body[] _bodies;

		private UDPSender _dataSender;


        public DataSubscriber(ConfigFlags configurationFlags, KinectProcessor kinectProcessor, UDPSender dataSender)
        {
            _configurationFlags = configurationFlags;

			_dataSender = dataSender;
			_dataSender.StartDataOut();

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

					var packetCount = 0;
					OscPacket[] messagePackets = new OscPacket[27];

                    foreach (var joint in body.Joints)
                    {
                        if (!_configurationFlags.JointFlags[joint.Key]) continue;

                        var jointMessage = OscSerializer.BuildJointMessage(joint.Value);
						//_dataSender.SendMessage(jointMessage);
						messagePackets[packetCount] = jointMessage;
						packetCount++;
					}

                    if (_configurationFlags.HandStateFlag[HandType.LEFT])
                    {
                        var handMessage = OscSerializer.BuildLeftHandStateMessage(body.HandLeftState);
						//_dataSender.SendMessage(handMessage);
						messagePackets[packetCount] = handMessage;
						packetCount++;
					}

                    if (_configurationFlags.HandStateFlag[HandType.RIGHT])
                    {
                        var handMessage = OscSerializer.BuildRightHandStateMessage(body.HandLeftState);
						//_dataSender.SendMessage(handMessage);
						messagePackets[packetCount] = handMessage;
						packetCount++;
					}

					OscBundle bundle = new OscBundle(DateTime.Now, messagePackets);
					_dataSender.SendBundle(bundle);
					return;
                }
            }
        }
    }
}