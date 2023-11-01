using System.Collections.Immutable;

namespace TAIO;

public class SubgraphUsingClique
{
    public ImmutableSortedSet<(int, int)> Solve(Graph graph1, Graph graph2)
    {
        Graph g = ModularProduct(graph1, graph2);
        var clique = new BronKerboschMaximumClique().Solve(g);
        return clique.Select(v => (v / graph2.VerticesCount, v % graph2.VerticesCount)).ToImmutableSortedSet();
    }

    private Graph ModularProduct(Graph graph1, Graph graph2)
    {
        int vertices = graph1.VerticesCount * graph2.VerticesCount;
        var matrix = new int[vertices,  vertices];
        for (int p1 = 0; p1 < graph1.VerticesCount; p1++)
        for (int p2 = 0; p2 < graph2.VerticesCount; p2++)
        for (int q1 = 0; q1 < graph1.VerticesCount; q1++)
        for (int q2 = 0; q2 < graph2.VerticesCount; q2++)
        {
            int edge = p1==q1 || p2==q2 || graph1.GetAt(p1, q1) != graph2.GetAt(p2, q2)? 0: 1;
            matrix[p1 * graph2.VerticesCount + p2, q1 * graph2.VerticesCount + q2] = edge;
        }
        return new Graph(matrix);
        
    }
}
