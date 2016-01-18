using System;
using Microsoft.Kinect;
using Rug.Osc;

namespace LaptopOrchestra.Kinect
{
    public static class OscSerializer
    {
        public static OscMessage BuildJointMessage(Joint joint)
        {	
			var address = String.Format(Constants.OscJointAddr, joint.JointType);
			var pos = joint.Position;
			return new OscMessage(address, pos.X, pos.Y, pos.Z);
        }

        public static OscMessage BuildVectorMessage(Joint joint1, Joint joint2)
        {	
			var address = String.Format(Constants.OscVectorAddr, joint1.JointType, joint2.JointType);
			var xDist = joint2.Position.X - joint1.Position.X;
			var yDist = joint2.Position.Y - joint1.Position.Y;
			var zDist = joint2.Position.Z - joint1.Position.Z;
			return new OscMessage(address, xDist , yDist , zDist );
        }

        public static OscMessage BuildDistanceMessage(Joint joint1, Joint joint2)
        {	
			var address = String.Format(Constants.OscDistanceAddr, joint1.JointType, joint2.JointType);
			var xDist = joint2.Position.X - joint1.Position.X;
			var yDist = joint2.Position.Y - joint1.Position.Y;
			var zDist = joint2.Position.Z - joint1.Position.Z;
			var absDist = Math.Sqrt(xDist * xDist + yDist * yDist* + zDist * zDist);
			return new OscMessage(address, absDist);
        }

		public static OscMessage BuildHandStateMessage(HandState leftHand, HandState rightHand)
		{
			var address = Constants.OscHandStateAddr;

			return new OscMessage(address, (int)leftHand, (int)rightHand);
		}
	}
}
