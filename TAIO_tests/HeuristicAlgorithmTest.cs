using System.Collections.Immutable;
using System.Diagnostics;
using TAIO;

namespace TAIO_tests;

public class Tests
{
    private IEnumerable<Graph> graphs;

    [SetUp]
    public void Setup()
    {
        graphs = Utils.LoadGraphs();
    }

    [Test]
    public void ShouldRepairVerticesToCliques()
    {
        foreach (var graph in graphs)
        {
            ShouldRepairVerticesToClique(graph);
        }
    }

    private void ShouldRepairVerticesToClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);
        HashSet<int> vertices = Utils.PrepareVertices(graph);

        (var clique, var time) = TimedUtils.Timed(() =>
        {
            ha.Repair(vertices);
            return vertices;
        });

        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique.ToImmutableSortedSet(), time));
    }

    [Test]
    public void ShouldExtendGivenClique()
    {
        foreach (var graph in graphs)
        {
            ShouldExtendGivenClique(graph);
        }
    }

    public void ShouldExtendGivenClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);

        HashSet<int> vertices = Utils.PrepareVertices(graph);
        (var clique, var time) = TimedUtils.Timed(() =>
        {
            ha.Repair(vertices);
            ha.Extend(vertices);
            return vertices;
        });

        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique.ToImmutableSortedSet(), time));
    }

    [Test]
    public void ShouldFindSomeCliques()
    {
        foreach (var graph in graphs)
        {
            ShouldFindSomeClique(graph);
        }
    }

    public void ShouldFindSomeClique(Graph graph)
    {
        var ha = new HeuresticAlgorithm(graph);

        HashSet<int> vertices = Utils.PrepareVertices(graph);

        (var clique, var time) = TimedUtils.Timed(() =>
        {
            ha.ApplyHeuristic(vertices);
            return vertices;
        });
        Assert.IsTrue(
            Helpers.EvaluateSolutionForCliqueProblem(graph, clique.ToImmutableSortedSet(), time));
    }
}