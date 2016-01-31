using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{
    public class KinectProcessor
    {
        #region Constructor

        public KinectProcessor()
        {
			_sensor = KinectSensor.GetDefault();

			if (_sensor == null) return;

			_sensor.Open();

			Reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
			CoordinateMapper = _sensor.CoordinateMapper;
		}

        #endregion

        #region Members

        /// <summary>
        ///     Reader that will interprate data comming from Kinect
        /// </summary>
        public MultiSourceFrameReader Reader;

        /// <summary>
        ///      Used to fixed alignment issue between skeleton positional data and color image
        /// </summary>
        public CoordinateMapper CoordinateMapper;

        /// <summary>
        ///     Microsoft Kinect 2.0 sensor
        /// </summary>
        private KinectSensor _sensor;

        #endregion

        #region Event handlers

        public void StopKinect()
        {
            Reader?.Dispose();

            _sensor?.Close();
        }

        #endregion
    }
}