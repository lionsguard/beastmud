using System.Collections.Generic;

namespace Beast
{
    public class AStarSearch<TNode>
    {
        private readonly ISearchContext<TNode> _context;

        public AStarSearch(ISearchContext<TNode> context)
        {
            _context = context;
        }

        private class PathNode
        {
            public PathNode(TNode node)
            {
                Value = node;
            }
            public TNode Value { get; private set; }
            public PathNode Parent { get; set; }
            public int CostG { get; set; }
            public int CostH { get; set; }
            public int TotalCost
            {
                get { return CostG + CostH; }
            }
        }

        private static Stack<TNode> GetPathFromNode(PathNode node)
        {
            if (node == null) return null;
            var stack = new Stack<TNode>();
            stack.Push(node.Value);
            while (node.Parent != null)
            {
                stack.Push(node.Parent.Value);
                node = node.Parent;
            }
            return stack;
        }

        private class PathNodeList: List<PathNode>
        {
            public bool AlreadyContainsNode(TNode node)
            {
                foreach (var pathNode in this)
                {
                    if (pathNode.Value.Equals(node))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Stack<TNode> GetPath(TNode start, TNode end, int maxSteps)
        {
            var openList = new PathNodeList();
            var closedList = new PathNodeList();

            var bestNode = new PathNode(start)
            {
                CostH = _context.Heuristic(start, end)
            };

            openList.Add(bestNode);
            int step = 0;
            while (openList.Count > 0)
            {
                bestNode = openList[0];
                step++;
                if (step > maxSteps)
                {
                    return GetPathFromNode(bestNode);
                }
                openList.Remove(bestNode);
                closedList.Add(bestNode);

                var neighbors = _context.GetNodeGraph(bestNode.Value);

                foreach (var node in neighbors)
                {
                    var neighbor = node;
                    
                    if (!closedList.AlreadyContainsNode(neighbor))
                    {
                        var newNode = new PathNode(neighbor)
                        {
                            Parent = bestNode,
                            CostG =
                                bestNode.CostG +
                                _context.CalculateKnownCost(bestNode.Value, neighbor)
                        };


                        if (openList.AlreadyContainsNode(neighbor))
                        {
                            var oldNode = openList.Find(n => n.Value.Equals(neighbor));

                            if (newNode.CostG < oldNode.CostG)
                            {
                                oldNode.Parent = bestNode;
                                openList.Sort((a, b) => a.TotalCost - b.TotalCost);
                            }
                        }
                        else
                        {
                            newNode.CostH = _context.Heuristic(newNode.Value, end);
                            openList.Add(newNode);
                            openList.Sort((a, b) => a.TotalCost - b.TotalCost);

                            if (neighbor.Equals(end))
                            {
                                return GetPathFromNode(newNode);
                            }
                        }
                    }
                }
            }
            return GetPathFromNode(bestNode);
        }
    }       
}
