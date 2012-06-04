using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast
{
	public struct Direction
	{
		public KnownDirection Value;
		public Unit Unit;
		public List<string> Aliases;

		public Direction(KnownDirection value, Unit unit, params string[] aliases)
		{
			Value = value;
			Unit = unit;
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
			get { return new Direction(KnownDirection.Void, new Unit(0, 0, 0)); }
		}
		public static Direction North
		{
			get{return new Direction(KnownDirection.North, new Unit(0, -1, 0));}
		}
		public static Direction South
		{
			get { return new Direction(KnownDirection.South, new Unit(0, 1, 0)); }
		}
		public static Direction East
		{
			get { return new Direction(KnownDirection.East, new Unit(1, 0, 0)); }
		}
		public static Direction West
		{
			get { return new Direction(KnownDirection.West, new Unit(-1, 0, 0)); }
		}
		public static Direction Northeast
		{
			get { return new Direction(KnownDirection.Northeast, new Unit(1, -1, 0)); }
		}
		public static Direction Northwest
		{
			get { return new Direction(KnownDirection.Northwest, new Unit(-1, -1, 0)); }
		}
		public static Direction Southeast
		{
			get { return new Direction(KnownDirection.Southeast, new Unit(1, 1, 0)); }
		}
		public static Direction Southwest
		{
			get { return new Direction(KnownDirection.Southwest, new Unit(-1, 1, 0)); }
		}
		public static Direction Up
		{
			get { return new Direction(KnownDirection.Up, new Unit(0, 0, 1)); }
		}
		public static Direction Down
		{
			get { return new Direction(KnownDirection.Down, new Unit(0, 0, -1)); }
		}
		public static Direction Enter
		{
			get { return new Direction(KnownDirection.Enter, new Unit(0, 0, 0)); }
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
	}
}