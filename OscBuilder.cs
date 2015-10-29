using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Rug.Osc;

namespace LaptopOrchestra.Kinect
{
    class OscBuilder
    {
        public OscMessage BuildJointMessage(Joint joint)
        {	
			var address = String.Format("/kinect/joint/{0}", joint.JointType);
			var pos = joint.Position;
			return new OscMessage(address, pos.X, pos.Y, pos.Z);
        }

        public OscMessage BuildVectorMessage(Joint joint1, Joint joint2)
        {	
			var address = String.Format("/kinect/vector/{0}/{1}", joint1.JointType, joint2.JointType);
			var xDist = joint2.Position.X - joint1.Position.X;
			var yDist = joint2.Position.Y - joint1.Position.Y;
			var zDist = joint2.Position.Z - joint1.Position.Z;
			return new OscMessage(address, xDist , yDist , zDist );
        }

        public OscMessage BuildDistanceMessage(Joint joint1, Joint joint2)
        {	
			var address = String.Format("/kinect/distance/{0}/{1}", joint1.JointType, joint2.JointType);
			var xDist = joint2.Position.X - joint1.Position.X;
			var yDist = joint2.Position.Y - joint1.Position.Y;
			var zDist = joint2.Position.Z - joint1.Position.Z;
			var absDist = Math.Sqrt(xDist * xDist + yDist * yDist* + zDist * zDist);
			return new OscMessage(address, absDist);
        }
    }
}
