
using System;
using System.Collections.Generic;

namespace Beast
{
	public static class ObjectManager
	{
		public static T Create<T>(string templateName, string name, params KeyValuePair<Property, object>[] properties) where T : IGameObject
		{
			// Find and load the template.
			var template = Game.Current.Repository.GetTemplate(templateName);

			// Create a new instance of the specified type.
			var obj = Activator.CreateInstance<T>();

			// Fill the properties of the type from the template.
			if (template != null)
			{
				
			}

			// Overwrite any specified properties.
			obj.Name = name;
			if (properties != null && properties.Length > 0)
			{
				foreach (var pair in properties)
				{
					obj[pair.Key] = pair.Value;
				}
			}
			return obj;
		}
	}
}
