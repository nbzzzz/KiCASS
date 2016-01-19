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
		private ConfigFlags _flagIterator;
		private System.Timers.Timer _configTimer;
		private bool _endSession;
	    private int sessionRetries;

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
		    sessionRetries++;

            if (sessionRetries > Constants.MaxSessionRetries)
            {
                Logger.Info("Have not recieved messages from " + Ip + ":" + Port + " for " + Constants.MaxSessionRetries*Constants.SessionRecvConfigInterval + " terminating session");
                EndSession = true;
            }
		}

		public void SetJointFlags(char[] address)
		{
            Logger.Debug("Setting joint flags to " + address + "for " + Ip + ":" + "Port");
            foreach (var key in _flagIterator.JointFlags.Keys)
			{
				if (address[(int)key] == Constants.CharTrue)
				{
                    _configFlags.JointFlags[key] = true;
				}
				else
				{
                    _configFlags.JointFlags[key] = false;
                }
			}
		    sessionRetries = 0;
		}


        public void SetHandFlag(bool handStateFlag)
        {
            Logger.Debug("Setting hand flag to " + handStateFlag + "for " + Ip + ":" + "Port");
            _configFlags.HandStateFlag = handStateFlag;
            		    sessionRetries = 0;
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
