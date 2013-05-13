using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
	public struct Direction
	{
		public List<string> Aliases;
		public KnownDirection Value;
		public Unit Unit;

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

		#region Directions
		public static Direction North
		{
			get{return new Direction(KnownDirection.North, new Unit(0, -1, 0), "north", "n");}
		}
		public static Direction South
		{
			get { return new Direction(KnownDirection.South, new Unit(0, 1, 0), "south", "s"); }
		}
		public static Direction East
		{
			get { return new Direction(KnownDirection.East, new Unit(1, 0, 0), "east", "e"); }
		}
		public static Direction West
		{
			get { return new Direction(KnownDirection.West, new Unit(-1, 0, 0), "west", "w"); }
		}
		public static Direction Northeast
		{
			get { return new Direction(KnownDirection.Northeast, new Unit(1, -1, 0), "northeast", "ne"); }
		}
		public static Direction Northwest
		{
			get { return new Direction(KnownDirection.Northwest, new Unit(-1, -1, 0), "northwest", "ne"); }
		}
		public static Direction Southeast
		{
			get { return new Direction(KnownDirection.Southeast, new Unit(1, 1, 0), "southeast", "se"); }
		}
		public static Direction Southwest
		{
			get { return new Direction(KnownDirection.Southwest, new Unit(-1, 1, 0), "southwest", "sw"); }
		}
		public static Direction Up
		{
			get { return new Direction(KnownDirection.Up, new Unit(0, 0, 1), "up", "u"); }
		}
		public static Direction Down
		{
			get { return new Direction(KnownDirection.Down, new Unit(0, 0, -1), "down","d"); }
		}
		#endregion

		public static Direction FromAlias(string alias)
		{
			return All.FirstOrDefault(d => d.Aliases.Contains(alias.ToLower().Trim()));
		}

        public static Direction FromKnownDirection(KnownDirection direction)
        {
            return All.FirstOrDefault(d => d.Value == direction);
        }

        public static Direction FromUnit(Unit unit)
        {
            var x = unit.X;
            var y = unit.Y;
            var z = unit.Z;

            if (z > 0 && x == 0 && y == 0)
                return Up;
            if (z < 0 && x == 0 && y == 0)
                return Down;

            if (x > 0 && y == 0)
                return East;
            if (x < 0 && y == 0)
                return West;
            if (x > 0 && y < 0)
                return Northeast;
            if (x > 0 && y > 0)
                return Southeast;
            if (x < 0 && y < 0)
                return Northwest;
            if (x < 0 && y > 0)
                return Southwest;
            if (x == 0 && y > 0)
                return South;

            return Direction.North;
        }

        public static Direction FromPoints(Unit start, Unit end)
        {
            return FromUnit(start.MoveTowards(end));
        }

		public static IEnumerable<Direction> All
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