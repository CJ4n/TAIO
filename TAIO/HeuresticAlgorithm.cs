using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Security.Authentication.ExtendedProtection;

namespace TAIO;

public class HeuresticAlgorithm
{
    private Graph _graph;

    public HeuresticAlgorithm(Graph graph)
    {
        _graph = graph;
    }

    public void FindClinque(Vector<int> verticesIds)
    {
        Dictionary<int, int> vertices = new Dictionary<int, int>();
        for (int i = 0; i < verticesIds.Length; i++)
        {
            vertices.Add(verticesIds[i], verticesIds[i]);
        }

        Enlarge(vertices);
        Repair(vertices);
    }

    private void Enlarge(Dictionary<int, int> vertices)
    {
        int n = _graph.VerticesCount;
        int numVerticesToAdd = 4;
        for (int iter = 0; iter < numVerticesToAdd; iter++)
        {
            int id = new Random().Next(0, n);
            vertices.Add(id, id);
        }
    }

    private void Repair(Dictionary<int, int> vertices)
    {
        int n = _graph.VerticesCount;
        int idx = new Random().Next(0, n);

        for (int i = idx; i < n; i++)
        {
            if (!vertices.ContainsKey(i))
            {
                continue;
            }

            int option = new Random().Next(0, 2);
            if (option == 0)
            {
                vertices.Remove(i);
            }
            else
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (!vertices.ContainsKey(j))
                    {
                        continue;
                    }

                    if (0 != _graph.GetAt(j, i))
                    {
                        continue;
                    }

                    vertices.Remove(j);
                }

                for (int j = 0; j < i-1; j++)
                {
                    if (!vertices.ContainsKey(j))
                    {
                        continue;
                    }

                    if (0 != _graph.GetAt(j, i))
                    {
                        continue;
                    }

                    vertices.Remove(j);
                }
            }
        }
    }

    private void Extend()
    {
        throw new NotImplementedException();
    }
}