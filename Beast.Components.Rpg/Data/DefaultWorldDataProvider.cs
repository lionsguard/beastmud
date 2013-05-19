using Beast.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Data
{
    public class DefaultWorldDataProvider : DefaultDataProvider, IWorldDataProvider
    {
        public const string WorldDirectoryName = "world";
        public const string TerrainFileName = "terrain";
        public const string PlaceFlagsFileName = "place_flags";

        private string GetFileName(string name)
        {
            return GetPath(WorldDirectoryName, string.Concat(name, ".json"));
        }

        public IEnumerable<Terrain> GetTerrain()
        {
            return GetTerrain(GetFileName(TerrainFileName));
        }
        public IEnumerable<Terrain> GetTerrain(string fileName)
        {
            return GetObject<List<Terrain>>(fileName) ?? Terrain.DefaultTerrain;
        }

        public void SaveTerrain(IEnumerable<Terrain> terrain)
        {
            SaveTerrain(terrain, GetFileName(TerrainFileName));
        }
        public void SaveTerrain(IEnumerable<Terrain> terrain, string filename)
        {
            SaveObject(terrain.ToList(), filename);
        }

        public IEnumerable<PlaceFlag> GetPlaceFlags()
        {
            return GetPlaceFlags(GetFileName(PlaceFlagsFileName));
        }
        public IEnumerable<PlaceFlag> GetPlaceFlags(string fileName)
        {
            var obj = GetObject<List<PlaceFlag>>(fileName);
            if (obj == null)
                return PlaceFlag.All;
            return obj;
        }

        public void SavePlaceFlags(IEnumerable<PlaceFlag> flags)
        {
            SavePlaceFlags(flags, GetFileName(PlaceFlagsFileName));
        }
        public void SavePlaceFlags(IEnumerable<PlaceFlag> flags, string fileName)
        {
            SaveObject(flags.ToList(), fileName);
        }
    }
}
