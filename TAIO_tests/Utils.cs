using System.Collections.Immutable;

namespace TAIO_tests;

public static class Utils
{
    private const string SUCCESS = "Solution check passed successfully!";

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

    public static bool CheckClique(Graph graph, ImmutableSortedSet<int> clique)
    {
        Console.WriteLine(
            $"Checking clique of size {clique.Count} in graph of size {graph.VerticesCount}.");
        int missingEdges = 0;
        for (int i = 0; i < clique.Count; i++)
        {
            for (int j = 0; j < clique.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (graph.GetAt(clique[i], clique[j]) == 0 ||
                    graph.GetAt(clique[j], clique[i]) == 0)
                {
                    missingEdges++;
                    Console.WriteLine($"Missing edge between {clique[i]} and {clique[j]}");
                }
            }
        }

        Console.WriteLine(missingEdges != 0 ? $"Missing {missingEdges} edges total." : SUCCESS);
        return missingEdges == 0;
    }
}