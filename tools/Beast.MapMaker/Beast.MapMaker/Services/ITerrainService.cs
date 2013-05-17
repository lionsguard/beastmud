using Beast.MapMaker.ViewModel;
using Beast.Mapping;
using System.Collections.Generic;

namespace Beast.MapMaker.Services
{
    public interface ITerrainService
    {
        TerrainViewModel GetTerrain(int id);
        IEnumerable<TerrainViewModel> GetAllTerrain();
        void SaveTerrain(TerrainViewModel terrain);
    }
}
