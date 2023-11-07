using System.Collections.Immutable;

namespace TAIO;

public interface ICliqueAlgorithm
{
    public (ImmutableSortedSet<int>,int) Solve(Graph graph);
}
