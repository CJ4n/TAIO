﻿namespace TAIO;

public class GeneticAlgorithm
{
    private Graph _graph { get; }
    private int _n { get; }
    private HeuresticAlgorithm _ha { get; }
    private int _populationSize { get; }
    private List<HashSet<int>> _populations { get; set; }
    private float _mutationRate { get; }
    private int _maxIterations { get; }
    private int _iteraionCounter { get; set; }

    public GeneticAlgorithm(Graph graph)
    {
        _ha = new HeuresticAlgorithm(graph);
        _graph = graph;
        _n = graph.VerticesCount;
        _populationSize = _n;
        _maxIterations = 100;
        _mutationRate = 0.05f;
    }


    private void Crossover()
    {
        List<HashSet<int>> newPopulation = new List<HashSet<int>>();
        Random random = new Random();

        while (newPopulation.Count < _populationSize)
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

    private List<HashSet<int>> Selection()
    {
        // Implement selection logic, e.g., tournament selection, roulette wheel, etc.
        // For simplicity, let's assume we return a subset of the existing population.
        return _populations.OrderBy(x => Fitness(x)).Take(_populationSize / 2).ToList();
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

    public HashSet<int> Run()
    {
        InitialPopulation();

        ApplyHeuristic();
        while (!TerminationCondition())
        {
            List<HashSet<int>> selected = Selection();
            Crossover();
            Mutation();
            ApplyHeuristic();
            // Optionally, replace part of the population with the selected ones
            // to maintain diversity.
        }

        return _populations.OrderByDescending(Fitness).First();
        // Post-process the final population to determine the best solution.
    }
}