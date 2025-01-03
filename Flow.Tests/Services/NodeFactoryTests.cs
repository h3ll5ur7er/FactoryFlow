using System;
using Flow.Core.Models;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.Services;

public class NodeFactoryTests
{
    [Fact]
    public void CreateNode_ShouldCreateNodeWithCorrectType()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var factory = new NodeFactory();

        // Act
        var node = factory.CreateNode(NodeType.Generic, graphManager.Object);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(NodeType.Generic, node.NodeType);
    }

    [Fact]
    public void CreateRecipeNode_ShouldCreateNodeWithRecipe()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var factory = new NodeFactory();
        var ironOre = new Item("iron-ore", "Iron Ore");
        var ironPlate = new Item("iron-plate", "Iron Plate");
        var furnace = new Machine("stone-furnace", "Stone Furnace", 50m);
        var recipe = new Recipe(
            "iron-smelting",
            "Iron Smelting",
            new[] { new ItemStack(ironOre, 1) },
            new[] { new ItemStack(ironPlate, 1) },
            furnace,
            TimeSpan.FromSeconds(3.5)
        );

        // Act
        var node = factory.CreateRecipeNode(recipe, graphManager.Object);

        // Assert
        Assert.NotNull(node);
        Assert.Equal(NodeType.Recipe, node.NodeType);
        Assert.Equal(recipe, node.Recipe);
    }

    [Fact]
    public void CreateRecipeNode_WithNullRecipe_ShouldThrowArgumentNullException()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var factory = new NodeFactory();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => factory.CreateRecipeNode(null!, graphManager.Object));
    }
} 