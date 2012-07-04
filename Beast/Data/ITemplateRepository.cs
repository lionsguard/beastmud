namespace Beast.Data
{
	public interface ITemplateRepository : IRepository
	{
		long GetTemplateCount();
		IGameObject GetTemplate(string templateName);
		void SaveTemplate(IGameObject obj);
	}
}