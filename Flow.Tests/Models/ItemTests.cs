using Flow.Core.Models;
using Xunit;

namespace Flow.Tests.Models;

public class ItemTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesItem()
    {
        // Arrange
        var identifier = "iron-ore";
        var displayName = "Iron Ore";

        // Act
        var item = new Item(identifier, displayName);

        // Assert
        Assert.Equal(identifier, item.Identifier);
        Assert.Equal(displayName, item.DisplayName);
    }

    [Theory]
    [InlineData(null, "Iron Ore")]
    [InlineData("", "Iron Ore")]
    [InlineData(" ", "Iron Ore")]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier, string displayName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Item(identifier!, displayName));
        Assert.Contains("identifier", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("iron-ore", null)]
    [InlineData("iron-ore", "")]
    [InlineData("iron-ore", " ")]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string identifier, string? displayName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Item(identifier, displayName!));
        Assert.Contains("displayName", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
} 