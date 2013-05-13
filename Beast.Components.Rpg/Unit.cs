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

		public double Length()
		{
			double num = (X * X) + (Y * Y);
			return Math.Sqrt(num);
		}

		public double LengthSquared()
		{
			return ((X * X) + (Y * Y));
		}

		public double RotateTowards(Unit destination)
		{
			return Math.Atan2(destination.Y - Y, destination.X - X);
		}

		public double RotateTowards(Unit destination, double rotation, double rotationOffset)
		{
			var theta = Math.Atan2(destination.Y - Y, destination.X - X);
			var diff = theta - rotation;
			return diff + rotation - rotationOffset;
		}

		public Unit MoveTowards(Unit destination)
		{
			var edge = destination - this;
			//int x = 0, y = 0;
			edge.Normalize();
			//if (edge.X != 0)
			//{
			//    x = edge.X > 0 ? 1 : -1;
			//}
			//if (edge.Y != 0)
			//{
			//    y = edge.Y > 0 ? 1 : -1;
			//}
			//return new Unit(x, y, 0);
			return edge;
		}

		public void Normalize()
		{
			var num2 = (X * X) + (Y * Y);
			var num = 1d / (Math.Sqrt(num2));
			X = (int)Math.Round(X * num);
			Y = (int)Math.Round(Y * num);
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

		public static bool operator >=(Unit u1, Unit u2)
		{
			if (u1.X >= u2.X)
			{
				if (u1.Y >= u2.Y)
					return u1.Z >= u2.Z;
			}
			return false;
		}

		public static bool operator <=(Unit u1, Unit u2)
		{
			if (u1.X <= u2.X)
			{
				if (u1.Y <= u2.Y)
					return u1.Z <= u2.Z;
			}
			return false;
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