using System.Text;
using TAIO;

public class Graph
{
    public int VerticesCount { get; set; }
    public int EdgesCount { get; set; }
    public int[,]? Matrix { get; set; }

    // symmetrical matrix of maximal bidirectional subgraph
    private int[,]? BidirectionalMatrix { get; set; }

    public Graph(int verticesCount)
    {
        VerticesCount = verticesCount;
        Matrix = new int[verticesCount, verticesCount];
    }

    public Graph(int[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
            throw new ArgumentException("Adjacency matrix has to be a square");
        VerticesCount = matrix.GetLength(0);
        Matrix = matrix;
        EdgesCount = CountEdges();
        BidirectionalMatrix = RemoveSingularEdges();
    }

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
                    var values = (file.ReadLine()?.Split(' '));
                    for (int j = 0; j < n; j++)
                    {
                        int edges = int.Parse(values![j]);
                        g.EdgesCount += edges;
                        g.Matrix![i, j] = edges;
                    }
                }

                g.BidirectionalMatrix = g.RemoveSingularEdges();
                result.Add(g);
            }

            file.Close();
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
                g.EdgesCount++;
                g.Matrix![i, j] = 1;
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
                g.EdgesCount++;
                g.Matrix![i, j] = 1;
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
     * Removes one-way edges from the graph. Can be used as a preprocessing for finding a clique in graph.
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
        return new SubgraphUsingClique(cliqueAlgorithm).Solve(g1, g2).Count;
    }
    

    /*
     * Permutes two vertices index1 and index2
     */
    public void Permute(int index1, int index2)
    {
        throw new NotImplementedException();
    }

    public int GetAt(int column, int row)
    {
        return this.Matrix?[column, row] ?? throw new IndexOutOfRangeException();
    }

    public int GetAtBidirectional(int column, int row)
    {
        return this.BidirectionalMatrix?[column, row] ?? throw new IndexOutOfRangeException();
    }

    public bool IsBidirectionalEdge(int node1, int node2)
    {
        return this.GetAtBidirectional(node1,node2) >0;
    }

    public int Size()
    {
        return VerticesCount + EdgesCount;
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
        Console.WriteLine($"Graph V:{VerticesCount}, E:{EdgesCount}");
        for (int i = 0; i < VerticesCount; ++i)
        {
            for (int j = 0; j < VerticesCount; ++j)
            {
                if (hightlightMatrix[i, j] == true)
                {
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

        return sb.ToString();
    }

    public override String ToString()
    {
        if (Matrix == null) return "[]";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Graph V:{VerticesCount}, E:{EdgesCount}");
        if (VerticesCount > 50) return sb.ToString();
        sb.Append(Serialize());
        return sb.ToString();
    }
}
