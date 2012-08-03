using System;
using System.Xml.Linq;

namespace Beast
{
	public static class XExtensions
	{
		public static XElement ElementOrDefault(this XElement element, XName name)
		{
			return element.Element(name) ?? new XElement(name);
		}

		public static string GetStringValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			return attr != null ? attr.Value : string.Empty;
		}

		public static Guid GetGuidValue(this XElement element)
		{
			Guid value;
			Guid.TryParse(element.Value, out value);
			return value;
		}
		public static Guid GetGuidValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null) return Guid.Empty;
			Guid value;
			Guid.TryParse(attr.Value, out value);
			return value;
		}

		public static byte GetByteValue(this XElement element)
		{
			byte result;
			return byte.TryParse(element.Value, out result) ? result : (byte)0;
		}
		public static byte GetByteValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return 0;
			byte result;
			return byte.TryParse(attr.Value, out result) ? result : (byte)0;
		}

		public static short GetInt16Value(this XElement element)
		{
			short result;
			return Int16.TryParse(element.Value, out result) ? result : (short)0;
		}
		public static short GetInt16Value(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return 0;
			short result;
			return Int16.TryParse(attr.Value, out result) ? result : (short)0;
		}

		public static int GetInt32Value(this XElement element)
		{
			int result;
			return Int32.TryParse(element.Value, out result) ? result : 0;
		}
		public static int GetInt32Value(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return 0;
			int result;
			return Int32.TryParse(attr.Value, out result) ? result : 0;
		}

		public static long GetInt64Value(this XElement element)
		{
			long result;
			return Int64.TryParse(element.Value, out result) ? result : 0L;
		}
		public static long GetInt64Value(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return 0;
			long result;
			return Int64.TryParse(attr.Value, out result) ? result : 0L;
		}

		public static double GetDoubleValue(this XElement element)
		{
			double result;
			return Double.TryParse(element.Value, out result) ? result : 0;
		}
		public static double GetDoubleValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return 0;
			double result;
			return Double.TryParse(attr.Value, out result) ? result : 0;
		}

		public static DateTime GetDateTimeValue(this XElement element)
		{
			DateTime result;
			return DateTime.TryParse(element.Value, out result) ? result : DateTime.MinValue;
		}
		public static DateTime GetDateTimeValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return DateTime.MinValue;
			DateTime result;
			return DateTime.TryParse(attr.Value, out result) ? result : DateTime.MinValue;
		}

		public static bool GetBooleanValue(this XElement element)
		{
			bool result;
			return Boolean.TryParse(element.Value, out result) ? result : false;
		}
		public static bool GetBooleanValue(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return false;
			bool result;
			return Boolean.TryParse(attr.Value, out result) ? result : false;
		}

		public static object GetEnumValue(this XElement element, Type enumType)
		{
			return Enum.Parse(enumType, element.Value, true);
		}
		public static object GetEnumValue(this XElement element, string attributeName, Type enumType)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return Enum.GetValues(enumType).GetValue(0);
			return Enum.Parse(enumType, attr.Value, true);
		}
		public static T GetEnumValue<T>(this XElement element, string attributeName)
		{
			var attr = element.Attribute(attributeName);
			if (attr == null)
				return (T)Enum.GetValues(typeof(T)).GetValue(0);
			return (T)Enum.Parse(typeof(T), attr.Value, true);
		}
	}
}