using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Kinect;

namespace LaptopOrchestra.Kinect
{
	public class SessionWorker
	{
		private DataSubscriber _dataSub;
		private KinectProcessor _dataPub;
		private SessionManager _sessionManager;
		private UDPSender _udpSender;
		private int _port;
		private string _ip;
		private Dictionary<JointType, bool> _configFlags;
		private Dictionary<JointType, bool> _lookupFlags;
		private Dictionary<JointType, bool> _flagIterator;
		private System.Timers.Timer _configTimer;
		private int waitLookupTime = 500;
		private int totalConfigInterval = 5000;

		public int Port
		{
			get { return _port; }
		}

		public string Ip
		{
			get { return _ip; }
		}

		public Dictionary<JointType, bool> ConfigFlags
		{
			get { return _configFlags; }
		}

		public Dictionary<JointType, bool> LookupFlags
		{
			get { return _lookupFlags; }
		}

		public SessionWorker(string ip, int sendPort, KinectProcessor dataPub, SessionManager sessionManager)
		{
			_ip = ip;
			_port = sendPort;
			_udpSender = new UDPSender(_ip, _port);

			_sessionManager = sessionManager;
			_dataPub = dataPub;
			_configFlags = InitFlags(_configFlags);
			_lookupFlags = InitFlags(_lookupFlags);
			_flagIterator = InitFlags(_flagIterator);
			_dataSub = new DataSubscriber(_configFlags, _dataPub, _udpSender);
		}

		public void SetConfigTimer(string[] address)
		{
			SetLookupFlags(address);
			Thread.Sleep(waitLookupTime);
			ApplyLookupFlags();
			ClearLookupFlags();
			_configTimer = new System.Timers.Timer(totalConfigInterval);
			_configTimer.Elapsed += _configTimer_Elapsed;
			_configTimer.Enabled = true;
		}

		private void _configTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (CheckLookupFlags())
			{
				ApplyLookupFlags();
				ClearLookupFlags();
			}
		}

		private bool CheckLookupFlags()
		{
			if (!_lookupFlags.Any(x => x.Value == true))
			{
				CloseSession();
				return false;
			}
			return true;
		}

		public void SetLookupFlags(string[] address)
		{
			foreach (var key in _flagIterator.Keys)
			{
				if (address.Any(x => x == key.ToString()))
				{
					_lookupFlags[key] = true;
				}
			}
		}

		private void ClearLookupFlags()
		{
			foreach (var key in _flagIterator.Keys)
			{
				_lookupFlags[key] = false;
			}
		}

		private Dictionary<JointType, bool> InitFlags(Dictionary<JointType, bool> flags)
		{
			var jointTypes = Enum.GetValues(typeof(JointType));

			flags = new Dictionary<JointType, bool>();

			foreach (JointType jt in jointTypes)
			{
				flags[jt] = false;
			}

			return flags;
		}

		private void ApplyLookupFlags()
		{
			foreach (KeyValuePair<JointType, bool> pair in _lookupFlags)
			{
				_configFlags[pair.Key] = pair.Value;
			}
		}

		public void CloseSession()
		{
			_configTimer.Stop();
			_configTimer.Close();
			_udpSender.StopDataOut();
			_sessionManager.RemoveConnection(this);
		}
	}
}
