using System.Collections.Generic;

namespace Beast
{
    public interface ISearchContext<TNode>
    {
        IEnumerable<TNode> GetNodeGraph(TNode node);
        int CalculateKnownCost(TNode fromNode, TNode neighbor);
        int Heuristic(TNode node, TNode target);
    }
}
