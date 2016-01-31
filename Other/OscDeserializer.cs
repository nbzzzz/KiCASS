using System;
using Microsoft.Kinect;
using Rug.Osc;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static char[] GetMessageHandStateFlag(string[] msg)
        {
            char[] handStateFlags = msg[3].Replace("\"", "").ToCharArray();
            Array.Reverse(handStateFlags);

            return handStateFlags;
        }

	    public static bool IsValid(OscPacket packet)
	    {
	        string packetString = packet.ToString();
            string pattern = @"\/kinect\/(joint|handstate), ""\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"", ([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5]), ""([10]{2}|[10]{25})""";

            // Instantiate the regular expression object.
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(packetString);
            
            if (!m.Success)
	        {
	            Logger.Error("Invalid Osc Message recieved " + packetString);
	            return false;
	        }
	        return true;
	    }
	}
}
