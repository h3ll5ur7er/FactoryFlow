using System;
using System.Linq;
using Flow.Core.Models;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class RecipeNodeViewModelTests
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
    public void Constructor_ShouldInitializeWithRecipe()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNodeViewModel(recipe);

        // Assert
        Assert.Equal(recipe, node.Recipe);
        Assert.Equal(NodeType.Recipe, node.NodeType);
        Assert.Equal(recipe.DisplayName, node.Title);
        Assert.Single(node.InputConnectors);
        Assert.Single(node.OutputConnectors);
    }

    [Fact]
    public void Constructor_WhenRecipeIsNull_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new RecipeNodeViewModel(null!));
    }

    [Fact]
    public void InputConnectors_ShouldMatchRecipeInputs()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNodeViewModel(recipe);

        // Assert
        var connector = Assert.Single(node.InputConnectors);
        Assert.Equal(ConnectorType.Input, connector.Type);
        Assert.False(connector.AllowMultipleConnections);
        Assert.Single(connector.AcceptedTypes);
        Assert.Contains(recipe.Inputs.First().Item.GetType(), connector.AcceptedTypes);
    }

    [Fact]
    public void OutputConnectors_ShouldMatchRecipeOutputs()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNodeViewModel(recipe);

        // Assert
        var connector = Assert.Single(node.OutputConnectors);
        Assert.Equal(ConnectorType.Output, connector.Type);
        Assert.True(connector.AllowMultipleConnections);
        Assert.Single(connector.AcceptedTypes);
        Assert.Contains(recipe.Outputs.First().Item.GetType(), connector.AcceptedTypes);
    }

    [Fact]
    public void Recipe_WhenSet_ShouldUpdateConnectors()
    {
        // Arrange
        var recipe1 = CreateTestRecipe();
        var node = new RecipeNodeViewModel(recipe1);
        
        var inputItem2 = new Item("input2", "Input Item 2");
        var outputItem2 = new Item("output2", "Output Item 2");
        var machine2 = new Machine("machine2", "Test Machine 2", 200);
        var recipe2 = new Recipe(
            "test2",
            "Test Recipe 2",
            new[] { new ItemStack(inputItem2, 1) },
            new[] { new ItemStack(outputItem2, 1) },
            machine2,
            TimeSpan.FromSeconds(1));

        // Act
        node.Recipe = recipe2;

        // Assert
        Assert.Equal(recipe2, node.Recipe);
        Assert.Equal(recipe2.DisplayName, node.Title);
        
        var inputConnector = Assert.Single(node.InputConnectors);
        Assert.Contains(inputItem2.GetType(), inputConnector.AcceptedTypes);
        
        var outputConnector = Assert.Single(node.OutputConnectors);
        Assert.Contains(outputItem2.GetType(), outputConnector.AcceptedTypes);
    }

    [Fact]
    public void Recipe_WhenSetToNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new RecipeNodeViewModel(CreateTestRecipe());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.Recipe = null!);
    }

    [Fact]
    public void Recipe_WhenSet_ShouldDisconnectExistingConnectors()
    {
        // Arrange
        var recipe1 = CreateTestRecipe();
        var node = new RecipeNodeViewModel(recipe1);
        var inputConnector = node.InputConnectors.First();
        var outputConnector = node.OutputConnectors.First();
        
        var otherInput = new ConnectorViewModel(ConnectorType.Input);
        var otherOutput = new ConnectorViewModel(ConnectorType.Output);
        
        outputConnector.TryConnect(otherInput);
        otherOutput.TryConnect(inputConnector);
        
        var inputItem2 = new Item("input2", "Input Item 2");
        var outputItem2 = new Item("output2", "Output Item 2");
        var machine2 = new Machine("machine2", "Test Machine 2", 200);
        var recipe2 = new Recipe(
            "test2",
            "Test Recipe 2",
            new[] { new ItemStack(inputItem2, 1) },
            new[] { new ItemStack(outputItem2, 1) },
            machine2,
            TimeSpan.FromSeconds(1));

        // Act
        node.Recipe = recipe2;

        // Assert
        Assert.Empty(otherInput.Connections);
        Assert.Empty(otherOutput.Connections);
        Assert.Empty(node.InputConnectors.First().Connections);
        Assert.Empty(node.OutputConnectors.First().Connections);
    }
} 