using System;
using System.Collections.Generic;
using System.Linq;
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
		private ConfigFlags _configFlags;
		private ConfigFlags _lookupFlags;
		private ConfigFlags _flagIterator;
		private System.Timers.Timer _configTimer;
		private bool _endSession;

		public int Port
		{
			get { return _port; }
		}

		public string Ip
		{
			get { return _ip; }
		}

		public ConfigFlags ConfigFlags
		{
			get { return _configFlags; }
		}

		public ConfigFlags LookupFlags
		{
			get { return _lookupFlags; }
		}

		public bool EndSession
		{
			get { return _endSession; }
			set
			{
				if (value == true)
				{
					CloseSession();
					_endSession = value;
				}
				else
				{
					_endSession = value;
				}
			}
		}

		public SessionWorker(string ip, int sendPort, KinectProcessor dataPub, SessionManager sessionManager)
		{
			_ip = ip;
			_port = sendPort;
			_udpSender = new UDPSender(_ip, _port);
			_endSession = false;

			_sessionManager = sessionManager;
			_dataPub = dataPub;
			_configFlags = new ConfigFlags();
			_lookupFlags = new ConfigFlags();
			_flagIterator = new ConfigFlags();
			_dataSub = new DataSubscriber(_configFlags, _dataPub, _udpSender);
		}

		public void SetTimers()
		{
			_configTimer = new System.Timers.Timer(Constants.SessionRecvConfigInterval);
			_configTimer.Elapsed += _configTimer_Elapsed;
			_configTimer.Enabled = true;
		}

		private void _configTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (CheckLookupFlags())
			{
				ApplyLookupFlags();
			}
			ClearLookupFlags();
		}

		private bool CheckLookupFlags()
		{
			if (!_lookupFlags.JointFlags.Any(x => x.Value == true))
			{
				CloseSession();
				return false;
			}
			return true;
		}

		public void SetLookupFlags(char[] address)
		{
			foreach (var key in _flagIterator.JointFlags.Keys)
			{
				if (address[(int)key] == Constants.CharTrue)
				{
					_lookupFlags.JointFlags[key] = true;
				}
			}
			ApplyLookupFlags();
		}

		private void ClearLookupFlags()
		{
			foreach (var key in _flagIterator.JointFlags.Keys)
			{
				_lookupFlags.JointFlags[key] = false;
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
			foreach (KeyValuePair<JointType, bool> pair in _lookupFlags.JointFlags)
			{
				_configFlags.JointFlags[pair.Key] = pair.Value;
			}
		}

		private void CloseSession()
		{
			_configTimer.Stop();
			_configTimer.Close();
			_udpSender.StopDataOut();
			GC.Collect();
		}
	}
}
