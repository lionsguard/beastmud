using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Beast
{
    /// <summary>
    /// Provides static methods for logging.
    /// </summary>
	public static class Log
	{
		private static readonly List<ILogContext> LoggingContexts = new List<ILogContext>();

		public static void Initialize(params ILogContext[] loggingContexts)
		{
			LoggingContexts.Clear();
			LoggingContexts.Add(new TraceLogContext());

            Load(loggingContexts);
		}

		public static void Load(IEnumerable<ILogContext> loggingContexts)
		{
			if (loggingContexts == null)
				return;
			LoggingContexts.AddRange(loggingContexts);
		}

		public static void Debug(string message)
		{
			Debug(message, null);
		}
		public static void Debug(string format, params object[] args)
		{
			Write(LogType.Debug, format, args);
		}

		public static void Info(string message)
		{
			Info(message, null);
		}
		public static void Info(string format, params object[] args)
		{
			Write(LogType.Info, format, args);
		}

		public static void Warn(string message)
		{
			Warn(message, null);
		}
		public static void Warn(string format, params object[] args)
		{
			Write(LogType.Warn, format, args);
		}

		public static void Error(string message)
		{
			Error(message, null);
		}
		public static void Error(string format, params object[] args)
		{
			Write(LogType.Error, format, args);
		}
		public static void Error(Exception ex)
		{
			Write(LogType.Error, ex.ToString(), null);
		}

		public static void Input(string message)
		{
			Input(message, null);
		}
		public static void Input(string format, params object[] args)
		{
			Write(LogType.Input, format, args);
		}

		public static void Message(string message)
		{
			Message(message, null);
		}
		public static void Message(string format, params object[] args)
		{
			Write(LogType.Output, format, args);
		}

		private static void Write(LogType type, string format, params object[] args)
		{
			try
			{
				var msg = format;
				if (args != null && args.Length > 0)
					msg = string.Format(format, args);

				foreach (var context in LoggingContexts)
				{
					context.Write(type, msg);
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		public class TraceLogContext : ILogContext
		{
			public void Write(LogType type, string message)
			{
				Trace.WriteLine(string.Format("{0} [ {1} ] - {2}", DateTime.UtcNow, type.ToString().ToUpper(), message));
			}
		}
	}
}