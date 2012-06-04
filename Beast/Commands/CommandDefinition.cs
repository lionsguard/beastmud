using System.Collections.Generic;
using System.Linq;

namespace Beast.Commands
{
	public class CommandDefinition
	{
		public string Name { get; set; }
		public string Summary { get; set; }
		public string Description { get; set; }
		public List<string> Synopsis { get; set; }
		public List<string> Aliases { get; set; }
		public List<string> Arguments { get; set; }

		public CommandDefinition()
		{
			Name = string.Empty;
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
			return Aliases.Aggregate(0, (current, alias) => current ^ alias.GetHashCode());
		}

		public static implicit operator string(CommandDefinition def)
		{
			return def.Aliases.FirstOrDefault();
		}

		public static implicit operator CommandDefinition(string name)
		{
			return new CommandDefinition {Name = name, Aliases = new List<string>{name}};
		}
	}
}