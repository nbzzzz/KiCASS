using System;
using System.Net;
using Rug.Osc;

public static class UDP
{
    /// <summary>
    /// Port data will be streamed to
    /// </summary>
    private static int _connectPort;

    /// <summary>
    /// IP Address data will be streamed to
    /// </summary>
    private static IPAddress _userIP;

    /// <summary>
    /// Object that will send OSC Packets through UDP
    /// </summary>
    private static OscSender _sender;

	public static void StartDataOut()
	{
		try
		{
			_sender = new OscSender(_userIP, _connectPort);
			_sender.Connect();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
            Logger.Debug(e.Message);
		}
	}

	public static void StopDataOut()
	{
		_sender.Close();
	}

	public static void ConfigureIpAndPort(string ip, int port)
	{
		if(_sender != null)
		{
			StopDataOut();
		}
        try {
            _userIP = IPAddress.Parse(ip);
            _connectPort = port;
        }
        catch
        {
            Console.WriteLine("INVALID IP or PORT");
        }

		StartDataOut();
	}

	public static void SendMessage(OscMessage message)
	{
		_sender.Send(message);
	}
}
