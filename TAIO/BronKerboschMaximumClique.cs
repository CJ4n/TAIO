using System.Collections.Immutable;

namespace TAIO;

public class BronKerboschMaximumClique
{
    // https://eduinf.waw.pl/inf/alg/001_search/0143.php#:~:text=Klika%20maksymalna%20(ang.,clique)%20jest%20najwi%C4%99kszym%20podgrafem%20pe%C5%82nym.

    // Klika maksymalna
    private HashSet<int> rMax;
    public ImmutableSortedSet<int> Solve(Graph graph)
    {
        // Zbiór wierzchołków, które są kandydatami do rozważenia.
        var pSet = Enumerable.Range(0, graph.VerticesCount).ToHashSet();
        // Zbiór wierzchołków, które są częściowym wynikiem znajdowania kliki.
        var rSet = new HashSet<int>();
        // zbiór wierzchołków pominiętych.
        var xSet = new HashSet<int>();
        rMax = new HashSet<int>();
        BronKerbosch(graph, pSet, rSet, xSet);
        return rMax.ToImmutableSortedSet();
    }

    private void BronKerbosch(Graph graph, HashSet<int> pSet, HashSet<int> rSet, HashSet<int> xSet)
    {
        int ncmax;
        if (pSet.Count == 0 && xSet.Count == 0)
        {
            if (rSet.Count > rMax.Count)
                rMax = rSet;
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
            foreach (int w in graph.GetNeighboursBi(u))
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
        var vNeighs = graph.GetNeighboursBi(v);
        pBis.ExceptWith(vNeighs);
        foreach (int y in pBis)
        {
            var nSet = new HashSet<int>();
            nSet.UnionWith(graph.GetNeighboursBi(y));
            var rPrim = new HashSet<int>();
            rPrim.UnionWith(rSet);
            rPrim.Add(y);
            var pPrim = new HashSet<int>(pSet);
            pPrim.IntersectWith(nSet);
            var xPrim = new HashSet<int>(xSet);
            xPrim.IntersectWith(nSet);
            BronKerbosch(graph, pPrim, rPrim, xPrim);
            pSet.Remove(y);
            xSet.Add(y);
        }
    }
}
