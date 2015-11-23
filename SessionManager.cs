using System;
using System.Collections.Generic;

namespace LaptopOrchestra.Kinect
{
	public class SessionManager
	{
		private List<SessionWorker> _openConnections;

		public List<SessionWorker> OpenConnections
		{
			get { return _openConnections; }
		}

		public SessionManager()
		{
			_openConnections = new List<SessionWorker>();
		}

		public void AddConnection(SessionWorker session)
		{
			_openConnections.Add(session);
		}

		public void CloseAllConnections()
		{
			foreach (var session in _openConnections)
			{
				session.CloseSession();
			}
			_openConnections.Clear();
		}

		public void RemoveConnection(SessionWorker session)
		{
			_openConnections.Remove(session);
		}
	}
}
