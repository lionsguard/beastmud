using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Beast
{
	public static class ReflectionExtensions
	{
		public static string GetPropertyName<T>(Expression<Func<T>> expression)
		{
			var body = (MemberExpression)expression.Body;
			return body.Member.Name;
		}

		public static MemberInfo GetProperty<T>(Expression<Func<T>> expression)
		{
			var body = (MemberExpression)expression.Body;
			return body.Member;
		}

		public static PropertyInfo GetProperty<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
		{
			var body = (MemberExpression)expression.Body;
			return (PropertyInfo)body.Member;
		}

		public static object GetValue<T>(this T obj, string name)
		{
			var mi = obj.GetType().GetMember(name).FirstOrDefault();
			return mi == null ? null : mi.GetValue(obj);
		}

		public static object GetValue(this MemberInfo mi, object o)
		{
			var fi = mi as FieldInfo;
			if (fi != null)
				return fi.GetValue(o);
			var pi = mi as PropertyInfo;
			if (pi == null)
				return null;
			var getMethod = pi.GetGetMethod();
			return getMethod.Invoke(o, new object[0]);
		}

		public static void SetValue(this MemberInfo mi, object o, object val)
		{
			var fi = mi as FieldInfo;
			if (fi != null)
			{
				fi.SetValue(o, val);
				return;
			}
			var pi = mi as PropertyInfo;
			if (pi == null)
				return;
			var setMethod = pi.GetSetMethod();
			var nVal = Convert.ChangeType(val, Type.GetTypeCode(pi.PropertyType));
			setMethod.Invoke(o, new object[] { nVal });
		}
	}
}
