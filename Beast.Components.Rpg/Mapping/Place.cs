
namespace Beast.Mapping
{
    public class Place : GameObject, IPlace
    {
        public Unit Location { get; set; }
        public ExitCollection Exits { get; set; }
        public int Terrain { get; set; }    

        public Place()
        {
            Location = Unit.Empty;
            Exits = new ExitCollection();
        }

        public bool HasExit(KnownDirection direction)
        {
            return Exits.HasExit(direction);
        }
    }
}
