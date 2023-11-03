namespace TAIO;

public class GraphGenerator
{
    public static void generateGraphsToFile(int[] verticesCounts, String target)
    {
        using var outputFile = new StreamWriter(Path.Join(AppDomain.CurrentDomain.BaseDirectory, target));
        outputFile.WriteLine(verticesCounts.Length);
        for (int i = 0; i < verticesCounts.Length; i++)
        {
            outputFile.WriteLine(verticesCounts[i]);
            outputFile.WriteLine(Graph.GetRandomGraph(verticesCounts[i]).Serialize());
        }
    }
}
