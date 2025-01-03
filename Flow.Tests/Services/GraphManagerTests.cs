using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.Services;

public class GraphManagerTests
{
    private readonly GraphManager _manager;

    public GraphManagerTests()
    {
        _manager = new GraphManager();
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyGraph()
    {
        // Assert
        Assert.NotNull(_manager.CurrentGraph);
        Assert.Empty(_manager.CurrentGraph.Nodes);
    }

    [Fact]
    public void CreateNewGraph_ShouldCreateNewEmptyGraph()
    {
        // Arrange
        var node = new NodeViewModel { Title = "Test Node" };
        _manager.AddNode(node);
        Assert.Single(_manager.CurrentGraph.Nodes);

        // Act
        _manager.CreateNewGraph();

        // Assert
        Assert.NotNull(_manager.CurrentGraph);
        Assert.Empty(_manager.CurrentGraph.Nodes);
    }

    [Fact]
    public void AddNode_ShouldAddNodeToCurrentGraph()
    {
        // Arrange
        var node = new NodeViewModel { Title = "Test Node" };

        // Act
        _manager.AddNode(node);

        // Assert
        Assert.Single(_manager.CurrentGraph.Nodes);
        Assert.Contains(node, _manager.CurrentGraph.Nodes);
    }

    [Fact]
    public void AddNode_WhenNodeIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _manager.AddNode(null!));
    }

    [Fact]
    public void RemoveNode_ShouldRemoveNodeFromCurrentGraph()
    {
        // Arrange
        var node = new NodeViewModel { Title = "Test Node" };
        _manager.AddNode(node);
        Assert.Single(_manager.CurrentGraph.Nodes);

        // Act
        _manager.RemoveNode(node);

        // Assert
        Assert.Empty(_manager.CurrentGraph.Nodes);
    }

    [Fact]
    public void RemoveNode_WhenNodeIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _manager.RemoveNode(null!));
    }

    [Fact]
    public void ClearGraph_ShouldRemoveAllNodes()
    {
        // Arrange
        _manager.AddNode(new NodeViewModel { Title = "Node 1" });
        _manager.AddNode(new NodeViewModel { Title = "Node 2" });
        Assert.Equal(2, _manager.CurrentGraph.Nodes.Count);

        // Act
        _manager.ClearGraph();

        // Assert
        Assert.Empty(_manager.CurrentGraph.Nodes);
    }
} 