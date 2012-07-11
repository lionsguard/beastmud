
using System;
using System.Collections.Generic;

namespace Beast
{
	public interface IKnownTypeDefinition
	{
		IEnumerable<Type> KnownTypes { get; }
	}
}
