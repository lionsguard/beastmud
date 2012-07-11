using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Mobile : GameObject
	{
		public Place Place
		{
			get { return Game.Current.World.GetPlace(Get<string>(CommonProperties.PlaceId)); }
			set
			{
				if (value != null && string.IsNullOrEmpty(value.Id))
					Game.Current.World.SavePlace(value);

				Set(CommonProperties.PlaceId, value != null ? value.Id : null);
			}
		}

		public virtual void EnqueueMessages(params IMessage[] messages)
		{
		}

		public virtual IEnumerable<IMessage> DequeueMessages()
		{
			return new IMessage[0];
		}
	}
}
