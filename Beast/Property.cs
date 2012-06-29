using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Beast
{
	public class Property
	{
		public string Name { get; private set; }
		public Type PropertyType { get; private set; }
		public object DefaultValue { get; private set; }

		public Property(string name, Type propertyType, object defaultValue)
		{
			Name = name;
			PropertyType = propertyType;

			if (defaultValue == null && propertyType.IsValueType)
				defaultValue = Activator.CreateInstance(propertyType);
			DefaultValue = defaultValue;
		}

		public override bool Equals(object obj)
		{
			var prop = obj as Property;
			return prop != null && (prop.Name == Name && prop.PropertyType == PropertyType);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ PropertyType.GetHashCode();
		}

		public static implicit operator string(Property property)
		{
			return property.Name;
		}

		#region Find Properties

		private const BindingFlags PropertySearchFlags = BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic;
		private static readonly Dictionary<Type, IEnumerable<Property>> CachedProperties = new Dictionary<Type, IEnumerable<Property>>();

		public static IEnumerable<Property> FindProperties(object obj)
		{
			if (obj == null)
				return new Property[0];

			var type = obj.GetType();
			if (CachedProperties.ContainsKey(type))
				return CachedProperties[type];

			// Search CommonProperties first,then the object itself.
			var commonProps = typeof(CommonProperties).GetFields(PropertySearchFlags);
			var list = (from prop in commonProps where prop.FieldType == typeof (Property) select (Property) prop.GetValue(null)).ToList();

			var instanceProps = type.GetFields(PropertySearchFlags);
			list.AddRange(from prop in instanceProps where prop.FieldType == typeof(Property) select (Property)prop.GetValue(null));

			CachedProperties[type] = list;

			return list;
		}
		#endregion
	}
}