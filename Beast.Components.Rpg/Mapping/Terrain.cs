
using System.Collections.Generic;
namespace Beast.Mapping
{
    public class Terrain
    {
        public static readonly Terrain Empty = new Terrain { Id = 0, Name = "None", Color = "1C1C1C" };

        #region Default Terrain
        public static readonly IEnumerable<Terrain> DefaultTerrain = new[]
        {
            new Terrain{Id = 1, Name = "Beach", Color = "ffbf40"},
            new Terrain{Id = 2, Name = "Desert", Color = "bf8f30"},
            new Terrain{Id = 3, Name = "Urban", Color = "8c8c8c"},
            new Terrain{Id = 4, Name = "Jungle", Color = "539700"},
            new Terrain{Id = 5, Name = "Forest", Color = "008209"},
            new Terrain{Id = 6, Name = "Fresh Water", Color = "0058c7"},
            new Terrain{Id = 7, Name = "Sea Water", Color = "00b361"},
            new Terrain{Id = 8, Name = "River", Color = "0000c7"},
            new Terrain{Id = 9, Name = "Grass", Color = "00cc00"},
            new Terrain{Id = 10, Name = "Mountains", Color = "333333"},
            new Terrain{Id = 11, Name = "Swamp", Color = "84e900"},
            new Terrain{Id = 12, Name = "Path", Color = "966b4f"},
            new Terrain{Id = 13, Name = "Road", Color = "666666"},
            new Terrain{Id = 14, Name = "Hills", Color = "a65f00"},
            new Terrain{Id = 15, Name = "Garden", Color = "45bf42"},
            new Terrain{Id = 16, Name = "Tundra", Color = "ffffff"},
            new Terrain{Id = 17, Name = "Sewer", Color = "4e5f4d"},
            new Terrain{Id = 18, Name = "Underground Developed", Color = "442b1a"},
            new Terrain{Id = 19, Name = "Underground Natural", Color = "504136"},
            new Terrain{Id = 20, Name = "Magma", Color = "a21215"},
            new Terrain{Id = 21, Name = "Wall", Color = "2a2a2a"}
        };
        #endregion

        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
