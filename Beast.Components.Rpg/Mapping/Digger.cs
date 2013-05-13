using System;
using System.Collections.Generic;

namespace Beast.Mapping
{
    internal class Digger : ISearchContext<Unit>
    {
        private IDictionary<Unit, Place> _map;
        private MersenneTwister _rnd;
        private int _mapWidth;
        private int _mapHeight;

        public Digger(IDictionary<Unit, Place> map, int mapWidth, int mapHeight, MersenneTwister rnd)
        {
            _map = map;
            _rnd =rnd;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public Stack<Unit> GetPath(Unit start, Unit end)
        {
            var search = new AStarSearch<Unit>(this);
            return search.GetPath(start, end, _mapWidth * _mapHeight);
        }

        public IEnumerable<Unit> GetNodeGraph(Unit node)
        {
            var list = new List<Unit>();
            var directions = new List<Direction>();
            foreach (var direction in Direction.All)
            {
                if (direction.Value == KnownDirection.Up || direction.Value == KnownDirection.Down)
                    continue;

                var pos = node + direction.Unit;
                if (!IsInBounds(pos))
                    continue;

                list.Add(pos);
            }
            return list;
        }

        private bool IsInBounds(Unit unit)
        {
            return (unit.X >= 0 && unit.X <= (_mapWidth - 1))
                && (unit.Y >= 0 && unit.Y <= (_mapHeight - 1));
        }

        public int CalculateKnownCost(Unit fromNode, Unit neighbor)
        {
            var direction = Direction.FromPoints(fromNode, neighbor);
            var counter = direction.Counter();
            var prevNode = fromNode + counter.Unit;

            var result = 0;

            if (_map.ContainsKey(prevNode))
                result += -5;

            var prevDirection = Direction.FromPoints(prevNode, fromNode);

            result += prevDirection.Value == direction.Value ? 1 : -1;

            // Random
            var positive = _rnd.Next(1, 2);
            result += positive == 1 ? _rnd.Next(1, 3) : -_rnd.Next(1, 3);

            return fromNode.DistanceTo(neighbor) + result;
        }

        public int Heuristic(Unit node, Unit target)
        {
            return Math.Abs(node.X - target.X) + Math.Abs(node.Y - target.Y);
        }
    }
}
