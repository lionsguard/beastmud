namespace Beast
{
	public interface IModule : IUpdatable
	{
		void Initialize();
		void Shutdown();
	}
}