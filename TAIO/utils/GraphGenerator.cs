namespace TAIO;

public class GraphGenerator
{
    public static void GenerateGraphsToFile(int[] verticesCounts, String target)
    {
        using var outputFile = new StreamWriter(Path.Join(AppDomain.CurrentDomain.BaseDirectory, target));
        outputFile.WriteLine(verticesCounts.Length);
        foreach (var verticesCount in verticesCounts)
        {
            outputFile.WriteLine(verticesCount);
            outputFile.WriteLine(Graph.GetRandomGraph(verticesCount, 0.8f).Serialize());
        }
    }
}
