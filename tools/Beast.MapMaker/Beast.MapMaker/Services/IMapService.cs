using Beast.Mapping;

namespace Beast.MapMaker.Services
{
    public interface IMapService
    {
        Map GetMap(string fileName);
        void SaveMap(Map map, string fileName);
    }
}
