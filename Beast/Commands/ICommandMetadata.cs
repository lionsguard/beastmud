
using System.Collections.Generic;

namespace Beast.Commands
{
	public interface ICommandMetadata
	{
		string Name { get; }
		string Description { get; }
		string Synopsis { get; }
		IEnumerable<string> Aliases { get; }
		IEnumerable<string> Arguments { get; }
	}
}
