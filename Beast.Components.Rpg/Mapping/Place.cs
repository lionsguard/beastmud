
namespace Beast.Mapping
{
    [System.Diagnostics.DebuggerDisplay("{Location} : [{Terrain}] - {Name}")]
    public class Place : GameObject, IPlace
    {
        public Unit Location { get; set; }
        public ExitCollection Exits { get; set; }
        public int Terrain { get; set; }
        public int Flags { get; set; }

        public Place()
        {
            Location = Unit.Empty;
            Exits = new ExitCollection();
        }

        public bool HasExit(KnownDirection direction)
        {
            return Exits.HasExit(direction);
        }

        public bool HasFlag(PlaceFlag flag)
        {
            return HasFlag(flag.Value);
        }

        public virtual bool HasFlag(int flag)
        {
            return (Flags & flag) == flag;
        }
    }
}
