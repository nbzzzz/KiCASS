using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaptopOrchestra.Kinect;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using NUnit.Framework;

namespace LaptopOrchestraTest
{
    class SessionWorkerTest
    {
        [Test]
        public void SessionWorkerInitializationTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), null);

            Assert.AreEqual(ip, sessionWorker.Ip);
            Assert.AreEqual(port, sessionWorker.Port);
            Assert.False(sessionWorker.ConfigFlags.IfAnyConfig());
        }

        [Test]
        public void SessionWorkerSetJointFlagsTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), null);

            sessionWorker.SetJointFlags("1111111111111111111111111".ToCharArray());
            Assert.False(sessionWorker.ConfigFlags.JointFlags.Any(pair => pair.Value == false));
            sessionWorker.SetJointFlags("0000000000000000000000000".ToCharArray());
            Assert.False(sessionWorker.ConfigFlags.JointFlags.Any(pair => pair.Value == true));
        }


        [Test]
        public void SessionWorkerSetHandFlagsTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), null);

            sessionWorker.SetHandFlag("11".ToCharArray());
            Assert.True(sessionWorker.ConfigFlags.HandStateFlag[HandType.LEFT]);
            Assert.True(sessionWorker.ConfigFlags.HandStateFlag[HandType.RIGHT]);
            sessionWorker.SetHandFlag("00".ToCharArray());
            Assert.False(sessionWorker.ConfigFlags.HandStateFlag[HandType.LEFT]);
            Assert.False(sessionWorker.ConfigFlags.HandStateFlag[HandType.RIGHT]);
        }
    }
}
