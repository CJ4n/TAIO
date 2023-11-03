using System.Collections.Immutable;
using TAIO;

namespace TAIO_tests;

public class CompareBronKerboschWithGeneticAlgorithm
{
    private IEnumerable<Graph> graphs;


    [SetUp]
    public void Setup()
    {
        graphs = Utils.LoadGraphs();
    }


    [Test]
    public void CompareBronKerboschWithGeneticAlgorithmTest()
    {
        foreach (var graph in graphs)
        {
            if (graph.VerticesCount > 20)
            {
                continue;
            }

            GeneticAlgorithm ga = new GeneticAlgorithm();
            BronKerboschMaximumClique bk = new BronKerboschMaximumClique();

            (var cliqueGA, var timeGA) = TimedUtils.Timed(() => ga.Solve(graph));
            (var cliqueBK, var time2BK) = TimedUtils.Timed(() => bk.Solve(graph));
            Console.WriteLine("------------------------RESULTS------------------------");
            Console.WriteLine(
                $"Genetic algorithm found clique of size: {cliqueGA.Count} in time: {timeGA} ");
            Console.WriteLine(
                $"Bron Kerbosch found clique of size: {cliqueBK.Count} in time: {time2BK} ");
            if (cliqueBK.Count != cliqueGA.Count)
            {
                Console.WriteLine(
                    "+++++++++++++++++++++++++++++++++++++++Clique sizes are different!+++++++++++++++++++++++++++++++++++++++");
            }

            Console.WriteLine(
                "----------------GENETIC ALGORITHM----------------");
            Helpers.EvaluateSolutionForCliqueProblem(graph, cliqueGA.ToImmutableSortedSet(),
                timeGA);
            Console.WriteLine(
                "----------------BRON KERBOSH---------------");
            Helpers.EvaluateSolutionForCliqueProblem(graph, cliqueBK.ToImmutableSortedSet(),
                time2BK);
        }
    }
}
