using Flow.Core.Models;
using Flow.Core.Models.Graph;
using Xunit;

namespace Flow.Tests.Models.Graph;

public class ConnectionTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesConnection()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false);
        var input = new TestConnector("input", "Input", node2, true, false);

        // Act
        var connection = new Connection(output, input);

        // Assert
        Assert.Equal(output, connection.Source);
        Assert.Equal(input, connection.Target);
        Assert.Equal(1m, connection.FlowRate);
        Assert.True(connection.IsEnabled);
        Assert.Contains(connection, output.Connections);
        Assert.Contains(connection, input.Connections);
    }

    [Fact]
    public void Constructor_WithNullSource_ThrowsArgumentNullException()
    {
        // Arrange
        var node = new TestNode("node", "Node");
        var input = new TestConnector("input", "Input", node, true, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Connection(null!, input));
        Assert.Equal("source", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullTarget_ThrowsArgumentNullException()
    {
        // Arrange
        var node = new TestNode("node", "Node");
        var output = new TestConnector("output", "Output", node, false, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Connection(output, null!));
        Assert.Equal("target", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidFlowRate_ThrowsArgumentException(decimal flowRate)
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false);
        var input = new TestConnector("input", "Input", node2, true, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Connection(output, input, flowRate));
        Assert.Contains("Flow rate must be positive", exception.Message);
    }

    [Fact]
    public void Constructor_WithInputAsSource_ThrowsArgumentException()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var input1 = new TestConnector("input1", "Input 1", node1, true, false);
        var input2 = new TestConnector("input2", "Input 2", node2, true, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Connection(input1, input2));
        Assert.Contains("Source connector must be an output", exception.Message);
    }

    [Fact]
    public void Constructor_WithOutputAsTarget_ThrowsArgumentException()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output1 = new TestConnector("output1", "Output 1", node1, false, false);
        var output2 = new TestConnector("output2", "Output 2", node2, false, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new Connection(output1, output2));
        Assert.Contains("Target connector must be an input", exception.Message);
    }

    [Fact]
    public void Remove_DisconnectsFromBothConnectors()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false);
        var input = new TestConnector("input", "Input", node2, true, false);
        var connection = new Connection(output, input);

        // Act
        connection.Remove();

        // Assert
        Assert.Empty(output.Connections);
        Assert.Empty(input.Connections);
        Assert.False(connection.IsEnabled);
    }

    [Fact]
    public void Validate_WithValidConnection_ReturnsTrue()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false);
        var input = new TestConnector("input", "Input", node2, true, false);
        var connection = new Connection(output, input);

        // Act & Assert
        Assert.True(connection.Validate());
    }
} 