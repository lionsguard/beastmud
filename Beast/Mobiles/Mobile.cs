using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Mobile : GameObject
	{
		#region Position
		private Unit _position = Unit.Empty;
		public Unit Position
		{
			get
			{
				if (_position == Unit.Empty)
				{
					_position = new Unit(Get<int>(CommonProperties.X), Get<int>(CommonProperties.Y), Get<int>(CommonProperties.Z));
				}
				return _position;
			}
			set
			{
				_position = value;
				Set(CommonProperties.X, value.X);
				Set(CommonProperties.Y, value.Y);
				Set(CommonProperties.Z, value.Z);
			}
		}
		#endregion

		public virtual void EnqueueMessages(params IMessage[] messages)
		{
		}

		public virtual IEnumerable<IMessage> DequeueMessages()
		{
			return new IMessage[0];
		}
	}
}
