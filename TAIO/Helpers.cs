using System.Text;

namespace TAIO;

public static class Helpers
{
    public static String ItemsToString<T>(IEnumerable<T> es)
    {
        var sb = new StringBuilder();
        sb.Append('[');
        foreach (var e in es)
        {
            sb.Append(e + " ");
        }
        sb.Append(']');
        return sb.ToString();
    }
}
