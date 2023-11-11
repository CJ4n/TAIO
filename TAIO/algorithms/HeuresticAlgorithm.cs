using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Security.Authentication.ExtendedProtection;

namespace TAIO;

public class HeuresticAlgorithm
{
    private Graph _graph { get; }
    private int _n { get; }

    public HeuresticAlgorithm(Graph graph)
    {
        _graph = graph;
        _n = graph.VerticesCount;
    }

    public void ApplyHeuristic(HashSet<int> vertices)
    {
        Relax(vertices);
        Repair(vertices);
        Extend(vertices);
    }

    public void Relax(HashSet<int> vertices)
    {
        int numVerticesToAdd = _n / 3;
        for (int iter = 0; iter < numVerticesToAdd; iter++)
        {
            int id = new Random().Next(0, _n);
            vertices.Add(id);
        }
    }

    private bool GetRandomBool()
    {
        return new Random().Next(0, 2) == 0;
    }

    public void Repair(HashSet<int> vertices)
    {
        int idx = new Random().Next(0, _n);

        for (int currentNode = idx; currentNode < _n; currentNode++)
        {
            if (!vertices.Contains(currentNode))
            {
                continue;
            }

            if (GetRandomBool())
            {
                vertices.Remove(currentNode);
                continue;
            }
            DeleteNeighborsIfNotConnectedToNode(vertices, currentNode);
        }

        for (int currentNode = idx - 1; currentNode >= 0; currentNode--)
        {
            if (!vertices.Contains(currentNode))
            {
                continue;
            }

            if (GetRandomBool())
            {
                vertices.Remove(currentNode);
                continue;
            }
            DeleteNeighborsIfNotConnectedToNode(vertices, currentNode);
        }
    }

    private void DeleteNeighborsIfNotConnectedToNode(HashSet<int> vertices,
        int node)
    {
        for (int otherNode = node + 1; otherNode < _n; otherNode++)
        {
            if (!vertices.Contains(otherNode))
            {
                continue;
            }

            if (_graph.IsBidirectionalEdge(node, otherNode))
            {
                continue;
            }

            vertices.Remove(otherNode);
        }

        for (int otherNode = 0; otherNode < node - 1; otherNode++)
        {
            if (!vertices.Contains(otherNode))
            {
                continue;
            }

            if (_graph.IsBidirectionalEdge(node, otherNode))
            {
                continue;
            }

            vertices.Remove(otherNode);
        }
    }

    public void Extend(HashSet<int> vertices)
    {
        int idx = new Random().Next(0, _n);

        for (int currentNode = idx; currentNode < _n; currentNode++)
        {
            TryExtendCliqueWithGivenNode(vertices, currentNode);
        }

        for (int currentNode = 0; currentNode < idx; currentNode++)
        {
            TryExtendCliqueWithGivenNode(vertices, currentNode);
        }
    }

    private void TryExtendCliqueWithGivenNode(HashSet<int> vertices, int currentNode)
    {
        bool isConnected = true;
        foreach (var subgraphNode in vertices)
        {
            if (subgraphNode == currentNode)
            {
                continue;
            }

            if (_graph.GetAt(subgraphNode, currentNode) == 0 ||
                _graph.GetAt(currentNode, subgraphNode) == 0)
            {
                isConnected = false;
                break;
            }
        }

        if (isConnected)
        {
            vertices.Add(currentNode);
        }
    }
}