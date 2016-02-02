using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using NUnit.Framework;

namespace LaptopOrchestraTest
{
    class OscSerializerTest
    {
        [Test]
        public void BuildJointMessageTest()
        {
            Joint joint = new Joint();
            joint.JointType = JointType.HandLeft;
            var msg = LaptopOrchestra.Kinect.OscSerializer.BuildJointMessage(joint);
            Assert.AreEqual("/kinect/joint/HandLeft", msg.Address);
        }
    }
}
