using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{
    public class KinectProcessor
    {
        #region Constructor

        public KinectProcessor()
        {
            Sensor = KinectSensor.GetDefault();

			if (Sensor == null) return;

            Sensor.Open();

			Reader = Sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);

			CoordinateMapper = Sensor.CoordinateMapper;

            //IsAvailable2 = Sensor.IsAvailable;
		}

        #endregion

        #region Members

        /// <summary>
        ///     Reader that will interprate data comming from Kinect
        /// </summary>
        public MultiSourceFrameReader Reader;

        public bool IsAvailable2;

        /// <summary>
        ///      Used to fixed alignment issue between skeleton positional data and color image
        /// </summary>
        public CoordinateMapper CoordinateMapper;

        /// <summary>
        ///     Microsoft Kinect 2.0 sensor
        /// </summary>
        public KinectSensor Sensor;

        #endregion

        #region Event handlers

        public void StopKinect()
        {
            Reader?.Dispose();

            Sensor?.Close();
        }

        #endregion
    }
}