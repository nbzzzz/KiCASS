using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace LaptopOrchestra.Kinect
{
	public class SessionWorker
	{
		private DataSubscriber _dataSub;
		private KinectProcessor _dataPub;
		private UDPSender _udpSender;
		private int _sendPort;
		private string _ip;
		private Dictionary<Microsoft.Kinect.JointType, bool> _configFlags;
		private Dictionary<Microsoft.Kinect.JointType, bool> _lookupFlags;
		private System.Timers.Timer _configTimer;

		public int SendPort
		{
			get { return _sendPort; }
		}

		public string Ip
		{
			get { return _ip; }
		}

		public Dictionary<Microsoft.Kinect.JointType, bool> ConfigFlags
		{
			get { return _configFlags; }
			set { _configFlags = value; }
		}

		public Dictionary<Microsoft.Kinect.JointType, bool> LookupFlags
		{
			get { return _lookupFlags; }
			set { _lookupFlags = value; }
		}

		public SessionWorker(string ip, int sendPort, KinectProcessor dataPub)
		{
			_ip = ip;
			_sendPort = sendPort;
			_udpSender = new UDPSender(_ip, _sendPort);

			_dataPub = dataPub;
			_configFlags = new Dictionary<Microsoft.Kinect.JointType, bool>();
			_dataSub = new DataSubscriber(_configFlags, _dataPub, _udpSender);

			SetConfigTimer();
		}

		private void SetConfigTimer()
		{
			Thread.Sleep(500);
			_configTimer = new System.Timers.Timer(5000);
			_configTimer.Elapsed += _configTimer_Elapsed;
			_configTimer.Enabled = true;
		}

		private void _configTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ApplyLookupFlags();
        }

		public void SetLookupFlags(string[] address)
		{
			foreach (KeyValuePair<Microsoft.Kinect.JointType, bool> pair in _lookupFlags)
			{
				if (address.Any(x => x == pair.Key.ToString()))
				{
					_configFlags[pair.Key] = true;
				}
			}
		}

		private void ApplyLookupFlags()
		{
			foreach (KeyValuePair<Microsoft.Kinect.JointType, bool> pair in _lookupFlags)
			{
				_configFlags[pair.Key] = pair.Value;
			}
		}

		public void CloseSession()
		{
			_udpSender.StopDataOut();
		}
	}
}
