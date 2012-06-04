using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Commands
{
	public class CommandDefinition : IEqualityComparer<string>, IComparer<string>, IEquatable<string>
	{
		public string Name { get; set; }
		public string Summary { get; set; }
		public string Description { get; set; }
		public List<string> Synopsis { get; set; }
		public List<string> Aliases { get; set; }
		public List<string> Arguments { get; set; }

		public CommandDefinition()
		{
			Aliases = new List<string>();
			Arguments = new List<string>();
		}

		public CommandDefinition(string name, string summary, string description, List<string> synopsis, List<string> aliases, List<string> arguments)
		{
			Name = name;
			Summary = summary;
			Synopsis = synopsis;
			Description = description;
			Aliases = aliases ?? new List<string>();
			Arguments = arguments ?? new List<string>();
		}

		public int Compare(string x, string y)
		{
			var comp = string.Compare(x, y, true);
			if (comp == 0)
				return comp;

			return Aliases.Select(n => n.ToLower()).Any(n => string.Compare(n, y, true) == 0) ? 0 : -1;
		}

		public bool Equals(string other)
		{
			if (other == null)
				other = string.Empty;
			return Aliases.Select(n => n.ToLower()).Contains(other.ToLower());
		}

		public override bool Equals(object obj)
		{
			var cd = obj as CommandDefinition;
			if (cd == null)
				return false;

			var names = Aliases.Select(n => n.ToLower());
			var cdNames = cd.Aliases.Select(n => n.ToLower());

			return names.Any(n => cdNames.Contains(n));
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public static implicit operator string(CommandDefinition def)
		{
			return def.Aliases.FirstOrDefault();
		}

		public static implicit operator CommandDefinition(string name)
		{
			return new CommandDefinition {Aliases = new List<string>{name}};
		}

		public bool Equals(string x, string y)
		{
			if (x.ToLower() == y.ToLower())
				return true;

			return Aliases.Select(n => n.ToLower()).Contains(y.ToLower());
		}

		public int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}
}