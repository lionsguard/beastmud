using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Beast
{
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay("X:{X}, Y:{Y}, Z:{Z}")]
	public struct Unit
	{
		public static readonly Unit Empty = new Unit(int.MinValue, int.MinValue, int.MinValue);
		public static readonly Unit Zero = new Unit(0, 0, 0);

		private int _x;
		private int _y;
		private int _z;

		public int X
		{
			get { return _x; }
			set { _x = value; }
		}

		public int Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public int Z
		{
			get { return _z; }
			set { _z = value; }
		}

		public Unit(int x, int y, int z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public int DistanceTo(Unit unit)
		{
			return DistanceTo(unit.X, unit.Y);
		}

		public int DistanceTo(int x, int y)
		{
			var xd = X - x;
			var yd = Y - y;
			return (int)Math.Sqrt(xd * xd + yd * yd);
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", X, Y, Z);
		}

		public static Unit operator +(Unit u1, Unit u2)
		{
			return new Unit(u1.X + u2.X, u1.Y + u2.Y, u1.Z + u2.Z);
		}

		public static Unit operator -(Unit u1, Unit u2)
		{
			return new Unit(u1.X - u2.X, u1.Y - u2.Y, u1.Z - u2.Z);
		}

		public static bool operator ==(Unit u1, Unit u2)
		{
			return ((u1.X == u2.X) && (u1.Y == u2.Y) && (u1.Z == u2.Z));
		}

		public static bool operator !=(Unit u1, Unit u2)
		{
			return !(u1 == u2);
		}

		public override bool Equals(object o)
		{
			if ((o == null) || !(o is Unit))
			{
				return false;
			}
			var unit = (Unit)o;
			return (this == unit);
		}

		public bool Equals(Unit value)
		{
			return (this == value);
		}

		public override int GetHashCode()
		{
			return (X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode());
		}

		public static Unit Parse(string value)
		{
			Unit unit;
			TryParse(value, out unit);
			return unit;
		}

		public static bool TryParse(string value, out Unit result)
		{
			result = new Unit(0, 0, 0);

			if (string.IsNullOrEmpty(value))
				return false;

			var parts = value.Split(',');
			if (parts.Length != 3)
				return false;

			int x, y, z;
			if (!int.TryParse(parts[0], out x))
				return false;
			if (!int.TryParse(parts[1], out y))
				return false;
			if (!int.TryParse(parts[2], out z))
				return false;

			result._x = x;
			result._y = y;
			result._z = z;
			return true;
		}
	}

	public class UnitEqualityComparer : IEqualityComparer<Unit>
	{
		#region Implementation of IEqualityComparer<Unit>

		public bool Equals(Unit x, Unit y)
		{
			return x == y;
		}

		public int GetHashCode(Unit obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}