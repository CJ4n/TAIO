using CsvHelper.Configuration;

namespace TAIO_tests;

public class CliqueRecord
{
    public int VertexCount { get; set; }
    public int EdgeCount { get; set; }
    public int MaximumCliqueSize { get; set; }
    public int MaximumCliqueApproximationSize { get; set; }
    public int ApproximationError { get; set; }
    public double BronKerboschTime { get; set; }
    public double GeneticAlgorithmTime { get; set; }

    public CliqueRecord(int vertexCount, int edgeCount, int maximumCliqueSize, int maximumCliqueApproximationSize, int approximationError, double bronKerboschTime, double geneticAlgorithmTime)
    {
        VertexCount = vertexCount;
        EdgeCount = edgeCount;
        MaximumCliqueSize = maximumCliqueSize;
        MaximumCliqueApproximationSize = maximumCliqueApproximationSize;
        BronKerboschTime = bronKerboschTime;
        GeneticAlgorithmTime = geneticAlgorithmTime;
        ApproximationError = approximationError;
    }
    
    public class CliqueRecordMap : ClassMap<CliqueRecord>
    {
        public CliqueRecordMap()
        {
            Map(m => m.VertexCount).Index(0).Name("v");
            Map(m => m.EdgeCount).Index(0).Name("e");
            Map(m => m.MaximumCliqueSize).Index(0).Name("maxCliqueBK");
            Map(m => m.MaximumCliqueApproximationSize).Index(0).Name("maxCliqueApproxGA");
            Map(m => m.ApproximationError).Index(0).Name("error");
            Map(m => m.BronKerboschTime).Index(0).Name("ExactTimeBK");
            Map(m => m.GeneticAlgorithmTime).Index(0).Name("ApproxTimeGA");
        }
    }
}
