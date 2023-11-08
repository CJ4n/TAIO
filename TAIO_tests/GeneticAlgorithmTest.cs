using System.Collections.Immutable;
using System.Diagnostics;
using TAIO;

namespace TAIO_tests;

public class GeneticAlgorithmTest
{
    private IEnumerable<Graph> graphs;


    [SetUp]
    public void Setup()
    {
        graphs = Utils.LoadGraphs();
    }


    [Test]
    public void ShouldFindSomeCliqueOnlyCheckingTheBiggestGraph()
    {
        foreach (var graph in graphs)
        {
            if (graph.VerticesCount < 100)
            {
                continue;
            }

            ShouldFindSomeCliqueWithGeneticAlgorithm(graph);
        }
    }


    [Test]
    public void ShouldFindSomeCliquesWithGeneticAlgorithm()
    {
        foreach (var graph in graphs)
        {
            ShouldFindSomeCliqueWithGeneticAlgorithm(graph);
        }
    }

    public void ShouldFindSomeCliqueWithGeneticAlgorithm(Graph graph)
    {
        GeneticAlgorithm ga = new GeneticAlgorithm();
        ((var clique, var L), var time) = TimedUtils.Timed(() => ga.Solve(graph));
        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique.ToImmutableSortedSet(), L,
                time));
    }
}