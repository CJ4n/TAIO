using System.Collections.Immutable;
using System.Diagnostics;
using TAIO;

namespace TAIO_tests;

public class GeneticAlgorithmTest
{
    private IEnumerable<Graph> graphs;


    [SetUp]
    public void Setup()
    {
        string directoryPath = "graphs"; // Directory containing the graph files

        if (Directory.Exists(directoryPath))
        {
            graphs = Directory.EnumerateFiles(directoryPath)
                .SelectMany(file => Utils.TryParseGraphsFromFile(file));
        }
        else
        {
            Console.WriteLine($"Directory not found: {directoryPath}");
            graphs = Enumerable.Empty<Graph>(); // Ensure graphs is not null
        }
    }


    [Test]
    public void ShouldFindSomeCliquesWithGeneticAlgorithm()
    {
        foreach (var graph in graphs)
        {
            Console.WriteLine("---------------------------------");
            Stopwatch stopwatch = Stopwatch.StartNew();

            ShouldFindSomeCliqueWithGeneticAlgorithm(graph);

            stopwatch.Stop();

            Console.WriteLine(
                $"Time taken for graph size {graph.VerticesCount} : {stopwatch.ElapsedMilliseconds} ms"); // Report time taken
        }
    }

    public void ShouldFindSomeCliqueWithGeneticAlgorithm(Graph graph)
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(graph);
        var vertices = ga.Run();
        Assert.IsTrue(Utils.CheckClique(graph, vertices.ToImmutableSortedSet()),
            $"Failed in graph ");
    }
}