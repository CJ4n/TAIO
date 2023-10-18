public class Graph {
        public int verticesCount {get; set;}
        public int edgesCount {get; set;}
        public int[,]? matrix {set; get;}

        // simetrical matrix of maximal bidirectional subgraph
        public int[,]? bidirectionalMatrix{get; set;}

        public Graph(String pathToFile)
        {
            throw new NotImplementedException();
        }

        // permutate 2 vertices index1 and index2
        public void permutate(int index1, int index2)
        {
            throw new NotImplementedException();
        }

        public int getAt(int column, int row) 
        {
            return this.matrix != null ? this.matrix[column, row] : throw new IndexOutOfRangeException();
        }

        public int getAtBidirectional(int column, int row) 
        {
            return this.bidirectionalMatrix != null ? this.bidirectionalMatrix[column, row] : throw new IndexOutOfRangeException();
        }

        public int sieze() 
        {
            return verticesCount + edgesCount;
        }
}