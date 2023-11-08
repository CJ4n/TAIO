﻿using TAIO;
using TAIO_tests;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Benchmark.RunFullApproximationBenchmark();
            // Benchmark.RunFullCliqueBenchmark();
            // Benchmark.RunFullSubgraphBenchmark();
            // GraphGenerator.generateGraphsToFile(new[]{30, 30}, "graphs/data4.txt");
            // ExactCliques(Graph.ParseInputFile("graphs/data2.txt"));
            // ExactSubgraphs(Graph.ParseInputFile("graphs/data4.txt"));
            // ApproximatedCliques();
        }
        catch (NotImplementedException exception)
        {
            Console.WriteLine(exception);
        }
    }

    private static void ApproximatedCliques()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        Graph g = Graph.GetRandomGraphWithCliques(new List<int> { 100, 101, 104}, 1000, 0.7f);
        (var clique, double time) =
            TimedUtils.Timed(() => new GeneticAlgorithm().Solve(g));
        Helpers.EvaluateSolutionForCliqueProblem(g, clique, time);
        
    }

    private static void ExactSubgraphs(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() =>
            new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(new Graph(new int[1, 1]),
                new Graph(new int[1, 1])));

        for (int i = 0; i < graphs.Count; i++)
        for (int j = i + 1; j < graphs.Count; j++)
        {
            var g1 = graphs[i];
            var g2 = graphs[j];
            (var subgraph, double time) =
                TimedUtils.Timed(() =>
                    new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(g1, g2));
            Console.WriteLine(
                $"Found subgraph of size {subgraph.Count} with vertices mapping: {Helpers.ItemsToString(subgraph)} in {time} ms");
            Helpers.PrintHighlightedSubgraph(g1, subgraph, 0);
            Console.WriteLine();
            Helpers.PrintHighlightedSubgraph(g2, subgraph, 1);
            SolutionChecker.CheckSubgraph(g1, g2, subgraph, true);
            Console.WriteLine("===============================================");
        }
    }


    private static void ExactCliques(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        foreach (var g in graphs)
        {
            ((var clique, var L), double time) =
                TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(g));
            Helpers.EvaluateSolutionForCliqueProblem(g, clique, L, time);
        }
    }
}
