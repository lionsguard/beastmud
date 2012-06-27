

using System;

namespace Beast.Behaviors
{
	public class LifeFormBehavior : Behavior
	{
		public static readonly TimeSpan DefaultRefreshVitalsWaitTime = TimeSpan.FromMinutes(1);
		public const int DefaultBodyIncrement = 1;
		public const int DefaultMindIncrement = 1;

		private TimeSpan _lastUpdate = TimeSpan.Zero;

		public TimeSpan RefreshVitalsWaitTime { get; set; }
		public int BodyIncrement { get; set; }
		public int MindIncrement { get; set; }

		public LifeFormBehavior()
		{
			RefreshVitalsWaitTime = DefaultRefreshVitalsWaitTime;
			BodyIncrement = DefaultBodyIncrement;
			MindIncrement = DefaultMindIncrement;
		}
		
		protected override void OnAttached()
		{
			base.OnAttached();

			// Attach to the updating event.
			EventManager.Register(CommonEvents.Updating, Owner, OnUpdating);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			EventManager.Unregister(CommonEvents.Updating, Owner, OnUpdating);
		}

		private void OnUpdating(IGameObject obj, EventArgs e)
		{
			if(Owner != obj)
				return;

			if (!Owner.Get<bool>(CommonProperties.IsAtRest))
				return;

			var elapsed = Game.Current.GameTime.ElapsedGameTime;
			if ((_lastUpdate - elapsed) > RefreshVitalsWaitTime)
			{
				var body = Owner.Get<BoundProperty<int>>(CommonProperties.Body);
				if (body.Value < body.Maximum)
					body.Value += BodyIncrement;
				
				var mind = Owner.Get<BoundProperty<int>>(CommonProperties.Mind);
				if (mind.Value < mind.Maximum)
					mind.Value += MindIncrement;

				_lastUpdate = Game.Current.GameTime.TotalGameTime;
			}
		}
	}
}
