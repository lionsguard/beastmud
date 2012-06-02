using System;

namespace Beast
{
	public static class DateTimeEx
	{
		public static DateTime FromEpochMilliseconds(long milliseconds)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);
		}

		public static DateTime FromEpochSeconds(int seconds)
		{
			return FromEpochMilliseconds((long)seconds * 1000);
		}

		public static DateTime DateTimeFromEpochMilliSeconds(this long milliseconds)
		{
			return FromEpochMilliseconds(milliseconds);
		}

		public static DateTime DateTimeFromEpochSeconds(this int seconds)
		{
			return FromEpochSeconds(seconds);
		}

		public static TimeSpan ToEpoch(this DateTime dt)
		{
			return dt.ToUniversalTime() - new DateTime(1970, 1, 1);
		}

		public static int ToEpochSeconds(this DateTime dt)
		{
			return (int)(ToEpochMilliseconds(dt) / 1000);
		}

		public static long ToEpochMilliseconds(this DateTime dt)
		{
			return (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}
	}
}
