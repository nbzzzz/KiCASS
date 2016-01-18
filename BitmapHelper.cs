using System;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaptopOrchestra.Kinect
{
    public static class BitmapHelper
    {
        #region Camera

        public static ImageSource ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;

            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        #endregion

        #region Drawing

        public static void DrawSkeleton(this Canvas canvas, Body body, Dictionary<JointType, Point> jointPoints, bool isFirst)
        {
            if (body == null) return;

            // a bone defined as a line between two joints
            var bones = new List<Tuple<JointType, JointType>>();

            // Torso
            bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));
                
            canvas.DrawBones(body, jointPoints, bones, isFirst);
            canvas.DrawJoints(body, jointPoints, isFirst);
            canvas.DrawHand(jointPoints[JointType.HandLeft], body.HandLeftState, isFirst);
            canvas.DrawHand(jointPoints[JointType.HandRight], body.HandRightState, isFirst);
        }

        private static void DrawBones(this Canvas canvas, Body body, Dictionary<JointType, Point> jointPoints, List<Tuple<JointType, JointType>> bones, bool isFirst)
        {
            foreach (var bone in bones)
            {
                var jointType0 = bone.Item1;
                var jointType1 = bone.Item2;
                Joint joint0 = body.Joints[jointType0];
                Joint joint1 = body.Joints[jointType1];
                Point point0 = jointPoints[jointType0];
                Point point1 = jointPoints[jointType1];

                // If we can't find either of these joints, exit
                if (joint0.TrackingState == TrackingState.NotTracked ||
                    joint1.TrackingState == TrackingState.NotTracked)
                {
                    return;
                }

                // If mapped points are not valid then do not draw
                if (point0.X.Equals(double.NegativeInfinity) || point0.Y.Equals(double.NegativeInfinity) ||
                    point1.X.Equals(double.NegativeInfinity) || point1.Y.Equals(double.NegativeInfinity) )
                {
                    return;
                }

                // We assume all drawn bones are inferred unless BOTH joints are tracked
                Color drawColor = Constants.InferredColor;

                if (isFirst && (joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
                {
                    //TODO change this depending on body number; pass in the pen
                    drawColor = Constants.TrackedColor;
                }
				else if (!isFirst)
				{
					drawColor = Constants.UntrackedBodyColor;
				}

                // Draw a line to represent the bones
                canvas.DrawLine(jointPoints[jointType0], jointPoints[jointType1], drawColor);
            }
        }

        private static void DrawJoints(this Canvas canvas, Body body, Dictionary<JointType, Point> jointPoints, bool isFirst)
		{
            foreach (var joint in jointPoints.Keys)
            {
                if (jointPoints[joint].X.Equals(double.NegativeInfinity) ||
                    jointPoints[joint].Y.Equals(double.NegativeInfinity)) return;

                var isTracked = body.Joints[joint].TrackingState == TrackingState.Tracked;
                var point = jointPoints[joint];

				if (isFirst)
				{
					canvas.DrawPoint(point, isTracked ? Constants.TrackedColor : Constants.InferredColor);
				}
                else if (!isFirst)
				{
					canvas.DrawPoint(point, Constants.UntrackedBodyColor);
				}
            }

        }

        private static void DrawHand(this Canvas canvas, Point handPosition, HandState handState, bool isFirst)
        {
            double handSize = 90;

			if (isFirst)
			{
				switch (handState)
				{
					case HandState.Closed:
						canvas.DrawPoint(handPosition, Constants.HandClosedBrush, handSize);
						break;

					case HandState.Open:
						canvas.DrawPoint(handPosition, Constants.HandOpenBrush, handSize);
						break;

					case HandState.Lasso:
						canvas.DrawPoint(handPosition, Constants.HandLassoBrush, handSize);
						break;
				}
			}
			else if (!isFirst)
			{
				canvas.DrawPoint(handPosition, Constants.UntrackedBodyColor, handSize);
			}
        }

        #endregion

        #region Geometry Drawing

        public static void DrawPoint(this Canvas canvas, Point point, Color color, double radius)
        {
            if (Double.IsInfinity(point.X) || Double.IsInfinity(point.Y)) return;

            Ellipse ellipse = new Ellipse
            {
                Width = 2*radius,
                Height = 2*radius,
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Constants.PointBorderColor),
                StrokeThickness = 3
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawPoint(this Canvas canvas, Point point, Color color)
        {
            canvas.DrawPoint(point, color, 10);
        }


        public static void DrawLine(this Canvas canvas, Point firstPoint, Point secondPoint, Color color)
        {
            Line line = new Line
            {
                X1 = firstPoint.X,
                Y1 = firstPoint.Y,
                X2 = secondPoint.X,
                Y2 = secondPoint.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(color)
            };

            canvas.Children.Add(line);
        }

        #endregion
    }
}
