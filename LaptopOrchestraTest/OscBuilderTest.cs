using NUnit.Framework;
using LaptopOrchestra.Kinect;
using Microsoft.Kinect;

namespace LaptopOrchestraTest
{
    [TestFixture]
    public class OscBuilderTest
    {
        [Test]
        public void BuildJointMessageTest()
        {
            Joint joint = new Joint();
            joint.JointType = JointType.HandLeft;
            var msg = OscSerializer.BuildJointMessage(joint);

            Assert.AreEqual("/kinect/joint/HandLeft", msg.Address);
        }

    }
}
