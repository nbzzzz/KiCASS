﻿using System;
using System.Collections.Generic;

namespace LaptopOrchestra.Kinect
{
	public class SessionManager
	{
		private List<SessionWorker> _openConnections;

		public List<SessionWorker> OpenConnections
		{
			get
			{
				CleanConnections();
				return _openConnections;
			}
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
				session.EndSession = true;
			}
			_openConnections.Clear();
		}

		private void CleanConnections()
		{
			for (int i = _openConnections.Count-1; i >= 0; i--)
			{
				if (_openConnections[i].EndSession)
				{
					_openConnections.RemoveAt(i);
				}
			}
		}
	}
}
