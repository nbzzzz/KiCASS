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

        [Test]
        public void BuildDistanceMessageTest()
        {
            Joint joint = new Joint();
            joint.JointType = JointType.HandLeft;
            Joint joint2 = new Joint();
            joint2.JointType = JointType.HandRight;
            var msg = LaptopOrchestra.Kinect.OscSerializer.BuildDistanceMessage(joint,joint2);
            Assert.AreEqual("/kinect/distance/HandLeft/HandRight", msg.Address);
        }

        [Test]
        public void BuildVectorMessageTest()
        {
            Joint joint = new Joint();
            joint.JointType = JointType.HandLeft;
            Joint joint2 = new Joint();
            joint2.JointType = JointType.HandRight;
            var msg = LaptopOrchestra.Kinect.OscSerializer.BuildVectorMessage(joint, joint2);
            Assert.AreEqual("/kinect/vector/HandLeft/HandRight", msg.Address);
        }

        [Test]
        public void BuildHandMessageTest()
        {
            var msg = LaptopOrchestra.Kinect.OscSerializer.BuildLeftHandStateMessage(HandState.Closed);
            Assert.AreEqual("/kinect/handstate/left", msg.Address);

            msg = LaptopOrchestra.Kinect.OscSerializer.BuildRightHandStateMessage(HandState.Open);
            Assert.AreEqual("/kinect/handstate/right", msg.Address);
        }
    }
}
