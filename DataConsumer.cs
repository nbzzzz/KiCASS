using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace LaptopOrchestra.Kinect
{
    public class DataConsumer
    {
        /// <summary>
        /// Queue where the sensor will add data to
        /// </summary>
        private readonly Queue<IDictionary<JointType, Joint>> _queue;

        /// <summary>
        /// Configuration Flags Object
        /// </summary>
        private readonly Dictionary<JointType, bool> _configurationFlags;

        /// <summary>
        /// OSC message builder
        /// </summary>
        private readonly OscBuilder _oscBuilder;

        public DataConsumer(Queue<IDictionary<JointType, Joint>> queue, Dictionary<JointType, bool> configurationFlags)
        {
            _queue = queue;

            _oscBuilder = new OscBuilder();

            _configurationFlags = configurationFlags;

            UDP.ConfigureIpAndPort("127.0.0.1", 8000);

        }

        public void Consume()
        {
            while (true)
            {
                if(_queue.Count >= 1)
                {
                    IDictionary<JointType, Joint> joints = _queue.Dequeue();

                    // Check the list of joints to send (is this a reference or copy?)
                    //ObservableCollection<ListJoint> JointSendList = configurationTool.getJointList();
                    var jointSendList = _configurationFlags.Where(x => x.Value).Select(cf => cf.Key);

                    foreach (var jt in jointSendList)
                    {
                            try
                            {
                                var position = joints[jt].Position;

                                var joint = _oscBuilder.BuildJointMessage(joints[jt]);

                                UDP.SendMessage(joint);
                            }
                            catch
                            {
                                // TODO: What needs to be done if the joint type isn't found 
                            }
                    }
                }
                else
                {
                    //TODO: Monitor the size of the _queue if we should decrease this
                }
            }
        }
    }
}
