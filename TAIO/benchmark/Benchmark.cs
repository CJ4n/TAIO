using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TAIO;
using static TAIO.TimedUtils;

namespace TAIO_tests;

public class Benchmark
{
    public static void RunFullCliqueBenchmark()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        List<CliqueRecord> cliqueRecords = new();
        int sampleSize = 10;
        List<int> verticesCounts = new List<int>{50, 60, 70, 80, 90, 100};
        List<float> edgeProbabilities = new List<float> {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
        
        foreach (int verticesCount in verticesCounts)
        foreach (float edgeProbability in edgeProbabilities)
            for (int i = 0; i < sampleSize; i++)
            {
                cliqueRecords.AddRange(RunCliqueBenchmark(verticesCount, edgeProbability));
                Console.WriteLine($"Done: V:{verticesCount}, EP:{edgeProbability}, no.{i+1}");
            }
        
        SaveBenchmark<CliqueRecord, CliqueRecord.CliqueRecordMap>(cliqueRecords);
    }

    private static List<CliqueRecord> RunCliqueBenchmark(int vertexCount, float edgeProbability)
    {
        List<CliqueRecord> cliqueRecords = new();
        Graph g = Graph.GetRandomGraph(vertexCount, edgeProbability);
        ((var cliqueBK, var LBK), double timeBK) = Timed(() => new BronKerboschMaximumClique().Solve(g));
        ((var cliqueGA, var LGA), double timeGA) = Timed(() => new GeneticAlgorithm().Solve(g));
        if(!SolutionChecker.CheckClique(g, cliqueBK, LBK)) throw new SolutionChecker.WrongSolutionException();
        if(!SolutionChecker.CheckClique(g, cliqueGA, LGA)) throw new SolutionChecker.WrongSolutionException();
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

    public static void RunFullSubgraphBenchmark()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        List<SubgraphRecord> subgraphRecords = new();
        int sampleSize = 10;
        List<int> verticesCounts = new List<int>{4, 5, 6, 7, 8, 9};
        List<float> edgeProbabilities = new List<float> {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
        
        foreach (int verticesCount in verticesCounts)
        foreach (float edgeProbability in edgeProbabilities)
            for (int i = 0; i < sampleSize; i++)
            {
                subgraphRecords.AddRange(RunSubgraphBenchmark(verticesCount, edgeProbability));
                Console.WriteLine($"Done: V:{verticesCount}, EP:{edgeProbability}, no.{i+1}");
            }
        
        SaveBenchmark<SubgraphRecord, SubgraphRecord.SubgraphRecordMap>(subgraphRecords);
    }
    
    private static List<SubgraphRecord> RunSubgraphBenchmark(int vertexCount, float edgeProbability)
    {
        List<SubgraphRecord> subgraphRecords = new();
        Graph g1 = Graph.GetRandomGraph(vertexCount, edgeProbability);
        Graph g2 = Graph.GetRandomGraph(vertexCount, edgeProbability);
        (var subgraphBK, double timeBK) = Timed(
            () => new SubgraphUsingClique(new BronKerboschMaximumClique()).Solve(g1,g2));
        (var subgraphGA, double timeGA) = Timed(
            () => new SubgraphUsingClique(new GeneticAlgorithm()).Solve(g1, g2));
        if(!SolutionChecker.CheckSubgraph(g1, g2, subgraphBK)) throw new SolutionChecker.WrongSolutionException();
        if(!SolutionChecker.CheckSubgraph(g1, g2, subgraphGA)) throw new SolutionChecker.WrongSolutionException();
        subgraphRecords.Add(new SubgraphRecord(
            g1.VerticesCount,
            g1.EdgesCount,
            g2.VerticesCount,
            g2.EdgesCount,
            subgraphBK.Count,
            subgraphGA.Count,
            subgraphBK.Count - subgraphGA.Count,
            timeBK,
            timeGA
        ));
        return subgraphRecords;
    }
    
    public static void RunFullApproximationBenchmark()
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1, 1])));

        List<ApproxRecord> approxRecords = new();
        int sampleSize = 10;
        List<int> verticesCounts = new List<int>{100, 500, 1000, 2000, 3000};
        List<int> cliqueCounts = new List<int>{20, 50, 100, 300, 500, 1000};
        List<float> edgeProbabilities = new List<float> {0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };

        foreach (int verticesCount in verticesCounts)
        foreach (int cliqueCount in cliqueCounts.Where(cliqueCount => cliqueCount < verticesCount))
        foreach (var edgeProbability in edgeProbabilities)
            for (int i = 0; i < sampleSize; i++)
            {
                approxRecords.AddRange(RunApproximationBenchmark(cliqueCount, verticesCount, edgeProbability));
                Console.WriteLine($"Done: V:{verticesCount}, C:{cliqueCount}, EP:{edgeProbability}, no.{i+1}");
            }
           
        
        SaveBenchmark<ApproxRecord, ApproxRecord.ApproxRecordMap>(approxRecords);
    }
    
    private static List<ApproxRecord> RunApproximationBenchmark(int maxClique, int vertexCount, float edgeProbability)
    {
        var r = new Random();
        List<int> cliqueCounts = Enumerable.Range(0, 3).Select(x => r.Next(maxClique - 5, maxClique)).ToList();
        List<ApproxRecord> approxRecords = new();
        Graph g = Graph.GetRandomGraphWithCliques(cliqueCounts, vertexCount, edgeProbability);
        ((var clique, var L), double time) = Timed(() => new GeneticAlgorithm().Solve(g));
        if(!SolutionChecker.CheckClique(g, clique, L)) throw new SolutionChecker.WrongSolutionException();
        approxRecords.Add(new ApproxRecord(
            g.VerticesCount,
            g.EdgesCount,
            cliqueCounts.Max(),
            clique.Count,
            cliqueCounts.Max() - clique.Count,
            time
        ));
        return approxRecords;
    }

    private static void SaveBenchmark<T, TMap>(List<T> records) where TMap : ClassMap<T>
    {
        String directory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "benchmarks");
        Directory.CreateDirectory(directory);
        String filename = $"{DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture)}.csv";
        String path = Path.Join(directory, filename);
        using (var writer = new StreamWriter(path))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<TMap>();
            csv.WriteRecords(records);
            csv.Flush();
        }
        Console.WriteLine("Saved to " + path);
    }
}
