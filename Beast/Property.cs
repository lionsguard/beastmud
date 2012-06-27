using System;

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

		//public static implicit operator Property(string name)
		//{
		//    return new Property(name, typeof(object), null);
		//}
	}
}