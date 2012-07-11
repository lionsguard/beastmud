using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace Beast.Web
{
	public static class RouteDataExtensions
	{
		public static T Value<T>(this RouteValueDictionary dictionary, string key)
		{
			return dictionary.Value<T>(key, default(T));
		}

		public static T Value<T>(this RouteValueDictionary dictionary, string key, T defaultValue)
		{
			object value = null;
			if (dictionary.ContainsKey(key))
			{
				value = dictionary[key];
			}
			if (value != null)
			{
				if (value is string && String.IsNullOrEmpty(value.ToString()))
				{
					value = null;
				}

				if (value != null)
				{
					if (typeof(T).IsEnum)
					{
						return (T)Enum.Parse(typeof(T), value.ToString(), true);
					}
					
					if (typeof(T) == typeof(Boolean))
					{
						value = Boolean.Parse(value.ToString());
					}
					else if (typeof(T) == typeof(string))
					{
						if (!String.IsNullOrEmpty(value.ToString()))
						{
							return (T)value;
						}
					}
					
					if (value is IConvertible)
					{
						return (T) Convert.ChangeType(value, typeof (T));
					}
					return (T) value;
				}
			}
			return defaultValue;
		}
		/// <summary>
		/// Merges the 2 source route value dictionaries.
		/// </summary>
		/// <param name="routeValueDictionary1">RouteValueDictionary 1.</param>
		/// <param name="routeValueDictionary2">RouteValueDictionary 2.</param>
		public static void Merge(this RouteValueDictionary routeValueDictionary1, RouteValueDictionary routeValueDictionary2)
		{
			if ((routeValueDictionary1 != null) & (routeValueDictionary2 != null))
			{
				foreach (KeyValuePair<string, object> routeElement in routeValueDictionary2)
				{
					routeValueDictionary1[routeElement.Key] = routeElement.Value;
				}
			}
		}
	}
}
