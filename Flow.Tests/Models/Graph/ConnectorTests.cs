using Flow.Core.Models;
using Flow.Core.Models.Graph;
using Xunit;

namespace Flow.Tests.Models.Graph;

public class ConnectorTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesConnector()
    {
        // Arrange
        var node = new TestNode("node", "Node");
        var identifier = "input";
        var displayName = "Input";
        var isInput = true;
        var allowsMultiple = false;

        // Act
        var connector = new Connector(identifier, displayName, node, isInput, allowsMultiple);

        // Assert
        Assert.Equal(identifier, connector.Identifier);
        Assert.Equal(displayName, connector.DisplayName);
        Assert.Equal(node, connector.Parent);
        Assert.Equal(isInput, connector.IsInput);
        Assert.Equal(allowsMultiple, connector.AllowsMultipleConnections);
        Assert.Empty(connector.AcceptedItems);
        Assert.Empty(connector.Connections);
    }

    [Theory]
    [InlineData(null, "Input")]
    [InlineData("", "Input")]
    [InlineData(" ", "Input")]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier, string displayName)
    {
        // Arrange
        var node = new TestNode("node", "Node");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Connector(identifier!, displayName, node, true, false));
        Assert.Contains("Identifier cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData("input", null)]
    [InlineData("input", "")]
    [InlineData("input", " ")]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string identifier, string? displayName)
    {
        // Arrange
        var node = new TestNode("node", "Node");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Connector(identifier, displayName!, node, true, false));
        Assert.Contains("Display name cannot be empty", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullParent_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Connector("input", "Input", null!, true, false));
        Assert.Equal("parent", exception.ParamName);
    }

    [Fact]
    public void AddConnection_WithValidConnection_AddsConnection()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new Connector("output", "Output", node1, false, false);
        var input = new Connector("input", "Input", node2, true, false);
        var connection = new TestConnection(output, input);

        // Assert
        Assert.Contains(connection, output.Connections);
        Assert.Contains(connection, input.Connections);
    }

    [Fact]
    public void AddConnection_WithMultipleConnectionsNotAllowed_ThrowsInvalidOperationException()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var node3 = new TestNode("node3", "Node 3");
        var output = new Connector("output", "Output", node1, false, false);
        var input1 = new Connector("input1", "Input 1", node2, true, false);
        var input2 = new Connector("input2", "Input 2", node3, true, false);

        // Act
        _ = new TestConnection(output, input1);

        // Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => new TestConnection(output, input2));
        Assert.Contains("does not allow multiple connections", exception.Message);
    }

    [Fact]
    public void RemoveConnection_RemovesConnection()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new Connector("output", "Output", node1, false, false);
        var input = new Connector("input", "Input", node2, true, false);
        var connection = new TestConnection(output, input);

        // Act
        output.RemoveConnection(connection);
        input.RemoveConnection(connection);

        // Assert
        Assert.Empty(output.Connections);
        Assert.Empty(input.Connections);
    }

    [Fact]
    public void CanConnectTo_WithCompatibleConnectors_ReturnsTrue()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new Connector("output", "Output", node1, false, false);
        var input = new Connector("input", "Input", node2, true, false);

        // Act & Assert
        Assert.True(output.CanConnectTo(input));
        Assert.True(input.CanConnectTo(output));
    }

    [Fact]
    public void CanConnectTo_WithSameConnector_ReturnsFalse()
    {
        // Arrange
        var node = new TestNode("node", "Node");
        var connector = new Connector("connector", "Connector", node, true, false);

        // Act & Assert
        Assert.False(connector.CanConnectTo(connector));
    }

    [Fact]
    public void CanConnectTo_WithSameDirection_ReturnsFalse()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var input1 = new Connector("input1", "Input 1", node1, true, false);
        var input2 = new Connector("input2", "Input 2", node2, true, false);

        // Act & Assert
        Assert.False(input1.CanConnectTo(input2));
    }

    [Fact]
    public void CanConnectTo_WithIncompatibleItems_ReturnsFalse()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var ironOre = new Item("iron-ore", "Iron Ore");
        var copperOre = new Item("copper-ore", "Copper Ore");

        var output = new Connector("output", "Output", node1, false, false, new[] { ironOre });
        var input = new Connector("input", "Input", node2, true, false, new[] { copperOre });

        // Act & Assert
        Assert.False(output.CanConnectTo(input));
        Assert.False(input.CanConnectTo(output));
    }

    [Fact]
    public void ValidateConnections_WithValidConnections_ReturnsTrue()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new Connector("output", "Output", node1, false, true);  // Allows multiple
        var input1 = new Connector("input1", "Input 1", node2, true, false);
        var input2 = new Connector("input2", "Input 2", node2, true, false);

        _ = new TestConnection(output, input1);
        _ = new TestConnection(output, input2);

        // Act & Assert
        Assert.True(output.ValidateConnections());
        Assert.True(input1.ValidateConnections());
        Assert.True(input2.ValidateConnections());
    }
} 