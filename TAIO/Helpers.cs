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
        double time)
    {
        Console.WriteLine(
            $"Found clique of size {clique.Count} with vertices: {Helpers.ItemsToString(clique)} in {time} ms");
        Helpers.PrintHighlightedClique(g, clique);
        bool isClique = SolutionChecker.CheckClique(g, clique);
        Console.WriteLine("===============================================");
        return isClique;
    }
}