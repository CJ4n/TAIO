using TAIO;
using System.CommandLine;

internal class Program
{
static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("This is a simple program for graph property exploration");

        rootCommand.AddCommand(getCliqueCommand());
        rootCommand.AddCommand(getSubgraphCommand());
        return await rootCommand.InvokeAsync(args);
    }

    private static void ApproximatedCliques(List<Graph> graphs)
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
    private static void ApproximatedSubgraphs()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        Graph g1 = Graph.GetRandomGraph(50, 0.9f);
        Graph g2 = Graph.GetRandomGraph(50, 0.9f);
        (var subgraph, double time) =
            TimedUtils.Timed(() => new SubgraphUsingClique(new GeneticAlgorithm()).Solve(g1, g2));
            Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, g1, g2);
        
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
                Helpers.EvaluateSolutionForSubgraphProblem(subgraph, time, g1, g2);
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

    private static Option<bool> getAlgorithmType() {
        return new Option<bool> (
            name: "isExact",
            description: "Use approximated algorithms",
            getDefaultValue: () => false);
        
    }

    private static Argument<string> getPathArgument() {
        return new Argument<string>(
            name: "path",
            description: "path to file containing graphs"
        );
    }
    private static Command getCliqueCommand() {

        var cliqueCommand = new Command(
            name: "clique",
            description: "Calculate clique of graph"
        );

        var algorithType = getAlgorithmType();
        var fileInput = getPathArgument();

        cliqueCommand.AddOption(algorithType);
        cliqueCommand.AddArgument(fileInput);
        
        cliqueCommand.SetHandler((isExact, input) => {
            List<Graph> graphs = Graph.ParseInputFile(input);
            Console.WriteLine($"Executing for isExact={isExact}, path={input}");
            if (isExact) {
                ExactCliques(graphs);
            } else {
                ApproximatedCliques(graphs);
            }
        }, algorithType, fileInput);

        return cliqueCommand;
    }

    private static Command getSubgraphCommand() {
        var subgraphCommand = new Command(
            name: "subgraph",
            description: "Calculate subgraph of graphs"
        );

        // how are we gonna pass them ?

        return subgraphCommand;
    }
}
