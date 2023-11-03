using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TAIO;
using static TAIO.TimedUtils;

namespace TAIO_tests;

public class Benchmark
{
    private class CliqueRecord
    {
        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }
        public int MaximumCliqueSize { get; set; }
        public int MaximumCliqueApproximationSize { get; set; }
        public int ApproximationError { get; set; }
        public double BronKerboschTime { get; set; }
        public double GeneticAlgorithmTime { get; set; }

        public CliqueRecord(int vertexCount, int edgeCount, int maximumCliqueSize, int maximumCliqueApproximationSize, int approximationError, double bronKerboschTime, double geneticAlgorithmTime)
        {
            VertexCount = vertexCount;
            EdgeCount = edgeCount;
            MaximumCliqueSize = maximumCliqueSize;
            MaximumCliqueApproximationSize = maximumCliqueApproximationSize;
            BronKerboschTime = bronKerboschTime;
            GeneticAlgorithmTime = geneticAlgorithmTime;
            ApproximationError = approximationError;
        }
    }
    
    private class CliqueRecordMap : ClassMap<CliqueRecord>
    {
        public CliqueRecordMap()
        {
            Map(m => m.VertexCount).Index(0).Name("v");
            Map(m => m.EdgeCount).Index(0).Name("e");
            Map(m => m.MaximumCliqueSize).Index(0).Name("maxCliqueBK");
            Map(m => m.MaximumCliqueApproximationSize).Index(0).Name("maxCliqueApproxGA");
            Map(m => m.ApproximationError).Index(0).Name("error");
            Map(m => m.BronKerboschTime).Index(0).Name("ExactTimeKB");
            Map(m => m.GeneticAlgorithmTime).Index(0).Name("ApproxTimeGA");

        }
    }

    public static void RunFullCliqueBenchmark()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        List<CliqueRecord> cliqueRecords = new();
        int sampleSize = 1;
        List<int> verticesCounts = new List<int>{10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
        List<float> edgeProbabilities = new List<float> {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
        
        foreach (int verticesCount in verticesCounts)
        foreach (float edgeProbability in edgeProbabilities)
            for (int i = 0; i < sampleSize; i++)
            {
                cliqueRecords.AddRange(RunCliqueBenchmark(verticesCount, edgeProbability));
                Console.WriteLine($"Done: V:{verticesCount}, EP:{edgeProbability}, no.{i+1}");
            }
        
        saveBenchmark(cliqueRecords);
    }

    private static List<CliqueRecord> RunCliqueBenchmark(int vertexCount, float edgeProbability)
    {
        List<CliqueRecord> cliqueRecords = new();
        Graph g = Graph.GetRandomGraph(vertexCount, edgeProbability);
        (var cliqueBK, double timeBK) = Timed(() => new BronKerboschMaximumClique().Solve(g));
        (var cliqueGA, double timeGA) = Timed(() => new GeneticAlgorithm().Solve(g));
        if(!SolutionChecker.CheckClique(g, cliqueBK)) throw new SolutionChecker.WrongSolutionException();
        if(!SolutionChecker.CheckClique(g, cliqueGA)) throw new SolutionChecker.WrongSolutionException();
        cliqueRecords.Add(new CliqueRecord(
                vertexCount,
                g.EdgesCount,
                cliqueBK.Count,
                cliqueGA.Count,
                cliqueBK.Count - cliqueGA.Count,
                timeBK,
                timeGA
                ));
        return cliqueRecords;
    }

    private static void saveBenchmark(List<CliqueRecord> cliqueRecords)
    {
        String directory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "benchmarks");
        Directory.CreateDirectory(directory);
        String filename = $"{DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)}.csv";
        String path = Path.Join(directory, filename);
        using (var writer = new StreamWriter(path))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(cliqueRecords);
            csv.Flush();
        }
        Console.WriteLine("Saved to " + path);
    }
}
