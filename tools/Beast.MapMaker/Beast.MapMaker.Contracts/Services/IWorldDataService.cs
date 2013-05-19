using Beast.Mapping;
using System.Collections.Generic;

namespace Beast.MapMaker.Services
{
    public interface IWorldDataService
    {
        IEnumerable<Terrain> GetTerrain();
        void SaveTerrain(IEnumerable<Terrain> terrain);

        IEnumerable<PlaceFlag> GetPlaceFlags();
        void SavePlaceFlags(IEnumerable<PlaceFlag> flags);
    }
}
