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

            var res = ga.Solve(graph);
            ((var cliqueGA, var LGA), var timeGA) = TimedUtils.Timed(() => ga.Solve(graph));
            ((var cliqueBK, var LBK), var time2BK) = TimedUtils.Timed(() => bk.Solve(graph));
            Console.WriteLine("------------------------RESULTS------------------------");
            Console.WriteLine(
                $"Genetic algorithm found clique of size: {cliqueGA.Count} in time: {timeGA} ");
            Console.WriteLine(
                $"Bron Kerbosch found clique of size: {cliqueBK.Count} in time: {time2BK} ");
            if (cliqueBK.Count != cliqueGA.Count)
            {
                Console.WriteLine(
                    "+++++++++++++++++++++++++++++++++++++++Clique sizes are different!+++++++++++++++++++++++++++++++++++++++");
                if (LGA != LBK)
                {
                    Console.WriteLine(
                        "+++++++++++++++++++++++++++++++++++++++ Cliques of different thickness!+++++++++++++++++++++++++++++++++++++++");
                }
            }


            Console.WriteLine(
                "----------------GENETIC ALGORITHM----------------");
            Helpers.EvaluateSolutionForCliqueProblem(graph, cliqueGA.ToImmutableSortedSet(), LGA,
                timeGA);
            Console.WriteLine(
                "----------------BRON KERBOSH---------------");
            Helpers.EvaluateSolutionForCliqueProblem(graph, cliqueBK.ToImmutableSortedSet(), LBK,
                time2BK);
        }
    }
}