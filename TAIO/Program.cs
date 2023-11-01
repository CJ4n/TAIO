using System.Collections.Immutable;
using TAIO;

internal class Program
{
    private static void Main(string[] args)
    {
         try
         {
             var graphs = Graph.ParseInputFile("graphs/data1.txt");
             foreach (var g in graphs)
             {
                 // Program
                 var clique = new BronKerboschMaximumClique().solve(g); 
                 Console.Write("Found clique of size " + clique.Count + " with vertices: ");
                 Helpers.PrintItems(clique.ToImmutableSortedSet());
                 Console.WriteLine(g);
             }
         } catch(NotImplementedException exception)
         {
             Console.WriteLine(exception);
         } 
    }
}
