
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

		public ExitCollection Exits { get; private set; }

		private readonly Dictionary<string,Mobile> _mobiles = new Dictionary<string, Mobile>();

		public Place()
		{
			Exits = new ExitCollection(this);
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
