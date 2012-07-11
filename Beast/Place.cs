
using System.Collections.Generic;
using Beast.Mobiles;
using Beast.Net;

namespace Beast
{
	public class Place : GameObject
	{
		#region Terrain
		public int Terrain
		{
			get { return Get<int>(PropertyTerrain); }
			set { Set(PropertyTerrain, value); }
		}
		public static readonly string PropertyTerrain = "Terrain";
		#endregion

		#region Exits
		public Place North
		{
			get { return Exits.North; }
			set { Exits.North = value; }
		}
		public Place South
		{
			get { return Exits.South; }
			set { Exits.South = value; }
		}
		public Place East
		{
			get { return Exits.East; }
			set { Exits.East = value; }
		}
		public Place West
		{
			get { return Exits.West; }
			set { Exits.West = value; }
		}
		public Place Northeast
		{
			get { return Exits.Northeast; }
			set { Exits.Northeast = value; }
		}
		public Place Northwest
		{
			get { return Exits.Northwest; }
			set { Exits.Northwest = value; }
		}
		public Place Southeast
		{
			get { return Exits.Southeast; }
			set { Exits.Southeast = value; }
		}
		public Place Southwest
		{
			get { return Exits.Southwest; }
			set { Exits.Southwest = value; }
		}
		public Place Up
		{
			get { return Exits.Up; }
			set { Exits.Up = value; }
		}
		public Place Down
		{
			get { return Exits.Down; }
			set { Exits.Down = value; }
		}
		#endregion

		public ExitCollection Exits { get; private set; }

		private readonly Dictionary<string,Mobile> _mobiles = new Dictionary<string, Mobile>();

		public Place()
		{
			Exits = new ExitCollection(this);
		}

		public bool HasExit(KnownDirection direction)
		{
			return Exits.HasExit(direction);
		}

		public void Enter(Mobile who, KnownDirection direction)
		{
			who.Place = this;
			_mobiles.Add(who.Id, who);
			who.EnqueueMessages(NotificationMessage.Name(Name));
		}

		public void Exit(Mobile who, KnownDirection direction)
		{	
			_mobiles.Remove(who.Id);
		}

		public void Look(Mobile looker)
		{
			looker.EnqueueMessages(NotificationMessage.Name(Name), NotificationMessage.Description(Description));
		}

		public void Broadcast(Mobile sender, params IMessage[] messages)
		{
			foreach (var mobile in _mobiles.Values)
			{
				if (mobile.Id == sender.Id)
					continue;
				mobile.EnqueueMessages(messages);
			}
		}
	}
}
