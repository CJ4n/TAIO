using System.Collections.Immutable;
using TAIO;
using TAIO.tests;

internal class Program
{
    private static void Main(string[] args)
    {
         try
         {
             // Tests
             // GraphTests.run();
             var graphs = Graph.ParseInputFile("graphs/data1.txt");
             foreach (var g in graphs)
             {
                 // Program
                 var clique = new BronKerboschMaximumClique().solve(g); 
                 Console.Write("Found clique of size " + clique.Count + " with vertices: ");
                 Helpers.PrintItems(clique.ToImmutableSortedSet());
                 g.Print();
                 Console.WriteLine();
             }
         } catch(NotImplementedException exception)
         {
             Console.WriteLine(exception);
         } 
    }
}
