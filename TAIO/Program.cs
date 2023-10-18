internal class Program
{
    private static void Main(string[] args)
    {
         try 
         {
            Graph graph = new Graph("some/path/to/file.txt");
         } catch(NotImplementedException exception)
        {
            Console.WriteLine(exception);
        } 
    }
}