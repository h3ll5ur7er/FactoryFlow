using Flow.Core.Models;
using Xunit;

namespace Flow.Tests.Models;

public class RecipeTests
{
    private readonly Item _ironOre = new("iron-ore", "Iron Ore");
    private readonly Item _ironPlate = new("iron-plate", "Iron Plate");
    private readonly Machine _furnace = new("furnace", "Stone Furnace", 10.0m);
    private readonly TimeSpan _processingTime = TimeSpan.FromSeconds(2);

    [Fact]
    public void Constructor_WithValidParameters_CreatesRecipe()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act
        var recipe = new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            outputs,
            _furnace,
            _processingTime);

        // Assert
        Assert.Equal("iron-smelting", recipe.Identifier);
        Assert.Equal("Iron Smelting", recipe.DisplayName);
        Assert.Equal(inputs, recipe.Inputs);
        Assert.Equal(outputs, recipe.Outputs);
        Assert.Equal(_furnace, recipe.Machine);
        Assert.Equal(_processingTime, recipe.ProcessingTime);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier)
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            identifier!,
            "Iron Smelting",
            inputs,
            outputs,
            _furnace,
            _processingTime));

        Assert.Contains("identifier", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string? displayName)
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            "iron-smelting",
            displayName!,
            inputs,
            outputs,
            _furnace,
            _processingTime));

        Assert.Contains("displayName", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithNullInputs_ThrowsArgumentNullException()
    {
        // Arrange
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            null!,
            outputs,
            _furnace,
            _processingTime));

        Assert.Equal("inputs", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithEmptyInputs_ThrowsArgumentException()
    {
        // Arrange
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            Array.Empty<ItemStack>(),
            outputs,
            _furnace,
            _processingTime));

        Assert.Contains("inputs", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithNullOutputs_ThrowsArgumentNullException()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            null!,
            _furnace,
            _processingTime));

        Assert.Equal("outputs", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithEmptyOutputs_ThrowsArgumentException()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            Array.Empty<ItemStack>(),
            _furnace,
            _processingTime));

        Assert.Contains("outputs", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithNullMachine_ThrowsArgumentNullException()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            outputs,
            null!,
            _processingTime));

        Assert.Equal("machine", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithZeroProcessingTime_ThrowsArgumentException()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            outputs,
            _furnace,
            TimeSpan.Zero));

        Assert.Contains("processing time", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithNegativeProcessingTime_ThrowsArgumentException()
    {
        // Arrange
        var inputs = new[] { new ItemStack(_ironOre, 1.0m) };
        var outputs = new[] { new ItemStack(_ironPlate, 1.0m) };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Recipe(
            "iron-smelting",
            "Iron Smelting",
            inputs,
            outputs,
            _furnace,
            TimeSpan.FromSeconds(-1)));

        Assert.Contains("processing time", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
} 