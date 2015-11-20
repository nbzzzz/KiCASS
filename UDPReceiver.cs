using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rug.Osc;

namespace LaptopOrchestra.Kinect
{
	public class UDPReceiver
	{
		private OscReceiver _receiver;
		private int _port;
		private Thread _listenerThread;
		private SessionManager _sessionManager;
		private KinectProcessor _dataPub;

		public UDPReceiver(int port, SessionManager sessionManager, KinectProcessor dataPub)
		{
			_port = port;
			_sessionManager = sessionManager;
			_dataPub = dataPub;
			_receiver = new OscReceiver(_port);
			_listenerThread = new Thread(new ThreadStart(ListenerWork));
			_receiver.Connect();
			_listenerThread.Start();
		}

		private void ListenerWork()
		{
			while (_receiver.State != OscSocketState.Closed)
			{
				// if we are in a state to recieve
				if (_receiver.State == OscSocketState.Connected)
				{
					// get the next message 
					// this will block until one arrives or the socket is closed
					OscPacket packet = _receiver.Receive();

					// parse the message
					string[] msg = packet.ToString().Split(new string[] { ", " }, StringSplitOptions.None);
					//skip the first index of the parsed address as it is an empty string (remove if refactored to not start osc message with leading /)
					string[] msgAddress = msg[0].Split(new char[] { '/' }).Skip(1).ToArray();

					var ip = msg[1].Replace("\"", "");
					var port = int.Parse(msg[2]);
						
					// if the sessionWorker already exists, update config. Otherwise, create a new sessionWorker
					SessionWorker session = _sessionManager.OpenConnections.FirstOrDefault(x => x.Ip == ip && x.Port == port);

					if (session == null)
					{
						session = new SessionWorker(ip, port, _dataPub, _sessionManager);
						_sessionManager.AddConnection(session);
						session.SetConfigTimer(msgAddress);
					}
					else
					{
						session.SetLookupFlags(msgAddress);
					}
				}
			}
		}

		public void Close()
		{
			_receiver.Close();

			_listenerThread.Join();
		}
	}
}
