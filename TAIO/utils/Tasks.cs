using TAIO;


public static class Tasks
{
    public static void ApproximatedCliques(List<Graph> graphs)
    {
        Warmup();
        foreach (var g in graphs)
        {
            ((var clique, var L), double time) =
                TimedUtils.Timed(() => new GeneticAlgorithm().Solve(g));
            Helpers.EvaluateSolutionForCliqueProblem(g, clique, L, time);
        }
    }

     public static void ExactCliques(List<Graph> graphs)
    {
        Warmup();
        foreach (var g in graphs)
        {
            ((var clique, var L), double time) =
                TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(g));
            Helpers.EvaluateSolutionForCliqueProblem(g, clique, L, time);
        }
    }

    public static void ApproximatedSubgraphs(List<Graph> graphs1, List<Graph> graphs2)
    {
        Warmup();
        for (int i = 0; i < graphs1.Count && i < graphs2.Count;i++)
        {
            (var subgraph, double time) =
            TimedUtils.Timed(() => new SubgraphUsingClique(new GeneticAlgorithm()).Solve(graphs1[i], graphs2[i]));
            Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, graphs1[i], graphs2[i]);
        }
    }

    public static void ExactSubgraphs(List<Graph> graphs1, List<Graph> graphs2)
    {
        Warmup();
        for (int i = 0; i < graphs1.Count && i < graphs2.Count; i++)
        {
            (var subgraph, double time) =
                TimedUtils.Timed(() =>
                    new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(graphs1[i], graphs2[i]));
                Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, graphs1[i], graphs2[i]);
        }
    }

    public static void Size(List<Graph> graphs) 
    {
        for (int i = 0; i < graphs.Count; i++) 
        {
            Console.WriteLine($"Graph #{i} has size of {graphs[i].Size()}");
        }
    }

    public static void ExactMatrics(List<Graph> graphs1, List<Graph> graphs2) 
    {
        Warmup();
        for (int i = 0; i < graphs1.Count && i < graphs2.Count;i++)
        {
            (var distance, var time) =TimedUtils.Timed(() => Graph.ExactDistance(graphs1[i], graphs2[i]));
            Console.WriteLine(graphs1[i]);
            Console.WriteLine(graphs2[i]);
            Console.WriteLine($"Graphs #{i} and #{i+1} are {distance} units away ({time} ms)");
            Console.WriteLine("===============================================");
        }
    }

    public static void ApproximateMetrics(List<Graph> graphs1, List<Graph> graphs2)
    {
        Warmup();
        for (int i = 0; i < graphs1.Count && i < graphs2.Count;i++)
        {
            (var distance, var time) =TimedUtils.Timed(() => Graph.ApproxDistance(graphs1[i], graphs2[i]));
            Console.WriteLine(graphs1[i]);
            Console.WriteLine(graphs2[i]);
            Console.WriteLine($"Graphs #{i} and #{i+1} are {distance} units away ({time} ms)");
            Console.WriteLine("===============================================");
        }
    }

    private static void Warmup()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));
    }
}
