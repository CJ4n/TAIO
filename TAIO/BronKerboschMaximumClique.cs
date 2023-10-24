namespace TAIO;

public class BronKerboschMaximumClique
{
    // https://eduinf.waw.pl/inf/alg/001_search/0143.php#:~:text=Klika%20maksymalna%20(ang.,clique)%20jest%20najwi%C4%99kszym%20podgrafem%20pe%C5%82nym.

    private HashSet<int> rMax;
    public HashSet<int> solve(Graph graph)
    {
        var pSet = Enumerable.Range(0, graph.Size()).ToHashSet();
        var rSet = new HashSet<int>();
        var xSet = new HashSet<int>();
        rMax = new HashSet<int>();

        return bronKerbosch(graph, pSet, rSet, xSet);
    }

    private HashSet<int> bronKerbosch(Graph graph, HashSet<int> pSet, HashSet<int> rSet, HashSet<int> xSet)
    {
        int ncmax = 0;
        while (pSet.Count != 0 || xSet.Count != 0)
        {
            var pBis = new HashSet<int>();
            pBis.UnionWith(pSet);
            pBis.UnionWith(xSet);
            ncmax = 0;
            int v = 0; // Check if good init.
            
            foreach (int u in pBis)
            {
                int nc = 0;
                foreach (int w in graph.getNeighboursBi(u))
                    if (pSet.Contains(w))
                        nc++;
                if (nc > ncmax)
                {
                    v = u;
                    ncmax = nc;
                }

                pBis = new HashSet<int>();
                var vNeighs = graph.getNeighboursBi(v);
                pBis.UnionWith(pSet);
                pBis.ExceptWith(vNeighs);
                foreach (int y in pBis)
                {
                    var nSet = new HashSet<int>();
                    nSet.UnionWith(graph.getNeighboursBi(y));
                    var rPrim = new HashSet<int>();
                    rPrim.UnionWith(rSet);
                    rPrim.Add(y);
                    var pPrim = new HashSet<int>(pSet);
                    pPrim.IntersectWith(nSet);
                    var xPrim = new HashSet<int>(xSet);
                    xPrim.IntersectWith(nSet);
                    Helpers.PrintItems(pPrim);
                    Helpers.PrintItems(rPrim);
                    Helpers.PrintItems(xPrim);
                    bronKerbosch(graph, pPrim, rPrim, xPrim);
                }
            }
        }
        if (rSet.Count > rMax.Count)
            rMax = rSet;
        return rMax;
    }
}
