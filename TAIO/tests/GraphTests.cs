namespace TAIO.tests;

public class GraphTests
{
    private static int testCount = 0;
    
    public static void assert<T>(T b1, T b2)
    {
        if (!b1.Equals(b2)) Console.WriteLine("#" + testCount + "\t Test Failed");
        if (!b1.Equals(b2)) Console.WriteLine("Is:\t" + b1);
        if (!b1.Equals(b2)) Console.WriteLine("Should be:\t" + b2);
    }
    public static void run()
    {
        var g = Graph.ParseInputFile("graphs/test1.txt")[0]; 
        // assert(g.getNeighboursBi(2), new HashSet<int> {0, 1});
        
        
    }
}
