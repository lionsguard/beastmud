using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Mobile : GameObject
	{
		public virtual void EnqueueDelta(DeltaMessage delta)
		{
		}
		public virtual void EnqueueDeltas(IEnumerable<DeltaMessage> deltas)
		{
		}

		public virtual IEnumerable<DeltaMessage> DequeueDeltas()
		{
			return new DeltaMessage[0];
		}
	}
}
