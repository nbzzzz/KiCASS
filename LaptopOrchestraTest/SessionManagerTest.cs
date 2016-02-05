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
    class SessionManagerTest
    {
        [Test]
        public void SessionManagerAddTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            SessionManager sessionManager = new SessionManager();

            Assert.AreEqual(0,sessionManager.OpenConnections.Count);
            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), sessionManager);
            sessionManager.AddConnection(sessionWorker);
            Assert.AreEqual(1, sessionManager.OpenConnections.Count);
            Assert.AreEqual(ip, sessionManager.OpenConnections[0].Ip);
            Assert.AreEqual(port, sessionManager.OpenConnections[0].Port);
        }

        [Test]
        public void SessionManagerCloseAllTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            String ip2 = "127.0.0.1";
            int port2 = 8081;

            SessionManager sessionManager = new SessionManager();

            Assert.AreEqual(0, sessionManager.OpenConnections.Count);
            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), sessionManager);
            sessionManager.AddConnection(sessionWorker);
            sessionWorker.SetTimers();

            SessionWorker sessionWorker2 = new SessionWorker(ip2, port2, new KinectProcessor(), sessionManager);
            sessionManager.AddConnection(sessionWorker2);
            sessionWorker2.SetTimers();
            Assert.AreEqual(2, sessionManager.OpenConnections.Count);

            sessionManager.CloseAllConnections();
            Assert.AreEqual(0, sessionManager.OpenConnections.Count);

        }

        [Test]
        public void SessionManagerCleanTest()
        {
            String ip = "127.0.0.1";
            int port = 8080;

            String ip2 = "127.0.0.1";
            int port2 = 8081;

            SessionManager sessionManager = new SessionManager();

            Assert.AreEqual(0, sessionManager.OpenConnections.Count);
            SessionWorker sessionWorker = new SessionWorker(ip, port, new KinectProcessor(), sessionManager);
            sessionManager.AddConnection(sessionWorker);
            sessionWorker.SetTimers();

            Assert.AreEqual(1, sessionManager.OpenConnections.Count);
            sessionWorker.EndSession = true;
            Assert.AreEqual(0, sessionManager.OpenConnections.Count);
        }
    }
}
