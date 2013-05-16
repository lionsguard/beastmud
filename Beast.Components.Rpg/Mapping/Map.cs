using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
    public class Map : ICollection<Place>
    {
        private readonly Dictionary<Unit, Place> _places = new Dictionary<Unit, Place>();

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        public Range<int> Depth
        {
            get 
            {
                var min = this.Min(p => p.Location.Z);
                var max = this.Max(p => p.Location.Z);

                return new Range<int>(min, max);
            }
        }

        public List<Terrain> Terrain { get; set; }

        public Place Start { get; set; }

        public Place this[Unit unit]
        {
            get 
            {
                Place place;
                _places.TryGetValue(unit, out place);
                return place; 
            }
            set { _places[unit] = value; }
        }

        public Map()
        {
            Terrain = new List<Terrain>(Beast.Mapping.Terrain.DefaultTerrain);
        }

        public void Add(Place place)
        {
            this[place.Location] = place;
        }

        public void AddRange(IEnumerable<Place> places)
        {
            foreach (var place in places)
            {
                Add(place);
            }
        }

        public bool Contains(Unit unit)
        {
            return _places.ContainsKey(unit);
        }

        public void Clear()
        {
            _places.Clear();
        }

        public bool Contains(Place item)
        {
            return _places.ContainsKey(item.Location);
        }

        public void CopyTo(Place[] array, int arrayIndex)
        {
            _places.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _places.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Place item)
        {
            return _places.Remove(item.Location);
        }

        public IEnumerator<Place> GetEnumerator()
        {
            return _places.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsInBounds(Unit unit)
        {
            return (unit.X >= 0 && unit.X <= (Width - 1))
                && (unit.Y >= 0 && unit.Y <= (Height - 1));
        }

        public MapLevel GetLevel(int z)
        {
            return new MapLevel(Width, Height, z, this.Where(p => p.Location.Z == z).ToDictionary(p => p.Location, p => p));
        }

        /// <summary>
        /// Generates places for all MapStart locations.
        /// </summary>
        public void Generate()
        {
            var mapStarts = _places.Values.Where(p => p is MapStart).Select(p => p as MapStart);

            var mapper = new Mapper();
            foreach (var start in mapStarts)
            {
                AddRange(mapper.Generate(start));
            }
        }
    }
}
