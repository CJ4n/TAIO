using System.Collections.Immutable;

namespace TAIO;

public class BronKerboschMaximumClique : ICliqueAlgorithm
{
    // https://eduinf.waw.pl/inf/alg/001_search/0143.php#:~:text=Klika%20maksymalna%20(ang.,clique)%20jest%20najwi%C4%99kszym%20podgrafem%20pe%C5%82nym.

    private Graph _graph;

    // Klika największa
    private HashSet<int> _rMax;
    private int _maxEdge;

    public (ImmutableSortedSet<int>, int) Solve(Graph graph)
    {
        _graph = graph;
        // Zbiór wierzchołków, które są kandydatami do rozważenia.
        // W każdej chwili w zbiorze P znajdują się wierzchołki, z których każdy z osobna
        // jest w stanie zwiększyć klikę w zbiorze R.
        var pSet = Enumerable.Range(0, graph.VerticesCount).ToHashSet();
        // Zbiór wierzchołków, które są częściowym wynikiem znajdowania kliki.
        var rSet = new HashSet<int>();
        // zbiór wierzchołków pominiętych.
        var xSet = new HashSet<int>();
        _rMax = new HashSet<int>();
        _maxEdge = 0;
        BronKerbosch(pSet, rSet, xSet, 0);
        return (_rMax.ToImmutableSortedSet(), _maxEdge);
    }

    private void BronKerbosch(HashSet<int> pSet, HashSet<int> rSet, HashSet<int> xSet, int edgeCount)
    {
        int ncmax, edges;
        if (pSet.Count == 0 && xSet.Count == 0)
        {
            if (rSet.Count > _rMax.Count || (rSet.Count == _rMax.Count && edgeCount >= _maxEdge))
            {
                _rMax = rSet;
                _maxEdge = edgeCount;
            }
            return;
        }

        var pBis = new HashSet<int>();
        pBis.UnionWith(pSet);
        pBis.UnionWith(xSet);
        ncmax = 0;
        int v = pBis.First();

        foreach (int u in pBis)
        {
            int nc = 0;
            foreach (int w in _graph.GetNeighboursBi(u))
                if (pSet.Contains(w))
                    nc++;
            if (nc >= ncmax)
            {
                v = u;
                ncmax = nc;
            }
        }

        pBis = new HashSet<int>();
        pBis.UnionWith(pSet);
        var vNeighs = _graph.GetNeighboursBi(v);
        pBis.ExceptWith(vNeighs);
        foreach (int y in pBis)
        {
            int yEdges = rSet.Count != 0? rSet.Sum(r=>_graph.GetAtBidirectional(r, y)): 0;
            edges = edgeCount + yEdges;
            var nSet = new HashSet<int>();
            nSet.UnionWith(_graph.GetNeighboursBi(y));
            var rPrim = new HashSet<int>();
            rPrim.UnionWith(rSet);
            rPrim.Add(y);
            var pPrim = new HashSet<int>(pSet);
            pPrim.IntersectWith(nSet);
            var xPrim = new HashSet<int>(xSet);
            xPrim.IntersectWith(nSet);
            BronKerbosch(pPrim, rPrim, xPrim, edges);
            pSet.Remove(y);
            xSet.Add(y);
        }
    }
}
