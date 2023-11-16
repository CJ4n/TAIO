using System.CommandLine;
using TAIO;
using TAIO_tests;

internal class Program
{
static async Task<int> Main(string[] args)
{
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

    private static Argument<List<string>> GetPathArgument() 
    {
        return new Argument<List<string>>(
            name: "path to files",
            description: "path to files containing graphs"
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
            if (input.Count != 1)
            {
                Console.WriteLine("Clique calculation should be provided with only one file");
                return;
            }
            List<Graph> graphs = Graph.ParseInputFile(input[0]);
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
            if (input.Count != 2)
            {
                Console.WriteLine("Subgraph calculation should be provided with two files");
                return;
            }
            List<Graph> graphs1 = Graph.ParseInputFile(input[0]);
            List<Graph> graphs2 = Graph.ParseInputFile(input[1]);
            if (isExact) 
            {
                Tasks.ExactSubgraphs(graphs1, graphs2);
            }
            else 
            {
                Tasks.ApproximatedSubgraphs(graphs1, graphs2);
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
            List<Graph> graphs = Graph.ParseInputFile(input[0]);
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
            if (input.Count != 2)
            {
                Console.WriteLine("Subgraph calculation should be provided with two files");
                return;
            }
            List<Graph> graphs1 = Graph.ParseInputFile(input[0]);
            List<Graph> graphs2 = Graph.ParseInputFile(input[1]);
            if (isExact) 
            {
                Tasks.ExactMatrics(graphs1, graphs2);
            }
            else 
            {
                Tasks.ApproximateMetrics(graphs1, graphs2);
            }
        }, algorithmType, fileInput);

        return metricsCommand;
    }
}
