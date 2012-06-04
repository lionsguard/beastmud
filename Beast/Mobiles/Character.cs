
using System.Collections.Generic;
using Beast.Net;

namespace Beast.Mobiles
{
	public abstract class Character : Mobile
	{
		#region Deltas
		private readonly Queue<DeltaMessage> _deltas = new Queue<DeltaMessage>();
		public override void EnqueueDelta(DeltaMessage delta)
		{
			lock (_deltas)
			{
				_deltas.Enqueue(delta);
			}
		}

		public override void EnqueueDeltas(IEnumerable<DeltaMessage> deltas)
		{
			lock (_deltas)
			{
				foreach (var delta in deltas)
				{
					_deltas.Enqueue(delta);
				}
			}
		}

		public override IEnumerable<DeltaMessage> DequeueDeltas()
		{
			lock (_deltas)
			{
				while (_deltas.Count > 0)
				{
					yield return _deltas.Dequeue();
				}
			}
		}
		#endregion
	}
}