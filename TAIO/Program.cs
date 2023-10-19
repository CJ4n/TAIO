internal class Program
{
    private static void Main(string[] args)
    {
         try
         {
             var graphs = Graph.ParseInputFile("graphs/data1.txt");
             foreach (var g in graphs)
             {
                 g.Print();
                 Console.WriteLine();
             }
         } catch(NotImplementedException exception)
         {
             Console.WriteLine(exception);
         } 
    }
}