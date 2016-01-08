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
            OscBuilder oscBuilder = new OscBuilder();
            Joint joint = new Joint();
            joint.JointType = JointType.HandLeft;
            var msg = oscBuilder.BuildJointMessage(joint);

            Assert.AreEqual("/kinect/joint/HandLeft", msg.Address);
        }

    }
}
