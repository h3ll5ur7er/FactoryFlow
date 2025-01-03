using System;
using System.Threading.Tasks;
using Avalonia;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;
using Flow.Tests.TestHelpers;

namespace Flow.Tests.Services;

public class UIActionManagerTests
{
    [Fact]
    public async Task ExecuteActionAsync_ShouldExecuteRegisteredAction()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var actionExecuted = false;
        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            async () =>
            {
                actionExecuted = true;
                return true;
            });

        // Act
        await manager.ExecuteActionAsync("test");

        // Assert
        Assert.True(actionExecuted);
    }

    [Fact]
    public async Task CanExecuteActionAsync_ShouldReturnTrueForRegisteredAction()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            async () => true);

        // Act
        var result = await manager.CanExecuteActionAsync("test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RegisterAction_ShouldAddActionToManager()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);

        // Act
        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            async () => true);

        // Assert
        Assert.True(await manager.CanExecuteActionAsync("test"));
    }

    [Fact]
    public async Task UnregisterAction_ShouldRemoveActionFromManager()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            async () => true);

        // Act
        manager.UnregisterAction("test");

        // Assert
        Assert.False(await manager.CanExecuteActionAsync("test"));
    }

    [Fact]
    public async Task AddNodeAsync_ShouldCreateAndAddNodeToGraph()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var position = new Point(100, 100);

        // Act
        await manager.AddNodeAsync(position);

        // Assert
        nodeFactory.Verify(f => f.CreateNode(NodeType.Generic, graphManager.Object), Times.Once);
        graphManager.Verify(m => m.AddNode(It.IsAny<NodeViewModel>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSelectedNodesAsync_ShouldRemoveSelectedNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var node = nodeFactory.Object.CreateNode(NodeType.Generic, graphManager.Object);
        var graph = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);
        graphManager.Setup(m => m.CurrentGraph).Returns(graph);
        graph.AddNode(node);
        graph.SelectedNode = node;

        // Act
        await manager.DeleteSelectedNodesAsync();

        // Assert
        graphManager.Verify(m => m.RemoveNode(node), Times.Once);
    }
} 