using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Beast.Items;
using Beast.Mobiles;
using Beast.Security;

namespace Beast
{
	[Export(typeof(IKnownTypeDefinition))]
	public class KnownTypeDefinition : IKnownTypeDefinition
	{
		public IEnumerable<Type> KnownTypes
		{
			get
			{
				return new[]
				       	{
				       		typeof(DataObject),
				       		typeof(GameObject),
				       		typeof(User),
				       		typeof(Login),
				       		typeof(GenericLogin),
				       		typeof(Mobile),
				       		typeof(Character),
				       		typeof(Item),
				       		typeof(Place),
				       		typeof(Terrain),
				       	};
			}
		}
	}
}