using System.Collections.Immutable;
using TAIO;

internal class Program
{
    private static void Main(string[] args)
    {
         try
         {
             ExactCliques(Graph.ParseInputFile("graphs/data2.txt"));
             ExactSubgraphs(Graph.ParseInputFile("graphs/data3.txt"));
         } catch(NotImplementedException exception)
         {
             Console.WriteLine(exception);
         } 
    }

    private static void ExactSubgraphs(List<Graph> graphs)
    {
        for(int i=0; i<graphs.Count; i++)
        for(int j=i+1; j<graphs.Count; j++)
        {
            var g1 = graphs[i];
            var g2 = graphs[j];
            // Program
            var subgraph = new SubgraphUsingClique().Solve(g1, g2);
            Console.Write("Found subgraph of size " + subgraph.Count + " with vertices: ");
            Helpers.PrintItems(subgraph.ToImmutableSortedSet());
            // Console.WriteLine(g1);
            // Console.WriteLine(g2);
            Console.WriteLine("===============================================");
        }
    }

    private static void ExactCliques(List<Graph> graphs)
    {
        foreach (var g in graphs)
        {
            // Program
            var clique = new BronKerboschMaximumClique().Solve(g);
            Console.Write("Found clique of size " + clique.Count + " with vertices: ");
            Helpers.PrintItems(clique.ToImmutableSortedSet());
            Console.WriteLine(g);
            Console.WriteLine("===============================================");
        }
    }
}
