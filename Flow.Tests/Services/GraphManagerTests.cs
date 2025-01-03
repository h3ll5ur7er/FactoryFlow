using Flow.Core.Services;
using Flow.Tests.TestHelpers;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.Services;

public class GraphManagerTests
{
    [Fact]
    public void AddNode_AddsNodeToGraph()
    {
        // Arrange
        var nodeFactory = MockFactory.CreateNodeFactory();
        var gameRegistry = MockFactory.CreateGameRegistry();
        var graphManager = new GraphManager(nodeFactory.Object, gameRegistry.Object);
        var node = new NodeViewModel(graphManager);

        // Act
        graphManager.AddNode(node);

        // Assert
        Assert.NotNull(graphManager.CurrentGraph);
        Assert.Contains(node, graphManager.CurrentGraph.Nodes);
    }

    [Fact]
    public void RemoveNode_RemovesNodeFromGraph()
    {
        // Arrange
        var nodeFactory = MockFactory.CreateNodeFactory();
        var gameRegistry = MockFactory.CreateGameRegistry();
        var graphManager = new GraphManager(nodeFactory.Object, gameRegistry.Object);
        var node = new NodeViewModel(graphManager);
        graphManager.AddNode(node);

        // Act
        graphManager.RemoveNode(node);

        // Assert
        Assert.NotNull(graphManager.CurrentGraph);
        Assert.DoesNotContain(node, graphManager.CurrentGraph.Nodes);
    }

    [Fact]
    public void ClearGraph_RemovesAllNodes()
    {
        // Arrange
        var nodeFactory = MockFactory.CreateNodeFactory();
        var gameRegistry = MockFactory.CreateGameRegistry();
        var graphManager = new GraphManager(nodeFactory.Object, gameRegistry.Object);
        var node1 = new NodeViewModel(graphManager);
        var node2 = new NodeViewModel(graphManager);
        graphManager.AddNode(node1);
        graphManager.AddNode(node2);

        // Act
        graphManager.ClearGraph();

        // Assert
        Assert.NotNull(graphManager.CurrentGraph);
        Assert.Empty(graphManager.CurrentGraph.Nodes);
    }

    [Fact]
    public void CurrentGraph_ReturnsGraphCanvasViewModel()
    {
        // Arrange
        var nodeFactory = MockFactory.CreateNodeFactory();
        var gameRegistry = MockFactory.CreateGameRegistry();
        var graphManager = new GraphManager(nodeFactory.Object, gameRegistry.Object);

        // Act
        var graph = graphManager.CurrentGraph;

        // Assert
        Assert.NotNull(graph);
        Assert.IsType<GraphCanvasViewModel>(graph);
    }
} 