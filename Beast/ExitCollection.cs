using System;
using System.Collections;
using System.Collections.Generic;

namespace Beast
{
	public class ExitCollection : IEnumerable<bool>
	{
		private readonly Dictionary<KnownDirection, bool> _values;

		public bool this[KnownDirection direction]
		{
			get { return _values[direction]; }
			set { _values[direction] = value; }
		}

		public bool North
		{
			get { return this[KnownDirection.North]; }
			set { this[KnownDirection.North] = value; }
		}

		public bool South
		{
			get { return this[KnownDirection.South]; }
			set { this[KnownDirection.South] = value; }
		}

		public bool East
		{
			get { return this[KnownDirection.East]; }
			set { this[KnownDirection.East] = value; }
		}

		public bool West
		{
			get { return this[KnownDirection.West]; }
			set { this[KnownDirection.West] = value; }
		}

		public bool Northeast
		{
			get { return this[KnownDirection.Northeast]; }
			set { this[KnownDirection.Northeast] = value; }
		}

		public bool Northwest
		{
			get { return this[KnownDirection.Northwest]; }
			set { this[KnownDirection.Northwest] = value; }
		}

		public bool Southeast
		{
			get { return this[KnownDirection.Southeast]; }
			set { this[KnownDirection.Southeast] = value; }
		}

		public bool Southwest
		{
			get { return this[KnownDirection.Southwest]; }
			set { this[KnownDirection.Southwest] = value; }
		}

		public bool Up
		{
			get { return this[KnownDirection.Up]; }
			set { this[KnownDirection.Up] = value; }
		}

		public bool Down
		{
			get { return this[KnownDirection.Down]; }
			set { this[KnownDirection.Down] = value; }
		}

		public ExitCollection()
		{
			_values = new Dictionary<KnownDirection, bool>();
			foreach (var direction in Direction.All)
			{
				_values.Add(direction.Value, false);
			}
		}

		private List<bool> GetList()
		{
			return new List<bool>(new[] {North, South, East, West, Northeast, Northwest, Southeast, Southwest, Up, Down});
		}

		#region Implementation of IEnumerable

		public IEnumerator<bool> GetEnumerator()
		{
			return GetList().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		public override string ToString()
		{
			return string.Join(",", GetList());
		}

		public static ExitCollection FromString(string values)
		{
			var exits = new ExitCollection();
			var parts = values.Split(',');
			if (parts.Length <= Enum.GetValues(typeof(KnownDirection)).Length)
			{
				for (var i = 0; i < parts.Length; i++)
				{
					if (string.IsNullOrEmpty(parts[i]))
						continue;
					exits[(KnownDirection) i] = Convert.ToBoolean(parts[i]);
				}
			}
			return exits;
		}
	}
}