
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

		#region Location
		private Unit _location = Unit.Empty;
		public Unit Location
		{
			get
			{
				if (_location == Unit.Empty)
				{
					_location = new Unit(Get<int>(CommonProperties.X), Get<int>(CommonProperties.Y), Get<int>(CommonProperties.Z));
				}
				return _location;
			}
			set
			{
				_location = value;
				Set(CommonProperties.X, value.X);
				Set(CommonProperties.Y, value.Y);
				Set(CommonProperties.Z, value.Z);
			}
		}
		#endregion

		public ExitCollection Exits { get; private set; }

		private readonly Dictionary<string,Mobile> _mobiles = new Dictionary<string, Mobile>();

		public Place()
		{
			Location = Unit.Empty;
			Exits = new ExitCollection(this);
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
