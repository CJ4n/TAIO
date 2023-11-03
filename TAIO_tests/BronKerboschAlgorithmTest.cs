using TAIO;

namespace TAIO_tests;

public class BronKerboschAlgorithmTest
{
    private IEnumerable<Graph> graphs;


    [SetUp]
    public void Setup()
    {
        graphs = Utils.LoadGraphs();
    }

    [Test]
    public void ShouldFindMaximalCliques()
    {
        foreach (var graph in graphs)
        {
            if (graph.VerticesCount < 10)
            {
                ShouldFindMaximalClique(graph);
            }
        }
    }

    public void ShouldFindMaximalClique(Graph graph)
    {
        BronKerboschMaximumClique bk = new BronKerboschMaximumClique();

        (var clique, var time) = TimedUtils.Timed(() => bk.Solve(graph));
        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique, time));
    }
}