using System.Collections.Generic;

namespace Beast.Mapping
{
    public class MapLevel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Z { get; set; }

        public List<MapTile> Tiles { get; set; }

        public MapLevel(int width, int height, int z, IDictionary<Unit, Place> places)
        {
            Width = width;
            Height = height;
            Z = z;

            Tiles = new List<MapTile>(places.Count);

            foreach (var kvp in places)
            {
                var tile =new MapTile
                {
                    X = kvp.Key.X,
                    Y = kvp.Key.Y,
                    Z = kvp.Key.Z,
                    Terrain = kvp.Value.Terrain
                };
                tile.Exits = new int[kvp.Value.Exits.Count];
                var index = 0;
                foreach (var item in kvp.Value.Exits)
                {
                    if (item.HasExit)
                    {
                        tile.Exits[index] = (int)item.Direction;
                        index++;
                    }
                }
                Tiles.Add(tile);
            }
        }
    }
}
