namespace TAIO;

public abstract class TimedUtils
{
    public static (T, double) Timed<T>(Func<T> func)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        T result = func.Invoke();
        watch.Stop();
        return (result, watch.Elapsed.TotalMilliseconds);
    }
}
