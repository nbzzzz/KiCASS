using System;
using System.Collections;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;

namespace LaptopOrchestra.Kinect
{
    class DataConsumer
    {

        public delegate void UpdateSkeleton(IDictionary<JointType, Joint> joints);

        /// <summary>
        /// Queue where the sensor will add data to
        /// </summary>
        private Queue<IDictionary<JointType, Joint>> queue;

        private Dictionary<JointType, bool> configurationFlags;

        /// <summary>
        /// Queue where the sensor will add data to
        /// </summary>
        private OscBuilder oscBuilder;

        public DataConsumer(Queue<IDictionary<JointType, Joint>> queue, Dictionary<JointType, bool> configurationFlags)
        {
            this.queue = queue;

            this.oscBuilder = new OscBuilder();

            this.configurationFlags = configurationFlags;

            UDP.ConfigureIpAndPort("127.0.0.1", 8000);

        }

        public void consume()
        {
            while (true)
            {
                //Console.WriteLine("Currently " + queue.Count + " in the queue");
                if(this.queue.Count >= 1)
                {
                    // Pop the queue from the kinect
                    IDictionary<JointType, Joint> Joints = this.queue.Dequeue();

                    // Check the list of joints to send (is this a reference or copy?)
                    //ObservableCollection<ListJoint> JointSendList = configurationTool.getJointList();
                    var JointSendList = configurationFlags.Where(x => x.Value == true).Select(cf => cf.Key);

                    foreach (var jt in JointSendList)
                    {
                        //if ( jt.send && Joints != null )
                        //{
                            try
                            {
                                var position = Joints[jt].Position;

                                var joint = oscBuilder.BuildJointMessage(Joints[jt]);

                                UDP.SendMessage(joint);
                            }
                            catch
                            {
                                // TODO: What needs to be done if the joint type isn't found 
                            }
                            


                        //}
                    }
                }
                else
                {
                    //TODO: Monitor the size of the queue if we should decrease this
                }
            }
        }
    }
}
