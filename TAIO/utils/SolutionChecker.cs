using System.Collections.Immutable;

namespace TAIO;

public class SolutionChecker
{
    private const string SUCCESS = "Solution check passed successfully!";


    public static bool CheckClique(Graph graph, ImmutableSortedSet<int> clique, int L,
        bool verbose = false)
    {
        int missingEdges = 0;
        for (int i = 0; i < clique.Count; i++)
        {
            for (int j = i+1; j < clique.Count; j++)
            {

                if (int.Min(graph.GetAt(clique[i], clique[j]), graph.GetAt(clique[j], clique[i])) > 0)
                {
                    continue;
                }

                missingEdges++;
                if (verbose)
                    Console.WriteLine(
                        $"Missing edge between {clique[i]} and {clique[j]} with thickness {L}.");
            }
        }

        if (verbose)
            Console.WriteLine(missingEdges != 0 ? $"Missing {missingEdges} edges total." : SUCCESS);

        return missingEdges == 0;
    }


    public static bool CheckSubgraph(Graph graph1, Graph graph2,
        ImmutableSortedSet<(int, int)> mapping, bool verbose = false)
    {
        int differentEdges = 0;
        for (int i = 0; i < mapping.Count; i++)
        for (int j = 0; j < mapping.Count; j++)
        {
            if(i == j)
                continue;
            if ((graph1.GetAt(mapping[i].Item1, mapping[j].Item1) > 0 && graph2.GetAt(mapping[i].Item2, mapping[j].Item2) == 0) ||
                (graph1.GetAt(mapping[i].Item1, mapping[j].Item1) == 0 && graph2.GetAt(mapping[i].Item2, mapping[j].Item2) > 0))
            {
                differentEdges++;
                if (verbose)
                    Console.WriteLine(
                        $"{mapping[i].Item1} & {mapping[j].Item1} edge in G1 differs from {mapping[i].Item2} & {mapping[j].Item2} edge in G2");
            }

        }
            
        if (verbose)
            Console.WriteLine(differentEdges != 0
                ? $"Differences in {differentEdges} edges total."
                : SUCCESS);

        return differentEdges == 0;
    }

    public class WrongSolutionException : Exception
    {
    }
}
