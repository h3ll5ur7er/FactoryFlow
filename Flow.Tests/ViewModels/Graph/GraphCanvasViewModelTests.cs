using System;
using System.Linq;
using Flow.Core.Models;
using Flow.ViewModels.Graph;
using Xunit;
using Flow.Core.Services;
using Flow.Tests.TestHelpers;
using Moq;

namespace Flow.Tests.ViewModels.Graph;

public class GraphCanvasViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();

        // Act
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);

        // Assert
        Assert.NotNull(viewModel.Nodes);
        Assert.Empty(viewModel.Nodes);
        Assert.Null(viewModel.SelectedNode);
    }

    [Fact]
    public void AddRecipeNode_ShouldCreateAndAddRecipeNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);

        // Act
        viewModel.AddRecipeNodeCommand.Execute(null);

        // Assert
        nodeFactory.Verify(f => f.CreateRecipeNode(It.IsAny<Recipe>(), graphManager.Object), Times.Once);
        graphManager.Verify(m => m.AddNode(It.IsAny<NodeViewModel>()), Times.Once);
    }

    [Fact]
    public void AddGenericNode_ShouldCreateAndAddGenericNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);

        // Act
        viewModel.AddGenericNodeCommand.Execute(null);

        // Assert
        nodeFactory.Verify(f => f.CreateNode(NodeType.Generic, graphManager.Object), Times.Once);
        graphManager.Verify(m => m.AddNode(It.IsAny<NodeViewModel>()), Times.Once);
    }

    [Fact]
    public void AddSplergerNode_ShouldCreateAndAddSplergerNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);

        // Act
        viewModel.AddSplergerNodeCommand.Execute(null);

        // Assert
        nodeFactory.Verify(f => f.CreateNode(NodeType.Splerger, graphManager.Object), Times.Once);
        graphManager.Verify(m => m.AddNode(It.IsAny<NodeViewModel>()), Times.Once);
    }

    [Fact]
    public void AddNode_ShouldAddNodeToCollection()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object);
        var node = nodeFactory.Object.CreateNode(NodeType.Generic, graphManager.Object);

        // Act
        viewModel.AddNode(node);

        // Assert
        Assert.Single(viewModel.Nodes);
        Assert.Equal(node, viewModel.Nodes.First());
    }
} 