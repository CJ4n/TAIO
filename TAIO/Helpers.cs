namespace TAIO;

public static class Helpers
{
    public static void PrintItems<T>(IEnumerable<T> es)
    {
        Console.Write("[");
        foreach (var e in es)
        {
            Console.Write(e + " ");
        }
        Console.WriteLine("]");
    }
}
