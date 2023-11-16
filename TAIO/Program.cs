using System.CommandLine;
using TAIO;
using TAIO_tests;

internal class Program
{
static async Task<int> Main(string[] args)
{
        Benchmark.RunFullCliqueBenchmark();
        // GraphGenerator.GenerateGraphsToFile(new[]{5}, "graphs/data5.txt");
        return 0;
        var rootCommand = new RootCommand("This is a simple program for graph property exploration");

        rootCommand.AddCommand(GetCliqueCommand());
        rootCommand.AddCommand(GetSubgraphCommand());
        rootCommand.AddCommand(GetSizeCommand());
        rootCommand.AddCommand(GetDistanceCommand());
        return await rootCommand.InvokeAsync(args);
    }

    private static Option<bool> GetAlgorithmType() 
    {
        return new Option<bool> (
            name: "isExact",
            description: "Should use exact algorithms for calculations",
            getDefaultValue: () => false);
        
    }

    private static Argument<string> GetPathArgument() 
    {
        return new Argument<string>(
            name: "path to file",
            description: "path to file containing graphs"
        );
    }

    private static Command GetCliqueCommand() 
    {

        var cliqueCommand = new Command(
            name: "clique",
            description: "Calculate clique of graph"
        );

        var algorithmType = GetAlgorithmType();
        var fileInput = GetPathArgument();

        cliqueCommand.AddOption(algorithmType);
        cliqueCommand.AddArgument(fileInput);
        
        cliqueCommand.SetHandler((isExact, input) => {
            List<Graph> graphs = Graph.ParseInputFile(input);
            if (isExact) 
            {
                Tasks.ExactCliques(graphs);
            }
            else
            {
                Tasks.ApproximatedCliques(graphs);
            }
        }, algorithmType, fileInput);

        return cliqueCommand;
    }

    private static Command GetSubgraphCommand() 
    {
        var subgraphCommand = new Command(
            name: "subgraph",
            description: "Calculate subgraphs of provided pairs of graphs"
        );

        var algorithmType = GetAlgorithmType();
        var fileInput = GetPathArgument();

        subgraphCommand.AddOption(algorithmType);
        subgraphCommand.AddArgument(fileInput);
        
        subgraphCommand.SetHandler((isExact, input)=> {
            List<Graph> graphs = Graph.ParseInputFile(input);
            if (graphs.Count % 2 != 0) 
            {
                Console.WriteLine("Amount of graphs in file needs to be divisible by 2");
                return;
            }
            if (isExact) 
            {
                Tasks.ExactSubgraphs(graphs);
            }
            else 
            {
                Tasks.ApproximatedSubgraphs(graphs);
            }
        }, algorithmType, fileInput);

        return subgraphCommand;
    }

    private static Command GetSizeCommand() 
    {
        var sizeCommand = new Command(
            name: "size",
            description: "Calculate size of each graph"
        );

        var fileInput = GetPathArgument();
        sizeCommand.AddArgument(fileInput);

        sizeCommand.SetHandler((input)=>{
            List<Graph> graphs = Graph.ParseInputFile(input);
            Tasks.Size(graphs);
        }, fileInput);

        return sizeCommand;
    }

    private static Command GetDistanceCommand()
    {
        var metricsCommand = new Command(
            name: "distance",
            description: "Calculate distance between pairs of graphs"
        );

        var fileInput = GetPathArgument();
        var algorithmType = GetAlgorithmType();

        metricsCommand.AddArgument(fileInput);
        metricsCommand.AddOption(algorithmType);

        metricsCommand.SetHandler((isExact, input)=> {
            List<Graph> graphs = Graph.ParseInputFile(input);
            if (graphs.Count % 2 != 0) 
            {
                Console.WriteLine("Amount of graphs in file needs to be divisible by 2");
                return;
            }
            if (isExact) 
            {
                Tasks.ExactMatrics(graphs);
            }
            else 
            {
                Tasks.ApproximateMetrics(graphs);
            }
        }, algorithmType, fileInput);

        return metricsCommand;
    }
}
