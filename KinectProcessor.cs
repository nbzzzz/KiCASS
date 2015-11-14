﻿using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{
    public class KinectProcessor
    {
        #region Constructor

        public KinectProcessor()
        {
            StartKinect();
        }

        #endregion

        #region Members

        /// <summary>
        ///     Reader that will interprate data comming from Kinect
        /// </summary>
        public MultiSourceFrameReader Reader;

        /// <summary>
        ///     Microsoft Kinect 2.0 sensor
        /// </summary>
        private KinectSensor _sensor;

        #endregion

        #region Event handlers

        private void StartKinect()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor == null) return;

            _sensor.Open();

            Reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
        }

        private void StopKinect()
        {
            Reader?.Dispose();

            _sensor?.Close();
        }

        #endregion
    }
}