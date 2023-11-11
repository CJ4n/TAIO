namespace TAIO_tests;

public class DistanceTest
{
    private List<Graph> graphs;

    [SetUp]
    public void SetUp()
    {
        graphs = Graph.ParseInputFile("graphs/data4.txt");
    }

    [Test]
    public void ShouldReturnZeroForIdenticalGraphs()
    {
        foreach (var g in graphs)
        {
            Assert.That(Graph.ExactDistance(g, g), Is.EqualTo(0));
            Assert.That(Graph.ApproxDistance(g, g), Is.EqualTo(0));
        }
    }
    
    [Test]
    public void ShouldBeSymmetric()
    {
        for (int i = 0; i < graphs.Count; i++)
        {
            for (int j = i + 1; j < graphs.Count; j++)
            {
                Assert.That(Graph.ExactDistance(graphs[i], graphs[j]), 
                    Is.EqualTo(Graph.ExactDistance(graphs[j], graphs[i])));
            }
        }
    }
    
    [Test]
    public void ShouldApproximateCorrectly()
    {
        for (int i = 0; i < graphs.Count; i++)
        {
            for (int j = i + 1; j < graphs.Count; j++)
            {
                Assert.That(Graph.ExactDistance(graphs[i], graphs[j]), 
                    Is.EqualTo(Graph.ApproxDistance(graphs[i], graphs[j])));
            }
        }
    }
}
