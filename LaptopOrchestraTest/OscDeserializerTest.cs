using LaptopOrchestra.Kinect;
using NUnit.Framework;

namespace LaptopOrchestraTest
{
    class OscDeserializerTest
    {
        [Test]
        public void OscValidPacketTest()
        {
            Assert.True(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.0.1"", 8080, ""1111111111111111111111111"""));
            Assert.True(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.0.1"", 1, ""0000000000000000000000000"""));
            Assert.True(OscDeserializer.IsValid(@"/kinect/handstate, ""127.0.0.1"", 8080, ""11"""));
            Assert.True(OscDeserializer.IsValid(@"/kinect/handstate, ""127.0.0.1"", 1, ""00"""));

            //Invalid Ip
            Assert.False(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.1"", 8080, ""1111111111111111111111111"""));

            //Invalid Port
            Assert.False(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.0.1"", 99999, ""1111111111111111111111111"""));

            //Invalid Number of Joints
            Assert.False(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.0.1"", 8080, ""11111111111111111111111"""));
            Assert.False(OscDeserializer.IsValid(@"/kinect/joint, ""127.0.0.1"", 8080, ""11111111111111111110001111"""));
           
            //Invalid Address
            Assert.False(OscDeserializer.IsValid(@"/kinect/joi, ""127.0.0.1"", 8080, ""1111111111111111111111111"""));
        }
    }
}
