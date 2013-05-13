using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Mapping
{
    public class Mapper
    {
        private MersenneTwister _rnd;
        private MapStart _mapStart;

        public IEnumerable<Place> Generate(MapStart mapStart)
        {
            _mapStart = mapStart;
            _rnd = new MersenneTwister(_mapStart.Seed);

            var places = new Dictionary<Unit, Place>();
            var depth = Math.Max(_mapStart.Depth, 1);
            var z = _mapStart.Location.Z;

            Place start = _mapStart;
            Place end = null;
            while (depth > 0)
            {
                if (start == null)
                {
                    CreateStartPosition(z, end, out start);
                    places.Add(start.Location, start);
                }

                // If end exist, create an exit between end and the current start
                if (end != null)
                {
                    var endDir = _mapStart.ZIndexUp ? Direction.Up : Direction.Down;
                    end.Exits.Add(endDir);
                    start.Exits.Add(endDir.Counter());
                }

                end = CreateEndPosition(start);
                places.Add(end.Location, end);

                Build(start, end, places);

                start = null;

                if (mapStart.ZIndexUp)
                    z++;
                else
                    z--;

                depth--;
            }

            return places.Values;
        }

        private void Build(Place start, Place end, IDictionary<Unit, Place> map)
        {
            var digger = new Digger(map, _mapStart.Width, _mapStart.Height, _rnd);

            var path = digger.GetPath(start.Location, end.Location);
            var starts = new List<Unit>();

            Place prev = null;
            while (path.Count > 0)
            {
                var node = path.Pop();

                AddPlace(node, map, ref prev);

                // Randomly choose some of these nodes as an additional starting point.
                if (_rnd.Next(0, 100) >= 50)
                {
                    starts.Add(node);
                }
            }

            // Get a few random end points
            var ends = FindRandomEndPoints(start.Location.Z, starts.Count).ToList();

            // Randomly pick a start and end pair.
            for (int i = 0; i < starts.Count; i++)
            {
                var index = _rnd.Next(0, starts.Count - 1);

                path = digger.GetPath(starts[i], ends[i]);

                prev = null;
                while (path.Count > 0)
                {
                    var node = path.Pop();
                    AddPlace(node, map, ref prev);
                }

                starts.RemoveAt(i);
                ends.RemoveAt(i);
            }
        }

        private void AddPlace(Unit node, IDictionary<Unit, Place> map, ref Place prev)
        {
            Place place;
            if (!map.TryGetValue(node, out place))
            {
                place = new Place { Location = node };
                map.Add(node, place);
            }

            if (prev != null)
            {
                var dir = Direction.FromPoints(prev.Location, node);
                prev.Exits.Add(dir);
                place.Exits.Add(dir.Counter());
            }
            prev = place;
        }

        private IEnumerable<Unit> FindRandomEndPoints(int z, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Unit(_rnd.Next(0, _mapStart.Width - 1), _rnd.Next(0, _mapStart.Height - 1), z);
            }
        }
       
        private bool IsInBounds(Unit unit, int z)
        {
            return (unit.X >= 0 && unit.X <= (_mapStart.Width - 1))
                && (unit.Y >= 0 && unit.Y <= (_mapStart.Height - 1))
                && unit.Z == z;
        }

        private Stack<Direction> GetRandomDirections()
        {
            return new Stack<Direction>(Direction.All.ToList().Shuffle());
        }

        private void CreateStartPosition(int z, Place previousEnd, out Place start)
        {
            if (previousEnd != null)
            {
                start = new Place { Location = new Unit(previousEnd.Location.X, previousEnd.Location.Y, z) };
                return;
            }

            var startEdge = _rnd.Next(0, 3);
            start = new Place { Location = CreateUnitFromEgde(startEdge, z) };
        }

        private Place CreateEndPosition(Place start)
        {
            var startEdge = FindEdge(start.Location);
            var dest = Unit.Empty;
            switch (startEdge)
            {
                case 0: // north
                    dest = CreateUnitFromEgde(1, start.Location.Z);
                    break;
                case 1: // south
                    dest = CreateUnitFromEgde(0, start.Location.Z);
                    break;
                case 2: // east
                    dest = CreateUnitFromEgde(3, start.Location.Z);
                    break;
                case 3: // west
                    dest = CreateUnitFromEgde(2, start.Location.Z);
                    break;
            }
            return new Place { Location = new Unit(dest.X, dest.Y, start.Location.Z) };
        }

        private int FindEdge(Unit unit)
        {
            if (unit.X == 0)
                return 3; // west
            if (unit.X == (_mapStart.Width - 1))
                return 2; // east
            if (unit.Y == 0)
                return 0; // north
            return 1; //south
        }

        private Unit CreateUnitFromEgde(int edge, int z)
        {
            var x = 0;
            var y = 0;
            switch (edge)
            {
                case 0:
                    // North edge
                    x = _rnd.Next(0, _mapStart.Width - 1);
                    break;
                case 1:
                    // South edge
                    y = _mapStart.Height - 1;
                    x = _rnd.Next(0, _mapStart.Width - 1);
                    break;
                case 2:
                    // East edge
                    x = _mapStart.Width - 1;
                    y = _rnd.Next(0, _mapStart.Height - 1);
                    break;
                case 3:
                    // West edge
                    y = _rnd.Next(0, _mapStart.Height - 1);
                    break;
            }
            return new Unit(x, y, z);
        }
    }
}
