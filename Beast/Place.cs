
using System.Collections.Generic;
using System.Collections.Specialized;
using Beast.Mobiles;
using Beast.Net;

namespace Beast
{
	public class Place
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Terrain Terrain { get; set; }
		public BitVector32 Flags { get; set; }
		public ExitCollection Exits { get; set; }
		public Unit Location { get; set; }

		private readonly Dictionary<string,Mobile> _mobiles = new Dictionary<string, Mobile>();

		public Place()
		{
			Flags = new BitVector32();
			Exits = new ExitCollection();
			Terrain = Terrain.Empty;
			Location = Unit.Empty;
		}

		public void Enter(Mobile who, KnownDirection direction)
		{
			who.Position = Location;
			_mobiles.Add(who.Id, who);
			who.EnqueueMessages(NotificationMessage.Name(Name), NotificationMessage.Location(Location.ToString()));
		}

		public void Exit(Mobile who, KnownDirection direction)
		{	
			_mobiles.Remove(who.Id);
		}

		public void Look(Mobile looker)
		{
			looker.EnqueueMessages(NotificationMessage.Name(Name), NotificationMessage.Location(Location.ToString()), NotificationMessage.Description(Description));
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
