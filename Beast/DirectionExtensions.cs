namespace Beast
{
	public static class DirectionExtensions
	{
		public static Direction Counter(this Direction direction)
		{
			switch (direction.Value)
			{
				case KnownDirection.North:
					return Direction.South;
				case KnownDirection.South:
					return Direction.North;
				case KnownDirection.East:
					return Direction.West;
				case KnownDirection.West:
					return Direction.East;
				case KnownDirection.Northeast:
					return Direction.Southwest;
				case KnownDirection.Northwest:
					return Direction.Southeast;
				case KnownDirection.Southeast:
					return Direction.Northwest;
				case KnownDirection.Southwest:
					return Direction.Northeast;
				case KnownDirection.Up:
					return Direction.Down;
				case KnownDirection.Down:
					return Direction.Up;
				case KnownDirection.Enter:
					return Direction.Enter;
				default:
					// Void
					return Direction.Void;
			}
		}

		public static Direction ToDirection(this KnownDirection value)
		{
			switch (value)
			{
				case KnownDirection.North:
					return Direction.North;
				case KnownDirection.South:
					return Direction.South;
				case KnownDirection.East:
					return Direction.East;
				case KnownDirection.West:
					return Direction.West;
				case KnownDirection.Northeast:
					return Direction.Northeast;
				case KnownDirection.Northwest:
					return Direction.Northwest;
				case KnownDirection.Southeast:
					return Direction.Southeast;
				case KnownDirection.Southwest:
					return Direction.Southwest;
				case KnownDirection.Up:
					return Direction.Up;
				case KnownDirection.Down:
					return Direction.Down;
				case KnownDirection.Enter:
					return Direction.Enter;
				default:
					// Void
					return Direction.Void;
			}
		}
	}
}