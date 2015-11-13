using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaptopOrchestra.Kinect
{
    public class KinectSimulatorData
    {
        public int TrackingId { get; set; }
        public float HipCenterX { get; set; }
        public float HipCenterY { get; set; }
        public float HipCenterZ { get; set; }
        public float SpineX { get; set; }
        public float SpineY { get; set; }
        public float SpineZ { get; set; }
        public float ShoulderCenterX { get; set; }
        public float ShoulderCenterY { get; set; }
        public float ShoulderCenterZ { get; set; }
        public float HeadX { get; set; }
        public float HeadY { get; set; }
        public float HeadZ { get; set; }
        public float ShoulderLeftX { get; set; }
        public float ShoulderLeftY { get; set; }
        public float ShoulderLeftZ { get; set; }
        public float ElbowLeftX { get; set; }
        public float ElbowLeftY { get; set; }
        public float ElbowLeftZ { get; set; }
        public float WristLeftX { get; set; }
        public float WristLeftY { get; set; }
        public float WristLeftZ { get; set; }
        public float HandLeftX { get; set; }
        public float HandLeftY { get; set; }
        public float HandLeftZ { get; set; }
        public float ShoulderRightX { get; set; }
        public float ShoulderRightY { get; set; }
        public float ShoulderRightZ { get; set; }
        public float ElbowRightX { get; set; }
        public float ElbowRightY { get; set; }
        public float ElbowRightZ { get; set; }
        public float WristRightX { get; set; }
        public float WristRightY { get; set; }
        public float WristRightZ { get; set; }
        public float HandRightX { get; set; }
        public float HandRightY { get; set; }
        public float HandRightZ { get; set; }
        public float HipLeftX { get; set; }
        public float HipLeftY { get; set; }
        public float HipLeftZ { get; set; }
        public float KneeLeftX { get; set; }
        public float KneeLeftY { get; set; }
        public float KneeLeftZ { get; set; }
        public float AnkleLeftX { get; set; }
        public float AnkleLeftY { get; set; }
        public float AnkleLeftZ { get; set; }
        public float FootLeftX { get; set; }
        public float FootLeftY { get; set; }
        public float FootLeftZ { get; set; }
        public float HipRightX { get; set; }
        public float HipRightY { get; set; }
        public float HipRightZ { get; set; }
        public float KneeRightX { get; set; }
        public float KneeRightY { get; set; }
        public float KneeRightZ { get; set; }
        public float AnkleRightX { get; set; }
        public float AnkleRightY { get; set; }
        public float AnkleRightZ { get; set; }
        public float FootRightX { get; set; }
        public float FootRightY { get; set; }
        public float FootRightZ { get; set; }


        public KinectSimulatorData(string[] data)
        {
            TrackingId = Convert.ToInt32(data[0]);
            HipCenterX = Convert.ToSingle(data[1]);
            HipCenterY = Convert.ToSingle(data[2]);
            HipCenterZ = Convert.ToSingle(data[3]);
            SpineX = Convert.ToSingle(data[5]);
            SpineY = Convert.ToSingle(data[6]);
            SpineZ = Convert.ToSingle(data[7]);
            ShoulderCenterX = Convert.ToSingle(data[9]);
            ShoulderCenterY = Convert.ToSingle(data[10]);
            ShoulderCenterZ = Convert.ToSingle(data[11]);
            HeadX = Convert.ToSingle(data[13]);
            HeadY = Convert.ToSingle(data[14]);
            HeadZ = Convert.ToSingle(data[15]);
            ShoulderLeftX = Convert.ToSingle(data[17]);
            ShoulderLeftY = Convert.ToSingle(data[18]);
            ShoulderLeftZ = Convert.ToSingle(data[19]);
            ElbowLeftX = Convert.ToSingle(data[21]);
            ElbowLeftY = Convert.ToSingle(data[22]);
            ElbowLeftZ = Convert.ToSingle(data[23]);
            WristLeftX = Convert.ToSingle(data[25]);
            WristLeftY = Convert.ToSingle(data[26]);
            WristLeftZ = Convert.ToSingle(data[27]);
            HandLeftX = Convert.ToSingle(data[29]);
            HandLeftY = Convert.ToSingle(data[30]);
            HandLeftZ = Convert.ToSingle(data[31]);
            ShoulderRightX = Convert.ToSingle(data[33]);
            ShoulderRightY = Convert.ToSingle(data[34]);
            ShoulderRightZ = Convert.ToSingle(data[35]);
            ElbowRightX = Convert.ToSingle(data[37]);
            ElbowRightY = Convert.ToSingle(data[38]);
            ElbowRightZ = Convert.ToSingle(data[39]);
            WristRightX = Convert.ToSingle(data[41]);
            WristRightY = Convert.ToSingle(data[42]);
            WristRightZ = Convert.ToSingle(data[43]);
            HandRightX = Convert.ToSingle(data[45]);
            HandRightY = Convert.ToSingle(data[46]);
            HandRightZ = Convert.ToSingle(data[47]);
            HipLeftX = Convert.ToSingle(data[49]);
            HipLeftY = Convert.ToSingle(data[50]);
            HipLeftZ = Convert.ToSingle(data[51]);
            KneeLeftX = Convert.ToSingle(data[53]);
            KneeLeftY = Convert.ToSingle(data[54]);
            KneeLeftZ = Convert.ToSingle(data[55]);
            AnkleLeftX = Convert.ToSingle(data[57]);
            AnkleLeftY = Convert.ToSingle(data[58]);
            AnkleLeftZ = Convert.ToSingle(data[59]);
            FootLeftX = Convert.ToSingle(data[61]);
            FootLeftY = Convert.ToSingle(data[62]);
            FootLeftZ = Convert.ToSingle(data[63]);
            HipRightX = Convert.ToSingle(data[65]);
            HipRightY = Convert.ToSingle(data[66]);
            HipRightZ = Convert.ToSingle(data[67]);
            KneeRightX = Convert.ToSingle(data[69]);
            KneeRightY = Convert.ToSingle(data[70]);
            KneeRightZ = Convert.ToSingle(data[71]);
            AnkleRightX = Convert.ToSingle(data[73]);
            AnkleRightY = Convert.ToSingle(data[74]);
            AnkleRightZ = Convert.ToSingle(data[75]);
            FootRightX = Convert.ToSingle(data[77]);
            FootRightY = Convert.ToSingle(data[78]);
            FootRightZ = Convert.ToSingle(data[79]);
        }

        public IReadOnlyDictionary<JointType, Joint> toKinectData()
        {
            // Initalize our skeleton
            var joints = new Dictionary<JointType, Joint>();

            //var JointTypes = Enum.GetValues(typeof(JointType));
            var JointTypes = Enum.GetValues(typeof(JointType)).Cast<JointType>(); ;

            foreach (var jt in JointTypes)
            {
                Joint joint = new Joint();

                //TODO SpineMid Neck SpineShoulderX HandTipLeft HandTipRight ThumbRight ThumbLeft
                switch (jt) {
                    case (JointType.SpineBase):
                        joint = toKinectData(JointType.SpineBase, SpineX, SpineY, SpineZ, TrackingState.Tracked);
                        break;
                    case (JointType.SpineMid):
                        joint = toKinectData(JointType.SpineMid, SpineX, SpineY, SpineZ, TrackingState.Tracked);
                        break;
                    case (JointType.Neck):
                        joint = toKinectData(JointType.Neck, HeadX, HeadY, HeadZ, TrackingState.Tracked);
                        break;
                    case (JointType.Head):
                        joint = toKinectData(JointType.Head, HeadX, HeadY, HeadZ, TrackingState.Tracked);
                        break;
                    case (JointType.ShoulderLeft):
                        joint = toKinectData(JointType.ShoulderLeft, ShoulderLeftX, ShoulderLeftY, ShoulderLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.ElbowLeft):
                        joint = toKinectData(JointType.ElbowLeft, ElbowLeftX, ElbowLeftY, ElbowLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.WristLeft):
                        joint = toKinectData(JointType.WristLeft, WristLeftX, WristLeftY, WristLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.HandLeft):
                        joint = toKinectData(JointType.HandLeft, HandLeftX, HandLeftY, HandLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.ShoulderRight):
                        joint = toKinectData(JointType.ShoulderRight, ShoulderRightX, ShoulderRightY, ShoulderRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.ElbowRight):
                        joint = toKinectData(JointType.ElbowRight, ElbowRightX, ElbowRightY, ElbowRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.WristRight):
                        joint = toKinectData(JointType.WristRight, WristRightX, WristRightY, WristRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.HandRight):
                        joint = toKinectData(JointType.HandRight, HandRightX, HandRightY, HandRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.HipLeft):
                        joint = toKinectData(JointType.HipLeft, HipLeftX, HipLeftY, HipLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.KneeLeft):
                        joint = toKinectData(JointType.KneeLeft, KneeLeftX, KneeLeftY, KneeLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.AnkleLeft):
                        joint = toKinectData(JointType.AnkleLeft, AnkleLeftX, AnkleLeftY, AnkleLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.FootLeft):
                        joint = toKinectData(JointType.FootLeft, FootLeftX, FootLeftY, FootLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.HipRight):
                        joint = toKinectData(JointType.HipRight, HipRightX, HipRightY, HipRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.KneeRight):
                        joint = toKinectData(JointType.KneeRight, KneeRightX, KneeRightY, KneeRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.AnkleRight):
                        joint = toKinectData(JointType.AnkleRight, AnkleRightX, AnkleRightY, AnkleRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.FootRight):
                        joint = toKinectData(JointType.FootRight, FootRightX, FootRightY, FootRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.SpineShoulder):
                        joint = toKinectData(JointType.SpineShoulder, SpineX, SpineY, SpineZ, TrackingState.Tracked);
                        break;
                    case (JointType.HandTipLeft):
                        joint = toKinectData(JointType.HandTipLeft, HandLeftX, HandLeftY, HandLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.ThumbLeft):
                        joint = toKinectData(JointType.ThumbLeft, HandLeftX, HandLeftY, HandLeftZ, TrackingState.Tracked);
                        break;
                    case (JointType.HandTipRight):
                        joint = toKinectData(JointType.HandTipRight, HandRightX, HandRightY, HandRightZ, TrackingState.Tracked);
                        break;
                    case (JointType.ThumbRight):
                        joint = toKinectData(JointType.ThumbRight, HandRightX, HandRightY, HandRightZ, TrackingState.Tracked);
                        break;
                }

                joints.Add(jt, joint);
            }

            return (IReadOnlyDictionary<JointType, Joint>)joints;
        }

        private Joint toKinectData(JointType jt, float x, float y, float z, TrackingState ts) {
            Joint joint = new Joint();
            CameraSpacePoint position = new CameraSpacePoint();

            position.X = x;
            position.Y = y;
            position.Z = z;

            joint.JointType = jt;
            joint.TrackingState = TrackingState.Tracked;
            joint.Position = position;

            return joint;
        }

    }
}
