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
    public void ShouldFindSomeCliquesWithGeneticAlgorithm()
    {
        foreach (var graph in graphs)
        {
            ShouldFindSomeCliqueWithGeneticAlgorithm(graph);
        }
    }

    public void ShouldFindSomeCliqueWithGeneticAlgorithm(Graph graph)
    {
        GeneticAlgorithm ga = new GeneticAlgorithm(graph);
        (var clique, var time) = TimedUtils.Timed(() => ga.Run());
        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique.ToImmutableSortedSet(), time));
    }
}