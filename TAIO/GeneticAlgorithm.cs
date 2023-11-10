using System.Collections.Immutable;

namespace TAIO;

public class GeneticAlgorithm : ICliqueAlgorithm
{
    private Graph _graph;
    private int _n;
    private HeuresticAlgorithm _ha;
    private int _populationSize { get; set; } = 50;
    private List<HashSet<int>> _populations { get; set; }
    private float _mutationRate { get; } = 0.05f;
    private int _maxIterations { get; } = 200;
    private int _iteraionCounter { get; set; }
    private int _howManyBestToKeep { get; } = 4;

    public (ImmutableSortedSet<int>, int) Solve(Graph graph)
    {
        InitializeAlgorithm(graph);
        InitialPopulation();

        ApplyHeuristic();
        while (!TerminationCondition())
        {
            Selection();
            // Save the best solutions before the genetic operations
            var bestSolutions = _populations.OrderByDescending(Fitness)
                .Take(_howManyBestToKeep)
                .Select(solution => new HashSet<int>(solution))
                .ToList();
            Crossover();
            Mutation();

            ApplyHeuristic();
            _populations.AddRange(bestSolutions);
        }

        var solutoin = _populations.OrderByDescending(Fitness).First();
        var thickness = GetMinCliqueThickness(solutoin);

        return (solutoin.ToImmutableSortedSet(), thickness);
    }

    private void InitializeAlgorithm(Graph graph)
    {
        _ha = new HeuresticAlgorithm(graph);
        _graph = graph;
        _n = graph.VerticesCount;
        _populationSize = Math.Max(50, graph.VerticesCount / 3);
    }

    private void Crossover()
    {
        List<HashSet<int>> newPopulation = new List<HashSet<int>>();
        Random random = new Random();

        while (newPopulation.Count < _populationSize - _howManyBestToKeep)
        {
            var parent1 = _populations[random.Next(_populations.Count)];
            var parent2 = _populations[random.Next(_populations.Count)];
            var child = new HashSet<int>(parent1);

            foreach (var vertex in parent2)
            {
                if (random.NextDouble() < 0.5)
                {
                    child.Add(vertex);
                }
            }

            newPopulation.Add(child);
        }

        _populations = newPopulation;
    }

    private void Mutation()
    {
        Random random = new Random();
        foreach (var solution in _populations)
        {
            if (random.NextDouble() < _mutationRate)
            {
                int vertex = random.Next(_n);
                if (solution.Contains(vertex))
                {
                    solution.Remove(vertex);
                }
                else
                {
                    solution.Add(vertex);
                }
            }
        }
    }

    private bool IsFirstCliqueBigger(HashSet<int> clique1, HashSet<int> clique2)
    {
        if (clique1.Count > clique2.Count)
        {
            return true;
        }

        if (clique1.Count < clique2.Count)
        {
            return false;
        }

        int minThicknessInClique1 = GetMinCliqueThickness(clique1);
        int minThicknessInClique2 = GetMinCliqueThickness(clique2);
        return minThicknessInClique1 > minThicknessInClique2;
    }

    private int GetMinCliqueThickness(HashSet<int> clique)
    {
        int minThicknessInClique = int.MaxValue;
        foreach (var node1 in clique)
        {
            foreach (var node2 in clique)
            {
                if (node1 == node2)
                {
                    continue;
                }

                minThicknessInClique = Math.Min(minThicknessInClique,
                    _graph.GetAtBidirectional(node1, node2));
            }
        }

        if (minThicknessInClique == int.MaxValue)
        {
            return 0;
        }

        return minThicknessInClique;
    }

    private void Selection()
    {
        // Convert the custom comparator to a Comparison<HashSet<int>> delegate
        Comparison<HashSet<int>> cliqueComparer = (clique1, clique2) =>
        {
            bool isFirstCliqueBigger = IsFirstCliqueBigger(clique1, clique2);
            if (isFirstCliqueBigger)
            {
                return
                    -1; // clique1sFirstCliqueBigger(clique1, clique2)) is bigger, so it should come first
            }

            if (!isFirstCliqueBigger)
            {
                return 1; // clique2 is bigger, so it should come first
            }

            return 0; // Both are equal
        };

        // Convert _populations to a List, sort it using the custom comparator, and then convert it back to IEnumerable
        var sortedPopulations = _populations.ToList();
        sortedPopulations.Sort(cliqueComparer);

        // Take the top half of the sorted list
        _populations = sortedPopulations.Take(_populationSize / 2).ToList();
    }

    private void ApplyHeuristic()
    {
        foreach (var population in _populations)
        {
            _ha.ApplyHeuristic(population);
        }
    }

    private int Fitness(HashSet<int> population)
    {
        return population.Count;
    }

    private bool TerminationCondition()
    {
        return _iteraionCounter++ > _maxIterations;
    }

    private void InitialPopulation()
    {
        _populations = new List<HashSet<int>>();
        Random random = new Random();

        for (int i = 0; i < _populationSize; i++)
        {
            var clique = new HashSet<int>();
            for (int j = 0; j < _n; j++)
            {
                if (random.NextDouble() < 0.5)
                {
                    clique.Add(j);
                }
            }

            _populations.Add(clique);
        }
    }
}