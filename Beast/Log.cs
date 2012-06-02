using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;

namespace Beast
{
	public static class Log
	{
		[ImportMany(typeof(ILogger), AllowRecomposition = true)]
		public static IEnumerable<ILogger> Loggers { get; set; }

		public static void Initialize(IEnumerable<ILogger> loggers)
		{
			Loggers = loggers;
		}

		#region Debug
		[Conditional("DEBUG")]
		public static void Debug(string text)
		{
			Write(LogType.Debug, text);
		}

		[Conditional("DEBUG")]
		public static void Debug(string format, params object[] args)
		{
			Write(LogType.Debug, format, args);
		}
		#endregion

		#region Info
		public static void Info(string text)
		{
			Write(LogType.Info, text);
		}

		public static void Info(string format, params object[] args)
		{
			Write(LogType.Info, format, args);
		}
		#endregion

		#region Warning
		public static void Warning(string text)
		{
			Write(LogType.Warn, text);
		}

		public static void Warning(string format, params object[] args)
		{
			Write(LogType.Warn, format, args);
		}
		#endregion

		#region Error
		public static void Error(string text)
		{
			Write(LogType.Error, text);
		}

		public static void Error(string format, params object[] args)
		{
			Write(LogType.Error, format, args);
		}

		public static void Error(Exception ex)
		{
			Write(ex);
		}
		#endregion

		public static void Write(LogType type, string text)
		{
			Write(type, text, null);
		}

		public static void Write(Exception ex)
		{
			Write(LogType.Error, ex.ToString());
		}

		public static void Write(LogType type, string format, params object[] args)
		{
			var msg = format;
			if (args != null && args.Length > 0)
				msg = string.Format(format, args);

			if (Loggers == null)
				return;

			foreach (var logger in Loggers)
			{
				logger.Write(type, msg);
			}
		}
	}
}
