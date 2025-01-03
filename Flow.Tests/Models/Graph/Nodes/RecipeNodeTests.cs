using Flow.Core.Models;
using Flow.Core.Models.Graph;
using Flow.Core.Models.Graph.Nodes;
using Xunit;

namespace Flow.Tests.Models.Graph.Nodes;

public class RecipeNodeTests
{
    private static Recipe CreateTestRecipe()
    {
        var ironOre = new Item("iron-ore", "Iron Ore");
        var ironPlate = new Item("iron-plate", "Iron Plate");
        var furnace = new Machine("stone-furnace", "Stone Furnace", 50m);

        return new Recipe(
            "iron-smelting",
            "Iron Smelting",
            new[] { new ItemStack(ironOre, 1) },
            new[] { new ItemStack(ironPlate, 1) },
            furnace,
            TimeSpan.FromSeconds(3.5)
        );
    }

    [Fact]
    public void Constructor_WithValidRecipe_CreatesNode()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNode(recipe);

        // Assert
        Assert.Equal($"recipe-{recipe.Identifier}", node.Identifier);
        Assert.Equal(recipe.DisplayName, node.DisplayName);
        Assert.Equal(recipe, node.Recipe);
        Assert.Equal(1, node.Multiplier);
        Assert.Equal(recipe.Inputs.Count(), node.Inputs.Count);
        Assert.Equal(recipe.Outputs.Count(), node.Outputs.Count);
    }

    [Fact]
    public void Constructor_WithNullRecipe_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new RecipeNode(null!));
        Assert.Equal("recipe", exception.ParamName);
    }

    [Fact]
    public void Constructor_CreatesInputConnectorsForRecipeInputs()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNode(recipe);

        // Assert
        var input = Assert.Single(node.Inputs);
        var recipeInput = Assert.Single(recipe.Inputs);
        Assert.Equal($"input-{recipeInput.Item.Identifier}", input.Identifier);
        Assert.Equal(recipeInput.Item.DisplayName, input.DisplayName);
        Assert.True(input.IsInput);
        Assert.True(input.AllowsMultipleConnections);
        Assert.Contains(recipeInput.Item, input.AcceptedItems);
    }

    [Fact]
    public void Constructor_CreatesOutputConnectorsForRecipeOutputs()
    {
        // Arrange
        var recipe = CreateTestRecipe();

        // Act
        var node = new RecipeNode(recipe);

        // Assert
        var output = Assert.Single(node.Outputs);
        var recipeOutput = Assert.Single(recipe.Outputs);
        Assert.Equal($"output-{recipeOutput.Item.Identifier}", output.Identifier);
        Assert.Equal(recipeOutput.Item.DisplayName, output.DisplayName);
        Assert.False(output.IsInput);
        Assert.True(output.AllowsMultipleConnections);
        Assert.Contains(recipeOutput.Item, output.AcceptedItems);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void SetMultiplier_WithInvalidValue_ThrowsArgumentException(decimal multiplier)
    {
        // Arrange
        var recipe = CreateTestRecipe();
        var node = new RecipeNode(recipe);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => node.Multiplier = multiplier);
        Assert.Contains("Multiplier must be positive", exception.Message);
    }

    [Fact]
    public void SetMultiplier_WithValidValue_UpdatesMultiplier()
    {
        // Arrange
        var recipe = CreateTestRecipe();
        var node = new RecipeNode(recipe);
        var newMultiplier = 2.5m;

        // Act
        node.Multiplier = newMultiplier;

        // Assert
        Assert.Equal(newMultiplier, node.Multiplier);
    }

    [Fact]
    public void GetThroughput_CalculatesCorrectValues()
    {
        // Arrange
        var recipe = CreateTestRecipe();
        var node = new RecipeNode(recipe);
        node.Multiplier = 2; // 2 machines

        // Calculate expected values:
        // - Recipe takes 3.5 seconds
        // - Each machine produces 1 item per cycle
        // - 2 machines running
        // Expected items per minute = (60 / 3.5) * 2 = 34.29 items/minute

        // Act
        var throughput = node.GetThroughput();

        // Assert
        var recipeInput = Assert.Single(recipe.Inputs);
        var recipeOutput = Assert.Single(recipe.Outputs);

        Assert.Equal(34.29m, throughput.InputsPerMinute[recipeInput.Item], 2);
        Assert.Equal(34.29m, throughput.OutputsPerMinute[recipeOutput.Item], 2);
        Assert.Equal(100m, throughput.PowerConsumption); // 50 * 2 machines
    }

    [Fact]
    public void Validate_WithValidConnections_ReturnsTrue()
    {
        // Arrange
        var recipe = CreateTestRecipe();
        var node = new RecipeNode(recipe);

        // Act & Assert
        Assert.True(node.Validate());
    }
} 