namespace Beast
{
	public class ExitCollection : OwnedPropertyCollection<bool>
	{
		public bool this[KnownDirection direction]
		{
			get { return Get(direction.ToString()); }
			set { Set(direction.ToString(), value); }
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

		public ExitCollection(IGameObject owner) 
			: base(owner, string.Empty)
		{
		}
	}
}