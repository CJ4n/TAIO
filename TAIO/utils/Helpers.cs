using System.Collections.Immutable;
using System.Text;

namespace TAIO;

public static class Helpers
{
    public static String ItemsToString<T>(IEnumerable<T> es)
    {
        var sb = new StringBuilder();
        sb.Append('[');
        foreach (var e in es)
        {
            sb.Append(e + " ");
        }

        sb.Append(']');
        return sb.ToString();
    }

    public static void PrintHighlightedSubgraph(Graph g, ImmutableSortedSet<(int, int)> subgraph,
        int index)
    {
        bool[,] highlight = new bool[g.VerticesCount, g.VerticesCount];
        foreach (var p in subgraph)
        foreach (var q in subgraph)
            if (index == 0) highlight[p.Item1, q.Item1] = true;
            else highlight[p.Item2, q.Item2] = true;
        g.PrintHighlighted(highlight);
    }

    public static void PrintHighlightedClique(Graph g, ImmutableSortedSet<int> clique)
    {
        bool[,] highlight = new bool[g.VerticesCount, g.VerticesCount];
        foreach (var p in clique)
        foreach (var q in clique)
            highlight[p, q] = true;
        g.PrintHighlighted(highlight);
    }


    public static bool EvaluateSolutionForCliqueProblem(Graph g, ImmutableSortedSet<int> clique,
        int L,
        double time)
    {
        PrintHighlightedClique(g, clique);
        Console.WriteLine(
            $"Found clique of size {clique.Count} with {L} bidirectional edges with vertices: {ItemsToString(clique)} in {time} ms");

        bool isClique = SolutionChecker.CheckClique(g, clique,L, true);
        Console.WriteLine("===============================================");
        return isClique;
    }

    public static void EvaluateSolutionForSubgraphProblem((ImmutableSortedSet<(int, int)>, int) subgraph, double time, Graph g1, Graph g2)
    {
       
        PrintHighlightedSubgraph(g1, subgraph.Item1, 0);
        Console.WriteLine();
        PrintHighlightedSubgraph(g2, subgraph.Item1, 1);
        Console.WriteLine(
            $"Found subgraph of size (V:{subgraph.Item1.Count}, E:{subgraph.Item2}) with vertices mapping: {ItemsToString(subgraph.Item1)} in {time} ms");
        SolutionChecker.CheckSubgraph(g1, g2, subgraph.Item1, true);
        Console.WriteLine("===============================================");
    }
}
