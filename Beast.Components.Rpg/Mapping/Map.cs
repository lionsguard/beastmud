using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
    public class Map : ICollection<IPlace>
    {
        private readonly Dictionary<Unit, IPlace> _places = new Dictionary<Unit, IPlace>();

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

        public IPlace Start { get; set; }

        public IPlace this[Unit unit]
        {
            get 
            {
                IPlace place;
                _places.TryGetValue(unit, out place);
                return place; 
            }
            set { _places[unit] = value; }
        }

        public void Add(IPlace place)
        {
            this[place.Location] = place;
        }

        public void AddRange(IEnumerable<IPlace> places)
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

        public bool Contains(IPlace item)
        {
            return _places.ContainsKey(item.Location);
        }

        public void CopyTo(IPlace[] array, int arrayIndex)
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

        public bool Remove(IPlace item)
        {
            return _places.Remove(item.Location);
        }

        public IEnumerator<IPlace> GetEnumerator()
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
            IPlace end;
            Generate(out end);
        }

        /// <summary>
        /// Generates places for all MapStart locations.
        /// </summary>
        /// <param name="mapEnd">The end point of the last level of the map.</param>
        public void Generate(out IPlace mapEnd)
        {
            mapEnd = null;
            var mapStarts = _places.Values.Where(p => p is MapStart).Select(p => p as MapStart).ToArray();

            var mapper = new Mapper();
            foreach (var start in mapStarts)
            {
                AddRange(mapper.Generate(start, out mapEnd));
            }
        }
    }
}
