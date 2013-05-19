using Beast.Mapping;

namespace Beast.MapMaker.Services
{
    public interface IMapService
    {
        Map GetMap();
        void SaveMap(Map map);
    }
}
