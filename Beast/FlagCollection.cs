namespace Beast
{
	public class FlagCollection : OwnedPropertyCollection<bool>
	{
		public FlagCollection(IGameObject owner)
			: base(owner, "Flag")
		{
		}
	}
}