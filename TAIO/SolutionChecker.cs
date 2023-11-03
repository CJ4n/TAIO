using System.Collections.Immutable;

namespace TAIO;

public class SolutionChecker
{
    private const string SUCCESS = "Solution check passed successfully!";


    public static bool CheckClique(Graph graph, ImmutableSortedSet<int> clique)
    {
        Console.WriteLine(
            $"Checking clique of size {clique.Count} in graph of size {graph.VerticesCount}.");
        int missingEdges = 0;
        for (int i = 0; i < clique.Count; i++)
        {
            for (int j = 0; j < clique.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (graph.GetAt(clique[i], clique[j]) == 1)
                {
                    continue;
                }

                missingEdges++;
                Console.WriteLine($"Missing edge between {clique[i]} and {clique[j]}");
            }
        }

        Console.WriteLine(missingEdges != 0 ? $"Missing {missingEdges} edges total." : SUCCESS);
        
        return missingEdges == 0;
    }


    public static void CheckSubgraph(Graph graph1, Graph graph2,
        ImmutableSortedSet<(int, int)> mapping)
    {
        int differentEdges = 0;
        for (int i = 0; i < mapping.Count; i++)
        for (int j = i + 1; j < mapping.Count; j++)
            if (graph1.GetAt(mapping[i].Item1, mapping[j].Item1) !=
                graph2.GetAt(mapping[i].Item2, mapping[j].Item2))
            {
                differentEdges++;
                Console.WriteLine(
                    $"{mapping[i].Item1} & {mapping[j].Item1} edge in G1 differs from {mapping[i].Item2} & {mapping[j].Item2} edge in G2");
            }

        Console.WriteLine(differentEdges != 0
            ? $"Differences in {differentEdges} edges total."
            : SUCCESS);
        
    }
}