using System;
using System.Threading.Tasks;
using Avalonia;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.Services;

public class UIActionManagerTests
{
    private readonly Mock<IGraphManager> _graphManager;
    private readonly Mock<INodeFactory> _nodeFactory;
    private readonly UIActionManager _manager;

    public UIActionManagerTests()
    {
        _graphManager = new Mock<IGraphManager>();
        _nodeFactory = new Mock<INodeFactory>();
        _manager = new UIActionManager(_graphManager.Object, _nodeFactory.Object);
    }

    [Fact]
    public async Task RegisterAction_ShouldRegisterActionForExecution()
    {
        // Arrange
        var executed = false;
        _manager.RegisterAction("test", () =>
        {
            executed = true;
            return Task.CompletedTask;
        });

        // Act
        await _manager.ExecuteActionAsync("test");

        // Assert
        Assert.True(executed);
    }

    [Fact]
    public async Task RegisterAction_WithCanExecute_ShouldCheckBeforeExecution()
    {
        // Arrange
        var executed = false;
        var canExecute = false;
        _manager.RegisterAction(
            "test",
            () =>
            {
                executed = true;
                return Task.CompletedTask;
            },
            () => Task.FromResult(canExecute)
        );

        // Act - Try to execute when canExecute is false
        await _manager.ExecuteActionAsync("test");
        Assert.False(executed);

        // Act - Try to execute when canExecute is true
        canExecute = true;
        await _manager.ExecuteActionAsync("test");
        Assert.True(executed);
    }

    [Fact]
    public async Task UnregisterAction_ShouldPreventExecution()
    {
        // Arrange
        var executed = false;
        _manager.RegisterAction("test", () =>
        {
            executed = true;
            return Task.CompletedTask;
        });

        // Act
        _manager.UnregisterAction("test");
        await _manager.ExecuteActionAsync("test");

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task AddNodeAsync_ShouldCreateAndAddNodeToGraph()
    {
        // Arrange
        var position = new Point(100, 100);
        var node = new NodeViewModel { Title = "Test Node" };
        _nodeFactory.Setup(f => f.CreateNode(NodeType.Generic)).Returns(node);

        // Act
        await _manager.AddNodeAsync(position);

        // Assert
        _nodeFactory.Verify(f => f.CreateNode(NodeType.Generic), Times.Once);
        _graphManager.Verify(g => g.AddNode(node), Times.Once);
        Assert.Equal(position, node.Position);
    }

    [Fact]
    public async Task DeleteSelectedNodesAsync_ShouldRemoveSelectedNode()
    {
        // Arrange
        var node = new NodeViewModel { Title = "Test Node" };
        var graph = new GraphCanvasViewModel();
        graph.AddNode(node);
        graph.SelectNode(node);

        _graphManager.Setup(g => g.CurrentGraph).Returns(graph);

        // Act
        await _manager.DeleteSelectedNodesAsync();

        // Assert
        _graphManager.Verify(g => g.RemoveNode(node), Times.Once);
    }

    [Fact]
    public async Task DeleteSelectedNodesAsync_WhenNoSelection_ShouldDoNothing()
    {
        // Arrange
        var graph = new GraphCanvasViewModel();
        _graphManager.Setup(g => g.CurrentGraph).Returns(graph);

        // Act
        await _manager.DeleteSelectedNodesAsync();

        // Assert
        _graphManager.Verify(g => g.RemoveNode(It.IsAny<NodeViewModel>()), Times.Never);
    }
} 