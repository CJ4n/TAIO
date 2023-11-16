using System.Collections;
using System.Text;
using TAIO;

public class Graph
{
    public int VerticesCount { get; set; }
    public int EdgesCount { get; set; }
    public int[,]? Matrix { get; set; }

    // Symmetrical matrix with only bidirectional edges of the graph.
    private int[,]? BidirectionalMatrix { get; set; }
    // Optional description of the graph
    private string Description { get; set; }


    /*
     * Create Empty graph
     */
    public Graph(int verticesCount)
    {
        VerticesCount = verticesCount;
        Matrix = new int[verticesCount, verticesCount];
        Description = "";
    }

    /*
     * Create a graph from adjacency matrix
     */
    public Graph(int[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
            throw new ArgumentException("Adjacency matrix has to be a square");
        VerticesCount = matrix.GetLength(0);
        Matrix = matrix;
        EdgesCount = CountEdges();
        BidirectionalMatrix = RemoveSingularEdges();
    }

    /*
     * Count edges in the adjacency matrix
     */
    private int CountEdges()
    {
        if (Matrix == null) return 0;
        return Enumerable.Range(0, Matrix.GetLength(0))
            .Sum(column => Enumerable.Range(0, Matrix.GetLength(1))
                .Select(row => Matrix[row, column])
                .TakeWhile(value => value != 0)
                .Sum());
    }

    /*
     * Parses an input file into the list of graphs.
     */
    public static List<Graph> ParseInputFile(string pathToFile)
    {
        List<Graph> result = new();
        try
        {
            using var file = new StreamReader(pathToFile);
            int nGraphs = Convert.ToInt32(file.ReadLine());
            while (file.ReadLine() is { } ln)
            {
                if (ln == "")
                {
                    continue;
                }

                int n = Convert.ToInt32(ln);
                Graph g = new Graph(n);
                for (int i = 0; i < n; i++)
                {
                    var values = file.ReadLine()?.Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        int edges = int.Parse(values![j]);
                        g.EdgesCount += edges;
                        g.Matrix![i, j] = edges;
                    }
                }

                while (file.ReadLine() is { } ln2)
                {
                    if (ln2 == "")
                        break;
                    if (g.Description != "") g.Description += "\n";
                    g.Description += ln2;
                }

                g.BidirectionalMatrix = g.RemoveSingularEdges();
                result.Add(g);
            }

            file.Close();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Could not find the input file:\n{e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"There was a problem with parsing the input file:\n{e}");
            throw;
        }

        return result;
    }


    /*
     * Generates random graph
     */
    public static Graph GetRandomGraph(int n, float edgeProbability = 0.5f)
    {
        var r = new Random();
        Graph g = new Graph(n);
        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            if (i != j && r.NextSingle() <= edgeProbability)
            {
                int edges = r.Next(1, 9);
                g.EdgesCount+= edges;
                g.Matrix![i, j] = edges;
            } 
            else g.Matrix![i, j] = 0;
            
        g.BidirectionalMatrix = g.RemoveSingularEdges();
        return g;
    }
    
    /*
     * Generates random graph with certain number of cliques
     */
    public static Graph GetRandomGraphWithCliques(List<int> cliqueSizes, int n, float edgeProbability = 0.5f)
    {
        var r = new Random();
        Graph g = new Graph(n);
        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            if (i != j && r.NextSingle() <= edgeProbability)
            {
                int edges = r.Next(1, 9);
                g.EdgesCount+= edges;
                g.Matrix![i, j] = edges;
            } 
            else g.Matrix![i, j] = 0;

        foreach (int clique in cliqueSizes)
        {
            var vertices = Enumerable.Range(0, n).OrderBy(x => r.Next()).Take(clique).ToList();
            foreach (int i in vertices)
            foreach (int j in vertices)
            {
                if (i != j)
                {
                    if(g.Matrix![i, j] == 0)
                        g.EdgesCount++;
                    g.Matrix![i, j] = 1;
                }
            }
        }
        g.BidirectionalMatrix = g.RemoveSingularEdges();
        return g;
    }
    
    /*
     * Generates random graph with a subgraph of a certain size
     */
    public static Graph GetRandomGraphWithSubgraph(Graph sub, int n, float edgeProbability = 0.5f)
    {
        if (sub.VerticesCount > n) throw new ArgumentException("Subgraph should not be bigger than the graph");
        var r = new Random();
        Graph g = new Graph(n);
        var verts = Enumerable.Range(0, n).OrderBy(_ => r.Next()).Take(sub.VerticesCount).ToList();
        int l = 0;
        var map = verts.ToDictionary(e => e, _ => l++);
        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
        {
            if (map.ContainsKey(i) && map.ContainsKey(j))
            {
                int edges = sub.GetAt(map[i], map[j]);
                g.EdgesCount+= edges;
                g.Matrix![i, j] = edges;
            } else
            if (i != j && r.NextSingle() <= edgeProbability)
            {
                int edges = r.Next(0, 9);
                g.EdgesCount+= edges;
                g.Matrix![i, j] = edges;
            } 
            else g.Matrix![i, j] = 0;

        }
        g.BidirectionalMatrix = g.RemoveSingularEdges();
        return g;
    }

    /*
     * Removes one-way edges from the graph.
     */
    public int[,] RemoveSingularEdges()
    {
        if (Matrix == null)
            throw new Exception("The graph has not been properly initialized\n");

        int[,] filteredMatrix = new int[VerticesCount, VerticesCount];
        for (int i = 0; i < VerticesCount; ++i)
        {
            for (int j = 0; j < VerticesCount; ++j)
            {
                filteredMatrix[i, j] = Math.Min(Matrix[i, j], Matrix[j, i]);
            }
        }

        return filteredMatrix;
    }
    
    /*
     * Calculates the exact distance between two graphs
     */
    public static double ExactDistance(Graph g1, Graph g2)
    {
        return 1.0 - (double)GetMaxSubgraphSize(new BronKerboschMaximumClique(), g1, g2) / 
            Math.Max(g1.VerticesCount, g2.VerticesCount);
    }
    
    /*
     * Calculates the approximated distance between two graphs
     */
    public static double ApproxDistance(Graph g1, Graph g2)
    {
        return 1.0 - (double)GetMaxSubgraphSize(new GeneticAlgorithm(), g1, g2) / 
            Math.Max(g1.VerticesCount, g2.VerticesCount);
    }

    private static int GetMaxSubgraphSize(ICliqueAlgorithm cliqueAlgorithm, Graph g1, Graph g2)
    {
        return new SubgraphUsingClique(cliqueAlgorithm).Solve(g1, g2).Item1.Count;
    }

    public int GetAt(int column, int row)
    {
        return Matrix?[column, row] ?? throw new IndexOutOfRangeException();
    }

    public int GetAtBidirectional(int column, int row)
    {
        return BidirectionalMatrix?[column, row] ?? throw new IndexOutOfRangeException();
    }

    public bool IsBidirectionalEdge(int node1, int node2)
    {
        return GetAtBidirectional(node1,node2) >0;
    }

    public (int, int) Size()
    {
        return (VerticesCount, EdgesCount);
    }


    public HashSet<int> GetNeighboursBi(int v)
    {
        return Enumerable.Range(0, BidirectionalMatrix!.GetLength(1))
            .Where(x => BidirectionalMatrix[v, x] > 0)
            .ToHashSet();
    }

    public void PrintHighlighted(bool[,] hightlightMatrix)
    {
        if (Matrix == null)
        {
            Console.WriteLine("[]");
            return;
        }

        var color = Console.BackgroundColor;
        var fontColor = Console.ForegroundColor;
        if (VerticesCount <= 100)
        {
            for (int i = 0; i < VerticesCount; ++i)
            {
                for (int j = 0; j < VerticesCount; ++j)
                {
                    if (hightlightMatrix[i, j])
                    {
                        if(i == j) 
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        else
                            Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    };
                    Console.Write(Matrix[i, j] + " ");
                    Console.BackgroundColor = color;
                    Console.ForegroundColor = fontColor;
                }
                Console.WriteLine();
            }
        }
        Console.WriteLine($"Graph (V:{VerticesCount}, E:{EdgesCount})");
        Console.WriteLine($"Desc: \"{Description}\"");
    }

    public String Serialize()
    {
        if (Matrix == null) return "[]";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < VerticesCount; ++i)
        {
            for (int j = 0; j < VerticesCount; ++j)
            {
                sb.Append(Matrix[i, j] + " ");
            }

            sb.AppendLine();
        }
        sb.AppendLine(Description);
        return sb.ToString();
    }

    public override String ToString()
    {
        if (Matrix == null) return "[]";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Graph (V:{VerticesCount}, E:{EdgesCount})");
        if (VerticesCount > 50) return sb.ToString();
        sb.Append(Serialize());
        return sb.ToString();
    }
}
