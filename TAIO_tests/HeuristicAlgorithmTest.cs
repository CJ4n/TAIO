using System.Collections.Immutable;
using System.Diagnostics;
using TAIO;

namespace TAIO_tests;

public class Tests
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
    public void ShouldRepairVerticesToCliques()
    {
        foreach (var graph in graphs)
        {
            Console.WriteLine("---------------------------------");
            Stopwatch stopwatch = Stopwatch.StartNew();

            ShouldRepairVerticesToClique(graph);

            stopwatch.Stop();

            Console.WriteLine(
                $"Time taken for graph size {graph.VerticesCount} : {stopwatch.ElapsedMilliseconds} ms"); // Report time taken
        }
    }


    private void ShouldRepairVerticesToClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);

        HashSet<int> vertices = Utils.PrepareVertices(graph);

        ha.Repair(vertices);

        Assert.IsTrue(Utils.CheckClique(graph, vertices.ToImmutableSortedSet()),
            $"Failed in graph ");
    }


    [Test]
    public void ShouldExtendGivenClique()
    {
        foreach (var graph in graphs)
        {
            Console.WriteLine("---------------------------------");
            Stopwatch stopwatch = Stopwatch.StartNew();

            ShouldExtendGivenClique(graph);

            stopwatch.Stop();

            Console.WriteLine(
                $"Time taken for graph size {graph.VerticesCount} : {stopwatch.ElapsedMilliseconds} ms"); // Report time taken
        }
    }

    public void ShouldExtendGivenClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);

        HashSet<int> vertices = Utils.PrepareVertices(graph);

        ha.Repair(vertices);
        ha.Extend(vertices);

        Assert.IsTrue(Utils.CheckClique(graph, vertices.ToImmutableSortedSet()),
            $"Failed in graph ");
    }


    [Test]
    public void ShouldFindSomeCliques()
    {
        foreach (var graph in graphs)
        {
            Console.WriteLine("---------------------------------");
            Stopwatch stopwatch = Stopwatch.StartNew();

            ShouldFindSomeClique(graph);

            stopwatch.Stop();

            Console.WriteLine(
                $"Time taken for graph size {graph.VerticesCount} : {stopwatch.ElapsedMilliseconds} ms"); // Report time taken
        }
    }

    public void ShouldFindSomeClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);

        HashSet<int> vertices = Utils.PrepareVertices(graph);

        ha.ApplyHeuristic(vertices);

        Assert.IsTrue(Utils.CheckClique(graph, vertices.ToImmutableSortedSet()),
            $"Failed in graph ");
    }
}