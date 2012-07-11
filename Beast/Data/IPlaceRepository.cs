
namespace Beast.Data
{
	public interface IPlaceRepository : IRepository
	{
		long GetPlaceCount();
		Place GetPlace(string id);
		void SavePlace(Place place);
	}
}