using System.Text;

public class Graph {
        public int VerticesCount { get; }
        private int EdgesCount { get; set; }
        private int[,]? Matrix { get; set; }

        // symmetrical matrix of maximal bidirectional subgraph
        private int[,]? BidirectionalMatrix { get; set; }

        private Graph(int verticesCount)
        {
            VerticesCount = verticesCount;
            Matrix = new int[verticesCount, verticesCount];
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
                while (file.ReadLine() is { } ln) {
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
                            g.Matrix![i, j] = int.Parse(values![j]);
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
        public static Graph GetRandomGraph(int n)
        {
            var r = new Random();
            Graph g = new Graph(n);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i == j)
                        g.Matrix![i, j] = 0;
                    else
                        g.Matrix![i, j] = r.Next(0, 2);

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
            
            int[,] filteredMatrix = new int[VerticesCount,VerticesCount];
            for (int i = 0; i < VerticesCount; ++i)
            {
                for (int j = 0; j < VerticesCount; ++j)
                {
                    filteredMatrix[i, j] = Matrix[i, j] & Matrix[j, i];
                }
            }

            return filteredMatrix;
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

        public override String ToString()
        {
            if (Matrix == null) return "[]";
            if(VerticesCount > 50) return $"Graph V:{VerticesCount}, E:{EdgesCount}";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < VerticesCount; ++i)
            {
                for (int j = 0; j < VerticesCount; ++j)
                {
                    sb.Append(Matrix![i, j] + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
}
