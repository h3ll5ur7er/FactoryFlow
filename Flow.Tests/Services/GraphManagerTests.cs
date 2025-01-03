using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.Services;

public class GraphManagerTests
{
    [Fact]
    public void Constructor_ShouldCreateNewGraph()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();

        // Act
        var manager = new GraphManager(nodeFactory.Object);

        // Assert
        Assert.NotNull(manager.CurrentGraph);
    }

    [Fact]
    public void AddNode_ShouldAddNodeToCurrentGraph()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var manager = new GraphManager(nodeFactory.Object);
        var node = nodeFactory.Object.CreateNode(NodeType.Generic, manager);

        // Act
        manager.AddNode(node);

        // Assert
        Assert.Contains(node, manager.CurrentGraph!.Nodes);
    }

    [Fact]
    public void RemoveNode_ShouldRemoveNodeFromCurrentGraph()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var manager = new GraphManager(nodeFactory.Object);
        var node = nodeFactory.Object.CreateNode(NodeType.Generic, manager);
        manager.AddNode(node);

        // Act
        manager.RemoveNode(node);

        // Assert
        Assert.DoesNotContain(node, manager.CurrentGraph!.Nodes);
    }

    [Fact]
    public void ClearGraph_ShouldRemoveAllNodes()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var manager = new GraphManager(nodeFactory.Object);
        var node1 = nodeFactory.Object.CreateNode(NodeType.Generic, manager);
        var node2 = nodeFactory.Object.CreateNode(NodeType.Generic, manager);
        manager.AddNode(node1);
        manager.AddNode(node2);

        // Act
        manager.ClearGraph();

        // Assert
        Assert.Empty(manager.CurrentGraph!.Nodes);
    }
} 