using System;

namespace Beast
{
	public interface ITypeResolver
	{
		Type ResolveType(string name);
	}
}