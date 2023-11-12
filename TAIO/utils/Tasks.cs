using TAIO;


public static class Tasks
{
    public static void ApproximatedCliques(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new GeneticAlgorithm().Solve(new Graph(new int[1, 1])));

        foreach (var g in graphs)
        {
            ((var clique, var L), double time) =
                TimedUtils.Timed(() => new GeneticAlgorithm().Solve(g));
            Helpers.EvaluateSolutionForCliqueProblem(g, clique, L, time);
        }
    }

     public static void ExactCliques(List<Graph> graphs)
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

    public static void ApproximatedSubgraphs(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        for (int i = 0; i < graphs.Count; i+=2)
        {
            (var subgraph, double time) =
            TimedUtils.Timed(() => new SubgraphUsingClique(new GeneticAlgorithm()).Solve(graphs[i], graphs[i+1]));
            Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, graphs[i], graphs[i+1]);
        }
    
    }

    public static void ExactSubgraphs(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() =>
            new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(new Graph(new int[1, 1]),
                new Graph(new int[1, 1])));

        for (int i = 0; i < graphs.Count; i+=2)
        {
            (var subgraph, double time) =
                TimedUtils.Timed(() =>
                    new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(graphs[i], graphs[i+1]));
                Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, graphs[i], graphs[i+1]);
        }
    }

    public static void Size(List<Graph> graphs) 
    {
        for (int i = 0; i < graphs.Count; i++) 
        {
            Console.WriteLine($"Graph #{i} has size of {graphs[i].Size()}");
        }
    }

    public static void ExactMatrics(List<Graph> graphs) 
    {
        for (int i = 0; i < graphs.Count; i+=2)
        {
            Console.WriteLine($"Graphs #{i} and #{i+1} are {Graph.ExactDistance(graphs[i], graphs[i+1])} units away");
        }
    }

    public static void ApproximateMetrics(List<Graph> graphs)
    {
        for (int i = 0; i < graphs.Count; i+=2)
        {
            Console.WriteLine($"Graphs #{i} and #{i+1} are {Graph.ApproxDistance(graphs[i], graphs[i+1])} units away");
        }
    }
}