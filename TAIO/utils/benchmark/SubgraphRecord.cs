using CsvHelper.Configuration;

namespace TAIO_tests;

public class SubgraphRecord
{
    public int VertexCount1 { get; set; }
    public int VertexCount2 { get; set; }
    public int EdgeCount1 { get; set; }
    public int EdgeCount2 { get; set; }
    public int MaximumCommonSubgraphSize { get; set; }
    public int MaximumCommonSubgraphApproximationSize { get; set; }
    public int ApproximationError { get; set; }
    public double BronKerboschTime { get; set; }
    public double GeneticAlgorithmTime { get; set; }

    public SubgraphRecord(int vertexCount1, int edgeCount1, int vertexCount2, int edgeCount2, int maximumCommonSubgraphSize, int maximumCommonSubgraphApproximationSize, int approximationError, double bronKerboschTime, double geneticAlgorithmTime)
    {
        VertexCount1 = vertexCount1;
        EdgeCount1 = edgeCount1;
        MaximumCommonSubgraphSize = maximumCommonSubgraphSize;
        MaximumCommonSubgraphApproximationSize = maximumCommonSubgraphApproximationSize;
        BronKerboschTime = bronKerboschTime;
        GeneticAlgorithmTime = geneticAlgorithmTime;
        VertexCount2 = vertexCount2;
        EdgeCount2 = edgeCount2;
        ApproximationError = approximationError;
    }
    
    public class SubgraphRecordMap : ClassMap<SubgraphRecord>
    {
        public SubgraphRecordMap()
        {
            Map(m => m.VertexCount1).Index(0).Name("v_1");
            Map(m => m.VertexCount2).Index(0).Name("v_2");
            Map(m => m.EdgeCount1).Index(0).Name("e_1");
            Map(m => m.EdgeCount2).Index(0).Name("e_2");
            Map(m => m.MaximumCommonSubgraphSize).Index(0).Name("maxSubgraphBK");
            Map(m => m.MaximumCommonSubgraphApproximationSize).Index(0).Name("maxSubgraphApproxGA");
            Map(m => m.ApproximationError).Index(0).Name("error");
            Map(m => m.BronKerboschTime).Index(0).Name("ExactTimeBK");
            Map(m => m.GeneticAlgorithmTime).Index(0).Name("ApproxTimeGA");

        }
    }
}
