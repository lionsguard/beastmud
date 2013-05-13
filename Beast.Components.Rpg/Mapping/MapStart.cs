
namespace Beast.Mapping
{
    public class MapStart : Place
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public int Seed { get; set; }
        public bool ZIndexUp { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        public static MapStart FromPlace(Place place)
        {
            return new MapStart
            {
                Location = place.Location,
                Exits = place.Exits,
                Terrain = place.Terrain
            };
        }
    }
}
