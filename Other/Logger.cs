using System.Reflection;
using log4net;

namespace LaptopOrchestra.Kinect
{
    public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(string message)
        {
            Log.Debug(message);
        }

        public static void Info(string message)
        {
            Log.Info(message);
        }

        public static void Warn(string message)
        {
            Log.Warn(message);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }

        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }
    }
}