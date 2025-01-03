using System;
using System.Linq;
using Avalonia;
using Flow.Core.Models;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class GraphCanvasViewModelTests
{
    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();

        // Act
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object, gameRegistry.Object);

        // Assert
        Assert.NotNull(viewModel.Nodes);
        Assert.Empty(viewModel.Nodes);
    }

    [Fact]
    public void AddRecipeNode_CreatesAndAddsRecipeNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = new Mock<IGraphManager>();
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object, gameRegistry.Object);
        var recipe = Flow.Tests.TestHelpers.MockFactory.CreateRecipe();
        var position = new Point(100, 100);

        var recipeNode = new RecipeNodeViewModel(graphManager.Object)
        {
            Recipe = recipe,
            Position = position
        };

        nodeFactory.Setup(f => f.CreateRecipeNode(recipe, graphManager.Object))
            .Returns(recipeNode);

        graphManager.Setup(m => m.AddNode(It.IsAny<NodeViewModel>()))
            .Callback<NodeViewModel>(node => viewModel.AddNode(node));

        // Act
        viewModel.SetContextMenuPosition(position);
        viewModel.AddRecipeNodeCommand.Execute(recipe);

        // Assert
        Assert.Contains(recipeNode, viewModel.Nodes);
        Assert.Equal(position, recipeNode.Position);
    }

    [Fact]
    public void AddGenericNode_CreatesAndAddsGenericNode()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = new Mock<IGraphManager>();
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var viewModel = new GraphCanvasViewModel(nodeFactory.Object, graphManager.Object, gameRegistry.Object);
        var position = new Point(100, 100);

        var genericNode = new NodeViewModel(graphManager.Object)
        {
            Position = position
        };

        nodeFactory.Setup(f => f.CreateNode(NodeType.Generic, graphManager.Object))
            .Returns(genericNode);

        graphManager.Setup(m => m.AddNode(It.IsAny<NodeViewModel>()))
            .Callback<NodeViewModel>(node => viewModel.AddNode(node));

        // Act
        viewModel.SetContextMenuPosition(position);
        viewModel.AddGenericNodeCommand.Execute(position);

        // Assert
        Assert.Contains(genericNode, viewModel.Nodes);
        Assert.Equal(position, genericNode.Position);
    }
} 