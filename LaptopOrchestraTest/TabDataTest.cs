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
    class TabDataTest
    {
        [Test]
        public void TabDataInitTest()
        {
            List<String> jointList = new List<string>();
            jointList.Add(JointType.AnkleLeft.ToString());
            jointList.Add(JointType.AnkleRight.ToString());


            ConfigFlags configFlags = new ConfigFlags();
            configFlags.JointFlags[JointType.HandLeft] = true; 

            TabData tabData = new TabData("Header", jointList, configFlags.JointFlags, true);

            Assert.AreEqual("Header", tabData.Header);
            Assert.True(tabData.Active);
            Assert.AreEqual(2, tabData.Items.Count);
           
        }

        [Test]
        public void TabDataInitOtherTest()
        {
            List<String> jointList = new List<string>();
            jointList.Add(JointType.AnkleLeft.ToString());
            jointList.Add(JointType.AnkleRight.ToString());


            ConfigFlags configFlags = new ConfigFlags();
            configFlags.JointFlags[JointType.HandLeft] = true;

            TabData tabData = new TabData("Header", 10, 15, 20, jointList, configFlags.JointFlags);

            Assert.AreEqual("Header", tabData.Header);
            Assert.AreEqual(2, tabData.Items.Count);
            Assert.AreEqual(10, tabData.Height);
            Assert.AreEqual(15, tabData.Width);
            Assert.AreEqual(20, tabData.ItemListWidth);

        }
    }
}
