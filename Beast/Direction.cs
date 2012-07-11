using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast
{
	public struct Direction
	{
		public KnownDirection Value;
		public List<string> Aliases;

		public Direction(KnownDirection value, params string[] aliases)
		{
			Value = value;
			Aliases = new List<string>(aliases);
		}

		#region GetHashCode
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		#endregion

		#region Equals
		public bool Equals(Direction obj)
		{
			return obj.Value == Value;
		}

		public override bool Equals(object obj)
		{
			if (obj is Direction)
			{
				return Equals((Direction)obj);
			}
			return base.Equals(obj);
		}
		#endregion

		#region Static Members
		public static Direction FromNameOrAlias(string name)
		{
			KnownDirection dir;
			if (!Enum.TryParse(name, true, out dir))
			{
				// Try alias.
				if (All.Any(d => d.Aliases.Contains(name)))
				{
					return All.FirstOrDefault(d => d.Aliases.Contains(name));
				}

				return Void;
			}
			return dir.ToDirection();
		}
		#endregion

		#region Directions
		public static Direction Void
		{
			get { return new Direction(KnownDirection.Void); }
		}
		public static Direction North
		{
			get{return new Direction(KnownDirection.North, "n");}
		}
		public static Direction South
		{
			get { return new Direction(KnownDirection.South, "s"); }
		}
		public static Direction East
		{
			get { return new Direction(KnownDirection.East, "e"); }
		}
		public static Direction West
		{
			get { return new Direction(KnownDirection.West, "w"); }
		}
		public static Direction Northeast
		{
			get { return new Direction(KnownDirection.Northeast,"ne"); }
		}
		public static Direction Northwest
		{
			get { return new Direction(KnownDirection.Northwest, "nw"); }
		}
		public static Direction Southeast
		{
			get { return new Direction(KnownDirection.Southeast, "se"); }
		}
		public static Direction Southwest
		{
			get { return new Direction(KnownDirection.Southwest, "sw"); }
		}
		public static Direction Up
		{
			get { return new Direction(KnownDirection.Up); }
		}
		public static Direction Down
		{
			get { return new Direction(KnownDirection.Down); }
		}
		public static Direction Enter
		{
			get { return new Direction(KnownDirection.Enter); }
		}
		#endregion

		public static IEnumerable<Direction> All
		{
			get
			{
				return new[]
				       	{
							Void,
							North,
							South,
							East,
							West,
							Northeast,
							Northwest,
							Southeast,
							Southwest,
							Up,
							Down,
							Enter
				       	};
			}
		}

		public static IEnumerable<Direction> Movement
		{
			get
			{
				return new[]
				       	{
							North,
							South,
							East,
							West,
							Northeast,
							Northwest,
							Southeast,
							Southwest,
							Up,
							Down
				       	};	
			}
		}
	}
}