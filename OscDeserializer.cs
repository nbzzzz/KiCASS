using System;
using Microsoft.Kinect;
using Rug.Osc;
using System.Linq;

namespace LaptopOrchestra.Kinect
{
	public static class OscDeserializer
	{
		public static string[] ParseOscPacket(OscPacket packet)
		{
			string[] msg = packet.ToString().Split(new string[] { ", " }, StringSplitOptions.None); ;

			return msg;
		}

		public static string[] GetMessageAddress(string[] msg)
		{
			string[] msgAddress = msg[0].Split(new char[] { '/' }).Skip(1).ToArray();

			return msgAddress;
		}

		public static string GetMessageIp(string[] msg)
		{
			string ip = msg[1].Replace("\"", ""); ;

			return ip;
		}

		public static int GetMessagePort(string[] msg)
		{
			int port = int.Parse(msg[2]);

			return port;
		}

		public static char[] GetMessageBinSeq(string[] msg)
		{
			char[] binSeq = msg[3].Replace("\"", "").ToCharArray();
			Array.Reverse(binSeq);

			return binSeq;
		}

        public static bool GetMessageHandStateFlag(string[] msg)
        {
            return msg[3] == "True";
        }
    }
}
