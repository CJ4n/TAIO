using System.Collections.Immutable;
using TAIO;

internal class Program
{
    private static void Main(string[] args)
    {
         try
         {
             // GraphGenerator.generateGraphsToFile(new[]{30, 30}, "graphs/data4.txt");
             ExactCliques(Graph.ParseInputFile("graphs/data_K.txt"));
             // ExactSubgraphs(Graph.ParseInputFile("graphs/data4.txt"));
         } catch(NotImplementedException exception)
         {
             Console.WriteLine(exception);
         } 
    }

    private static void ExactSubgraphs(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new SubgraphUsingClique().Solve(new Graph(new int[1,1]), new Graph(new int[1,1])));
        
        for(int i=0; i<graphs.Count; i++)
        for(int j=i+1; j<graphs.Count; j++)
        {
            var g1 = graphs[i];
            var g2 = graphs[j];
            (var subgraph, double time) = TimedUtils.Timed(() => new SubgraphUsingClique().Solve(g1, g2));
            Console.WriteLine($"Found subgraph of size {subgraph.Count} with vertices mapping: {Helpers.ItemsToString(subgraph)} in {time} ms");
            PrintHighlightedSubgraph(g1, subgraph, 0);
            Console.WriteLine();
            PrintHighlightedSubgraph(g2, subgraph, 1);
            SolutionChecker.checkSubgraph(g1, g2, subgraph);
            Console.WriteLine("===============================================");
        }
    }

    private static void PrintHighlightedSubgraph(Graph g, ImmutableSortedSet<(int, int)> subgraph, int index)
    {
        bool[,] highlight = new bool[g.VerticesCount, g.VerticesCount];
        foreach (var p in subgraph)
        foreach (var q in subgraph)
            if(index == 0) highlight[p.Item1, q.Item1] = true;
            else highlight[p.Item2, q.Item2] = true;
        g.PrintHighlighted(highlight);
    }

    private static void ExactCliques(List<Graph> graphs)
    {
        // Warm-up (first timed execution is faulty probably because of C# preprocessing taking time)
        TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(new Graph(new int[1,1])));
        
        foreach (var g in graphs)
        {
            (var clique, double time) = TimedUtils.Timed(() => new BronKerboschMaximumClique().Solve(g));
            Console.WriteLine($"Found clique of size {clique.Count} with vertices: {Helpers.ItemsToString(clique)} in {time} ms");
            PrintHighlightedClique(g, clique);
            SolutionChecker.checkClique(g, clique);
            Console.WriteLine("===============================================");
        }
    }

    private static void PrintHighlightedClique(Graph g, ImmutableSortedSet<int> clique)
    {
        bool[,] highlight = new bool[g.VerticesCount, g.VerticesCount];
        foreach (var p in clique)
        foreach (var q in clique)
            highlight[p, q] = true;
        g.PrintHighlighted(highlight);
    }
}
