using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LaptopOrchestra.Kinect
{

    public class KinectProcessor
    {
        #region Members

        /// <summary>
        /// Reader that will interprate data comming from Kinect
        /// </summary>
        public MultiSourceFrameReader Reader;

        /// <summary>
        /// Microsoft Kinect 2.0 sensor 
        /// </summary>
        private KinectSensor _sensor;

        #endregion

        #region Constructor

        public KinectProcessor()
        {
            StartKinect();
        }

        #endregion

        #region Event handlers

        private void StartKinect()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                Reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            }
        }

        private void StopKinect()
        {
            Reader?.Dispose();

            _sensor?.Close();
        }

        #endregion
    }

}
