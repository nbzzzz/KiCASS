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

        public delegate void UpdateSkeleton(IReadOnlyDictionary<JointType, Joint> joints);

        /// <summary>
        /// Queue where the sensor will add data to
        /// </summary>
        private Queue<IReadOnlyDictionary<JointType, Joint>> queue;

        /// <summary>
        /// Configuration tool GUI to access selected flags
        /// Should this only be an exposure to getJointList?
        /// </summary>
        private ConfigurationTool configurationTool;

        public DataConsumer(Queue<IReadOnlyDictionary<JointType, Joint>> queue, ConfigurationTool configurationTool)
        {
            this.queue = queue;

            this.configurationTool = configurationTool;

        }

        public void consume()
        {
            while (true)
            {
                Console.WriteLine("Currently " + queue.Count + " in the queue");
                if(this.queue.Count >= 1)
                {
                    // Pop the queue from the kinect
                    IReadOnlyDictionary<JointType, Joint> Joints = this.queue.Dequeue();
                    // Check the list of joints to send (is this a reference or copy?)
                    ObservableCollection<ListJoint> JointSendList = configurationTool.getJointList();
                    foreach (var jt in JointSendList)
                    {
                        if ( jt.send )
                        {
                            var position = Joints[jt.jointType].Position;
                            Console.WriteLine("'{0}: ({1}, {2}, {3})", jt.jointType, position.X, position.Y, position.Z);
                        }
                    }
                }
                else
                {
                    //TODO: Monitor the size of the queue if we should decrease this
                    //Thread.Sleep(5000);
                }
            }
        }
    }
}
