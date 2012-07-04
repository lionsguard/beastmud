
namespace Beast
{
	public class ExitCollection : OwnedPropertyCollection<string>
	{
		public Place this[KnownDirection direction]
		{
			get { return GetPlace(direction); }
			set { SetPlace(direction, value); }
		}

		public Place North
		{
			get { return this[KnownDirection.North]; }
			set { this[KnownDirection.North] = value; }
		}

		public Place South
		{
			get { return this[KnownDirection.South]; }
			set { this[KnownDirection.South] = value; }
		}

		public Place East
		{
			get { return this[KnownDirection.East]; }
			set { this[KnownDirection.East] = value; }
		}

		public Place West
		{
			get { return this[KnownDirection.West]; }
			set { this[KnownDirection.West] = value; }
		}

		public Place Northeast
		{
			get { return this[KnownDirection.Northeast]; }
			set { this[KnownDirection.Northeast] = value; }
		}

		public Place Northwest
		{
			get { return this[KnownDirection.Northwest]; }
			set { this[KnownDirection.Northwest] = value; }
		}

		public Place Southeast
		{
			get { return this[KnownDirection.Southeast]; }
			set { this[KnownDirection.Southeast] = value; }
		}

		public Place Southwest
		{
			get { return this[KnownDirection.Southwest]; }
			set { this[KnownDirection.Southwest] = value; }
		}

		public Place Up
		{
			get { return this[KnownDirection.Up]; }
			set { this[KnownDirection.Up] = value; }
		}

		public Place Down
		{
			get { return this[KnownDirection.Down]; }
			set { this[KnownDirection.Down] = value; }
		}

		public ExitCollection(IGameObject owner) 
			: base(owner, string.Empty)
		{
		}

		private Place GetPlace(KnownDirection direction)
		{
			var id = Get(direction.ToString());
			if (string.IsNullOrEmpty(id))
				return null;

			return World.GetPlace(id);
		}

		private void SetPlace(KnownDirection direction, Place value)
		{
			if (value != null)
			{
				if (string.IsNullOrEmpty(value.Id))
					World.SavePlace(value);
			}

			Set(direction.ToString(), value != null ? value.Id : null);
		}
	}
}