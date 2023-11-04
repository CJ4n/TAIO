using CsvHelper.Configuration;

namespace TAIO_tests;

public class ApproxRecord
{
    public int VertexCount { get; set; }
    public int EdgeCount { get; set; }
    public int MaximumPlantedCliqueSize { get; set; }
    public int MaximumCliqueApproximationSize { get; set; }
    public int ApproximationErrorToPlanted { get; set; }
    public double GeneticAlgorithmTime { get; set; }

    public ApproxRecord(int vertexCount, int edgeCount, int maximumPlantedCliqueSize, int maximumCliqueApproximationSize, int approximationErrorToPlanted, double geneticAlgorithmTime)
    {
        VertexCount = vertexCount;
        EdgeCount = edgeCount;
        MaximumPlantedCliqueSize = maximumPlantedCliqueSize;
        MaximumCliqueApproximationSize = maximumCliqueApproximationSize;
        GeneticAlgorithmTime = geneticAlgorithmTime;
        ApproximationErrorToPlanted = approximationErrorToPlanted;
    }
    
    public class ApproxRecordMap : ClassMap<ApproxRecord>
    {
        public ApproxRecordMap()
        {
            Map(m => m.VertexCount).Index(0).Name("v");
            Map(m => m.EdgeCount).Index(0).Name("e");
            Map(m => m.MaximumPlantedCliqueSize).Index(0).Name("maxPlantedClique");
            Map(m => m.MaximumCliqueApproximationSize).Index(0).Name("maxCliqueApproxGA");
            Map(m => m.ApproximationErrorToPlanted).Index(0).Name("errorToPlanted");
            Map(m => m.GeneticAlgorithmTime).Index(0).Name("ApproxTimeGA");

        }
    }
}
