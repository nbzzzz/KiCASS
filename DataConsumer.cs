using System;
using System.Collections;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LaptopOrchestra.Kinect
{
    class DataConsumer
    {
        /// <summary>
        /// Queue where the sensor will add data to
        /// </summary>
        private Queue<IReadOnlyDictionary<JointType, Joint>> queue;

        /// <summary>
        /// Flags to indidcate which data set to send to OSC
        /// </summary>
        private ConfigurationFlags configurationFlags = new ConfigurationFlags();

        public DataConsumer(Queue<IReadOnlyDictionary<JointType, Joint>> queue, ConfigurationFlags configurationFlags)
        {
            this.queue = queue;
            this.configurationFlags = configurationFlags;
        }

        public void consume()
        {
            while (true)
            {
                Console.WriteLine("Currently " + queue.Count + " in the queue");
                if(this.queue.Count >= 1)
                {
                    IReadOnlyDictionary<JointType, Joint> Joints = this.queue.Dequeue();
                    
                    var position = Joints[JointType.HandLeft].Position;
                    if (configurationFlags.leftHand)
                    {
                        Console.WriteLine("Left Hand: ({0}, {1}, {2})", position.X, position.Y, position.Z);
                    }

                    position = Joints[JointType.HandRight].Position;
                    if (configurationFlags.rightHand) {
                        Console.WriteLine("Right Hand: ({0}, {1}, {2})", position.X, position.Y, position.Z);
                    }
                }
                else
                {
                    //TODO: Monitor the size of the queue if we should decrease this
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
