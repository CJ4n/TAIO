namespace TAIO;

public abstract class TimedUtils
{
    /**
     * Utility class providing a monad for gathering data on execution time of a function.
     */
    public static (T, double) Timed<T>(
        Func<T> func)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        T res  = func.Invoke();
        watch.Stop();
        return (res, watch.Elapsed.TotalMilliseconds);
    }
}
