using System;
using System.Linq;

namespace Beast
{
	internal sealed class BeastTypeResolver : ITypeResolver
	{
		public Type ResolveType(string name)
		{
			return typeof (GameObject).Assembly.GetTypes().FirstOrDefault(t => string.Compare(t.Name, name, true) == 0);
		}
	}
}
