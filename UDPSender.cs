using System;
using System.Net;
using LaptopOrchestra.Kinect;
using Rug.Osc;

public class UDPSender
{
    /// <summary>
    ///     Port data will be streamed to
    /// </summary>
    private int _connectPort;

    /// <summary>
    ///     IP Address data will be streamed to
    /// </summary>
    private IPAddress _userIP;

    /// <summary>
    ///     Object that will send OSC Packets through UDP
    /// </summary>
    private OscSender _sender;

	public UDPSender(string ip, int port)
	{
		_userIP = IPAddress.Parse(ip);
		_connectPort = port;
	}

    public void StartDataOut()
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

    public void StopDataOut()
    {
        _sender.Close();
    }

    public void SendMessage(OscMessage message)
    {
        _sender.Send(message);
    }
}