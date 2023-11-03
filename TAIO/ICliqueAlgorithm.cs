using System.Collections.Immutable;

namespace TAIO;

public interface ICliqueAlgorithm
{
    public ImmutableSortedSet<int> Solve(Graph graph);
}
