using System.Collections.Immutable;

namespace TAIO_tests;

public static class Utils
{
    private const string SUCCESS = "Solution check passed successfully!";

    public static IEnumerable<Graph> LoadGraphs()
    {
        string directoryPath = "graphs"; // Directory containing the graph files
        IEnumerable<Graph> graphs;
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

        return graphs;
    }

    public static HashSet<int> PrepareVertices(Graph graph)
    {
        HashSet<int> vertices = new HashSet<int>();
        int size = graph.VerticesCount;
        int numverticestoadd =
            new Random((int)DateTime.Now.ToFileTime()).Next(2, Math.Max(size / 2, 2));
        for (int i = 0; i < numverticestoadd; i++)
        {
            vertices.Add(new Random((int)DateTime.Now.ToFileTime()).Next(0, size));
        }

        return vertices;
    }

    public static IEnumerable<Graph> TryParseGraphsFromFile(string filePath)
    {
        try
        {
            return Graph.ParseInputFile(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing file {filePath}: {ex.Message}");
            return Enumerable.Empty<Graph>(); // Return an empty enumerable on error
        }
    }


}