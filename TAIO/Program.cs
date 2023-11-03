using System.Collections.Immutable;
using TAIO;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            // GraphGenerator.generateGraphsToFile(new[]{30, 30}, "graphs/data4.txt");
            ExactCliques(Graph.ParseInputFile("graphs/data2.txt"));
            // ExactSubgraphs(Graph.ParseInputFile("graphs/data4.txt"));
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception);
        }
    }

    private static void ExactSubgraphs(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() =>
            new SubgraphUsingClique().Solve(new Graph(new int[1, 1]), new Graph(new int[1, 1])));

        for (int i = 0; i < graphs.Count; i++)
        for (int j = i + 1; j < graphs.Count; j++)
        {
            var g1 = graphs[i];
            var g2 = graphs[j];
            (var subgraph, double time) =
                TimedUtils.Timed(() => new SubgraphUsingClique().Solve(g1, g2));
            Console.WriteLine(
                $"Found subgraph of size {subgraph.Count} with vertices mapping: {Helpers.ItemsToString(subgraph)} in {time} ms");
            Helpers.PrintHighlightedSubgraph(g1, subgraph, 0);
            Console.WriteLine();
            Helpers.PrintHighlightedSubgraph(g2, subgraph, 1);
            SolutionChecker.CheckSubgraph(g1, g2, subgraph);
            Console.WriteLine("===============================================");
        }
    }


    private static void ExactCliques(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        foreach (var g in graphs)
        {
            (var clique, double time) =
                TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(g));
            Helpers.EvaluateSolutionForCliqueProblem(g, clique, time);
        }
    }
}