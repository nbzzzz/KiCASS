using System.Linq;
using LaptopOrchestra.Kinect;
using Microsoft.Kinect;
using Microsoft.Kinect.Input;
using NUnit.Framework;

namespace LaptopOrchestraTest
{
    [TestFixture]
    class ConfigFlagsTest
    {
        [Test]
        public void AnyConfigFlagTest()
        {
            ConfigFlags configFlags = new ConfigFlags();
            Assert.False(configFlags.JointFlags.Any(pair => pair.Value));
            Assert.False(configFlags.IfAnyConfig());

            configFlags.JointFlags[JointType.AnkleLeft] = true;
            Assert.True(configFlags.IfAnyConfig());
            Assert.True(configFlags.JointFlags.Any(pair => pair.Value));

            configFlags.JointFlags[JointType.AnkleLeft] = false;
            Assert.False(configFlags.IfAnyConfig());
            Assert.False(configFlags.JointFlags.Any(pair => pair.Value));

            configFlags.HandStateFlag[HandType.LEFT] = true;
            Assert.True(configFlags.IfAnyConfig());
            Assert.True(configFlags.HandStateFlag.Any(pair => pair.Value));

            configFlags.HandStateFlag[HandType.LEFT] = false;
            Assert.False(configFlags.IfAnyConfig());
            Assert.False(configFlags.HandStateFlag.Any(pair => pair.Value));
        }

        [Test]
        public void JointFlagsTest()
        {
            ConfigFlags configFlags = new ConfigFlags();
            Assert.False(configFlags.JointFlags.Any(pair => pair.Value));

            configFlags.JointFlags[JointType.HandRight] = true;
            Assert.True(configFlags.JointFlags.Any(pair => pair.Value));

            configFlags.JointFlags[JointType.HandRight] = false;
            Assert.False(configFlags.JointFlags.Any(pair => pair.Value));
        }

        [Test]
        public void HandFlagsTest()
        {
            ConfigFlags configFlags = new ConfigFlags();
            Assert.False(configFlags.HandStateFlag.Any(pair => pair.Value));

            configFlags.HandStateFlag[HandType.LEFT] = true;
            Assert.True(configFlags.HandStateFlag.Any(pair => pair.Value));

            configFlags.HandStateFlag[HandType.LEFT] = false;
            Assert.False(configFlags.HandStateFlag.Any(pair => pair.Value));
        }

    }
}
