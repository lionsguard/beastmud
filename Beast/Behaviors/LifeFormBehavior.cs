
using System;

namespace Beast.Behaviors
{
	public class LifeFormBehavior : Behavior
	{
		public const string PropertyKeyHealth = "Health";

		protected override void OnAttached()
		{
			base.OnAttached();

			Owner.Updated += Update;

			Owner.Properties[PropertyKeyHealth] = 0;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			Owner.Updated -= Update;
		}

		private void Update(object sender, EventArgs e)
		{
			
		}
	}
}
