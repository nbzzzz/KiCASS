using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

		public SessionWorker(string ip, int sendPort, KinectProcessor dataPub)
		{
			_ip = ip;
			_sendPort = sendPort;
			_udpSender = new UDPSender(_ip, _sendPort);

			_dataPub = dataPub;
			_configFlags = new Dictionary<Microsoft.Kinect.JointType, bool>();
			_dataSub = new DataSubscriber(_configFlags, _dataPub, _udpSender);
		}

		public void SetConfigFlags(string[] address)
		{
			foreach (KeyValuePair<Microsoft.Kinect.JointType, bool> pair in _configFlags)
			{
				_configFlags[pair.Key] = false;
				if (address.FirstOrDefault(x => x == pair.Key.ToString()) != "")
				{
					_configFlags[pair.Key] = true;
				}
			}
		}

		public void CloseSession()
		{
			_udpSender.StopDataOut();
		}
	}
}
