using Flow.Core.Models;
using Xunit;

namespace Flow.Tests.Models;

public class MachineTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesMachine()
    {
        // Arrange
        var identifier = "assembler-mk1";
        var displayName = "Assembler MK1";
        var powerConsumption = 50.0m;

        // Act
        var machine = new Machine(identifier, displayName, powerConsumption);

        // Assert
        Assert.Equal(identifier, machine.Identifier);
        Assert.Equal(displayName, machine.DisplayName);
        Assert.Equal(powerConsumption, machine.PowerConsumption);
    }

    [Theory]
    [InlineData(null, "Assembler MK1", 50.0)]
    [InlineData("", "Assembler MK1", 50.0)]
    [InlineData(" ", "Assembler MK1", 50.0)]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier, string displayName, decimal powerConsumption)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Machine(identifier!, displayName, powerConsumption));
        Assert.Contains("identifier", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("assembler-mk1", null, 50.0)]
    [InlineData("assembler-mk1", "", 50.0)]
    [InlineData("assembler-mk1", " ", 50.0)]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string identifier, string? displayName, decimal powerConsumption)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Machine(identifier, displayName!, powerConsumption));
        Assert.Contains("displayName", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-50.0)]
    public void Constructor_WithNegativePowerConsumption_ThrowsArgumentException(decimal powerConsumption)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Machine("assembler-mk1", "Assembler MK1", powerConsumption));
        Assert.Contains("power consumption", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithZeroPowerConsumption_CreatesValidMachine()
    {
        // Arrange
        var identifier = "solar-panel";
        var displayName = "Solar Panel";
        var powerConsumption = 0.0m;

        // Act
        var machine = new Machine(identifier, displayName, powerConsumption);

        // Assert
        Assert.Equal(powerConsumption, machine.PowerConsumption);
    }
} 