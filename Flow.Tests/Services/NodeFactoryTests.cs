using Flow.Core.Models;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.Services;

public class NodeFactoryTests
{
    private readonly NodeFactory _factory;

    public NodeFactoryTests()
    {
        _factory = new NodeFactory();
    }

    [Fact]
    public void CreateNode_ShouldCreateNodeWithCorrectType()
    {
        // Arrange
        var type = NodeType.Generic;

        // Act
        var node = _factory.CreateNode(type);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(type, node.NodeType);
        Assert.Equal($"New {type} Node", node.Title);
    }

    [Fact]
    public void CreateRecipeNode_ShouldCreateNodeWithCorrectRecipe()
    {
        // Arrange
        var inputItem = new Item("input", "Input Item");
        var outputItem = new Item("output", "Output Item");
        var machine = new Machine("machine", "Test Machine", 100);
        var recipe = new Recipe(
            "test",
            "Test Recipe",
            new[] { new ItemStack(inputItem, 1) },
            new[] { new ItemStack(outputItem, 1) },
            machine,
            TimeSpan.FromSeconds(1));

        // Act
        var node = _factory.CreateRecipeNode(recipe);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(NodeType.Recipe, node.NodeType);
        Assert.Equal(recipe.DisplayName, node.Title);
        Assert.Equal(recipe, node.Recipe);
        Assert.Single(node.InputConnectors);
        Assert.Single(node.OutputConnectors);
    }

    [Fact]
    public void CreateRecipeNode_WhenRecipeIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _factory.CreateRecipeNode(null!));
    }
} 