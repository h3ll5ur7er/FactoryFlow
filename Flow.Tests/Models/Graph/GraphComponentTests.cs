using Flow.Core.Models;
using Xunit;
using Xunit.Abstractions;

namespace Flow.Tests.Models.Graph;

public class GraphComponentTests
{
    private readonly ITestOutputHelper _output;

    public GraphComponentTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Node_WithValidConnectors_ValidatesSuccessfully()
    {
        // Arrange
        var node = new TestNode("test-node", "Test Node");
        var input = new TestConnector("input", "Input", node, true, false, output: _output);
        var output = new TestConnector("output", "Output", node, false, false, output: _output);
        node.AddInput(input);
        node.AddOutput(output);

        // Assert
        Assert.True(node.Validate());
    }

    [Fact]
    public void Connector_WithValidConnection_ValidatesSuccessfully()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false, output: _output);
        var input = new TestConnector("input", "Input", node2, true, false, output: _output);
        node1.AddOutput(output);
        node2.AddInput(input);

        // Act
        _output.WriteLine($"Can connect: {output.CanConnectTo(input)}");
        var connection = new TestConnection(output, input, output: _output);

        // Assert
        _output.WriteLine($"Output connections: {output.Connections.Count}");
        _output.WriteLine($"Input connections: {input.Connections.Count}");
        _output.WriteLine($"Connection validation: {connection.Validate()}");

        var outputValid = output.ValidateConnections();
        var inputValid = input.ValidateConnections();
        var connectionValid = connection.Validate();

        _output.WriteLine($"Output validation: {outputValid}");
        _output.WriteLine($"Input validation: {inputValid}");
        _output.WriteLine($"Connection validation: {connectionValid}");

        Assert.True(outputValid, "Output validation failed");
        Assert.True(inputValid, "Input validation failed");
        Assert.True(connectionValid, "Connection validation failed");
    }

    [Fact]
    public void Connector_WithMultipleConnections_ValidatesBasedOnAllowance()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var node3 = new TestNode("node3", "Node 3");

        var output = new TestConnector("output", "Output", node1, false, true, output: _output);  // Allows multiple
        var input1 = new TestConnector("input1", "Input 1", node2, true, false, output: _output); // Single connection
        var input2 = new TestConnector("input2", "Input 2", node3, true, false, output: _output); // Single connection

        node1.AddOutput(output);
        node2.AddInput(input1);
        node3.AddInput(input2);

        // Act
        _output.WriteLine($"Can connect to input1: {output.CanConnectTo(input1)}");
        var connection1 = new TestConnection(output, input1, output: _output);
        _output.WriteLine($"Can connect to input2: {output.CanConnectTo(input2)}");
        var connection2 = new TestConnection(output, input2, output: _output);

        // Assert
        _output.WriteLine($"Output connections: {output.Connections.Count}");
        _output.WriteLine($"Input1 connections: {input1.Connections.Count}");
        _output.WriteLine($"Input2 connections: {input2.Connections.Count}");
        _output.WriteLine($"Connection1 validation: {connection1.Validate()}");
        _output.WriteLine($"Connection2 validation: {connection2.Validate()}");

        var outputValid = output.ValidateConnections();
        var input1Valid = input1.ValidateConnections();
        var input2Valid = input2.ValidateConnections();
        var connection1Valid = connection1.Validate();
        var connection2Valid = connection2.Validate();

        _output.WriteLine($"Output validation: {outputValid}");
        _output.WriteLine($"Input1 validation: {input1Valid}");
        _output.WriteLine($"Input2 validation: {input2Valid}");
        _output.WriteLine($"Connection1 validation: {connection1Valid}");
        _output.WriteLine($"Connection2 validation: {connection2Valid}");

        Assert.True(outputValid, "Output validation failed");  // Multiple connections allowed
        Assert.True(input1Valid, "Input1 validation failed"); // Single connection
        Assert.True(input2Valid, "Input2 validation failed"); // Single connection
        Assert.True(connection1Valid, "Connection1 validation failed");
        Assert.True(connection2Valid, "Connection2 validation failed");
    }

    [Fact]
    public void Connector_WithIncompatibleItemTypes_CannotConnect()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");

        var ironOre = new Item("iron-ore", "Iron Ore");
        var copperOre = new Item("copper-ore", "Copper Ore");

        var output = new TestConnector("output", "Output", node1, false, false, new[] { ironOre }, _output);
        var input = new TestConnector("input", "Input", node2, true, false, new[] { copperOre }, _output);

        node1.AddOutput(output);
        node2.AddInput(input);

        // Assert
        Assert.False(output.CanConnectTo(input));
        Assert.False(input.CanConnectTo(output));
    }

    [Fact]
    public void Connection_WithInvalidFlowRate_FailsValidation()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false, output: _output);
        var input = new TestConnector("input", "Input", node2, true, false, output: _output);
        node1.AddOutput(output);
        node2.AddInput(input);

        // Act
        var connection = new TestConnection(output, input, 0m, _output);  // Invalid flow rate

        // Assert
        Assert.False(connection.Validate());
    }

    [Fact]
    public void Connection_WhenRemoved_UpdatesConnectors()
    {
        // Arrange
        var node1 = new TestNode("node1", "Node 1");
        var node2 = new TestNode("node2", "Node 2");
        var output = new TestConnector("output", "Output", node1, false, false, output: _output);
        var input = new TestConnector("input", "Input", node2, true, false, output: _output);
        node1.AddOutput(output);
        node2.AddInput(input);

        var connection = new TestConnection(output, input, output: _output);

        // Act
        connection.Remove();

        // Assert
        Assert.Empty(output.Connections);
        Assert.Empty(input.Connections);
    }
} 