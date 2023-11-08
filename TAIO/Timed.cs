namespace TAIO;

using System.Collections.Immutable;

public abstract class TimedUtils
{
    public static (T, double) Timed<T>(
        Func<T> func)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        T res  = func.Invoke();
        watch.Stop();
        return (res, watch.Elapsed.TotalMilliseconds);
    }
}