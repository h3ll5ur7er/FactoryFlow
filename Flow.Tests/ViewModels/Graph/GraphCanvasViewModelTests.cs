using System;
using System.Linq;
using Avalonia;
using Flow.Core.Models;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class GraphCanvasViewModelTests
{
    private static Recipe CreateTestRecipe()
    {
        var inputItem = new Item("input", "Input Item");
        var outputItem = new Item("output", "Output Item");
        var machine = new Machine("machine", "Test Machine", 100);
        
        return new Recipe(
            "test",
            "Test Recipe",
            new[] { new ItemStack(inputItem, 1) },
            new[] { new ItemStack(outputItem, 1) },
            machine,
            TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var canvas = new GraphCanvasViewModel();

        // Assert
        Assert.Empty(canvas.Nodes);
        Assert.Equal(1.0, canvas.Scale);
        Assert.Equal(new Point(), canvas.Offset);
        Assert.Null(canvas.SelectedNode);
    }

    [Fact]
    public void AddNode_WhenNodeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => canvas.AddNode(null!));
    }

    [Fact]
    public void AddNode_WhenNodeIsValid_ShouldAddToNodes()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());

        // Act
        canvas.AddNode(node);

        // Assert
        Assert.Single(canvas.Nodes);
        Assert.Contains(node, canvas.Nodes);
    }

    [Fact]
    public void RemoveNode_WhenNodeIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => canvas.RemoveNode(null!));
    }

    [Fact]
    public void RemoveNode_WhenNodeExists_ShouldRemoveFromNodes()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());
        canvas.AddNode(node);

        // Act
        var result = canvas.RemoveNode(node);

        // Assert
        Assert.True(result);
        Assert.Empty(canvas.Nodes);
    }

    [Fact]
    public void RemoveNode_WhenNodeDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());

        // Act
        var result = canvas.RemoveNode(node);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveNode_WhenNodeIsSelected_ShouldClearSelection()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());
        canvas.AddNode(node);
        canvas.SelectNode(node);

        // Act
        canvas.RemoveNode(node);

        // Assert
        Assert.Null(canvas.SelectedNode);
    }

    [Fact]
    public void SelectNode_WhenNodeIsNull_ShouldClearSelection()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());
        canvas.AddNode(node);
        canvas.SelectNode(node);

        // Act
        canvas.SelectNode(null);

        // Assert
        Assert.Null(canvas.SelectedNode);
        Assert.False(node.IsSelected);
    }

    [Fact]
    public void SelectNode_WhenNodeExists_ShouldSelectNode()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());
        canvas.AddNode(node);

        // Act
        canvas.SelectNode(node);

        // Assert
        Assert.Equal(node, canvas.SelectedNode);
        Assert.True(node.IsSelected);
    }

    [Fact]
    public void SelectNode_WhenDifferentNodeWasSelected_ShouldUpdateSelection()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node1 = new RecipeNodeViewModel(CreateTestRecipe());
        var node2 = new RecipeNodeViewModel(CreateTestRecipe());
        canvas.AddNode(node1);
        canvas.AddNode(node2);
        canvas.SelectNode(node1);

        // Act
        canvas.SelectNode(node2);

        // Assert
        Assert.Equal(node2, canvas.SelectedNode);
        Assert.True(node2.IsSelected);
        Assert.False(node1.IsSelected);
    }

    [Fact]
    public void SelectNode_WhenNodeDoesNotExist_ShouldThrowArgumentException()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var node = new RecipeNodeViewModel(CreateTestRecipe());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => canvas.SelectNode(node));
    }

    [Fact]
    public void RemoveNode_WhenNodeHasConnections_ShouldDisconnectAll()
    {
        // Arrange
        var canvas = new GraphCanvasViewModel();
        var recipe1 = CreateTestRecipe();
        var recipe2 = CreateTestRecipe();
        var node1 = new RecipeNodeViewModel(recipe1);
        var node2 = new RecipeNodeViewModel(recipe2);
        canvas.AddNode(node1);
        canvas.AddNode(node2);

        var output = node1.OutputConnectors.First();
        var input = node2.InputConnectors.First();
        output.TryConnect(input);

        // Act
        canvas.RemoveNode(node1);

        // Assert
        Assert.False(input.IsConnected);
        Assert.Empty(input.Connections);
    }
} 