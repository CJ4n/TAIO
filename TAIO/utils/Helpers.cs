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
        Console.WriteLine(
            $"Found clique of size {clique.Count} of thickness {L} with vertices: {Helpers.ItemsToString(clique)} in {time} ms");
        if (g.VerticesCount <= 100)
        {
            Helpers.PrintHighlightedClique(g, clique);
        }

        bool isClique = SolutionChecker.CheckClique(g, clique,L);
        Console.WriteLine("===============================================");
        return isClique;
    }

    public static void EvaluateSolutionForSubgraphProblem(ImmutableSortedSet<(int, int)> subgraph, double time, Graph g1, Graph g2)
    {
        Console.WriteLine(
            $"Found subgraph of size {subgraph.Count} with vertices mapping: {Helpers.ItemsToString(subgraph)} in {time} ms");
        Helpers.PrintHighlightedSubgraph(g1, subgraph, 0);
        Console.WriteLine();
        Helpers.PrintHighlightedSubgraph(g2, subgraph, 1);
        SolutionChecker.CheckSubgraph(g1, g2, subgraph, true);
        Console.WriteLine("===============================================");
    }
}
