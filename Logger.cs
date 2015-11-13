using log4net;

public static class Logger
{
	private static readonly ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	public static void Debug(string message)
	{
		_log.Debug(message);
	}

	public static void Info(string message)
	{
		_log.Info(message);
	}

	public static void Warn(string message)
	{
		_log.Warn(message);
	}

	public static void Error(string message)
	{
		_log.Error(message);
	}

	public static void Fatal(string message)
	{
		_log.Fatal(message);
	}
}
