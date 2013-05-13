
namespace Beast.Mapping
{
    public class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Terrain { get; set; }
        public int[] Exits { get; set; }    
    }
}
