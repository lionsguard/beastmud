namespace Beast.Data
{
	public interface IWorldRepository : IRepository
	{
		IWorld GetWorld();
		void SaveWorld<T>(T world) where T : class, IWorld;
	}
}