using LaptopOrchestra.Kinect;
using NUnit.Framework;
using Rug.Osc;

namespace LaptopOrchestraTest
{
    class OscDeserializerTest
    {

        string[] msg = new[] { "/kinect/joint", "127.0.0.1", "8080", "1111111111111111111111111" };

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

        [Test]
        public void GetAddressTest()
        {

            Assert.AreEqual(2, OscDeserializer.GetMessageAddress(msg).Length);
            Assert.AreEqual("kinect",OscDeserializer.GetMessageAddress(msg)[0]);
            Assert.AreEqual("joint", OscDeserializer.GetMessageAddress(msg)[1]);
        }

        [Test]
        public void GetIpTest()
        {
            Assert.AreEqual("127.0.0.1", OscDeserializer.GetMessageIp(msg));
        }

        [Test]
        public void GetPortTest()
        {
            Assert.AreEqual("127.0.0.1", OscDeserializer.GetMessageIp(msg));
        }

        [Test]
        public void GetBinSeqTest()
        {
            Assert.AreEqual("1111111111111111111111111", OscDeserializer.GetMessageBinSeq(msg));
        }
    }
}
