using Flow.Core.Models;
using Flow.Core.Models.Graph;
using Xunit;

namespace Flow.Tests.Models.Graph;

public class NodeTests
{
    private class TestableNode : Node
    {
        public TestableNode(string identifier, string displayName) 
            : base(identifier, displayName)
        {
        }

        public void ExposedAddInput(IConnector connector) => AddInput(connector);
        public void ExposedAddOutput(IConnector connector) => AddOutput(connector);
        public IConnector ExposedCreateInput(string id, string name, bool multiple = false, IEnumerable<Item>? items = null)
            => CreateInput(id, name, multiple, items);
        public IConnector ExposedCreateOutput(string id, string name, bool multiple = false, IEnumerable<Item>? items = null)
            => CreateOutput(id, name, multiple, items);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesNode()
    {
        // Arrange
        var identifier = "node";
        var displayName = "Node";

        // Act
        var node = new TestableNode(identifier, displayName);

        // Assert
        Assert.Equal(identifier, node.Identifier);
        Assert.Equal(displayName, node.DisplayName);
        Assert.Empty(node.Inputs);
        Assert.Empty(node.Outputs);
    }

    [Theory]
    [InlineData(null, "Node")]
    [InlineData("", "Node")]
    [InlineData(" ", "Node")]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier, string displayName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new TestableNode(identifier!, displayName));
        Assert.Contains("Identifier cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData("node", null)]
    [InlineData("node", "")]
    [InlineData("node", " ")]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string identifier, string? displayName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new TestableNode(identifier, displayName!));
        Assert.Contains("Display name cannot be empty", exception.Message);
    }

    [Fact]
    public void AddInput_WithValidConnector_AddsToInputs()
    {
        // Arrange
        var node = new TestableNode("node", "Node");
        var connector = new Connector("input", "Input", node, true, false);

        // Act
        node.ExposedAddInput(connector);

        // Assert
        var input = Assert.Single(node.Inputs);
        Assert.Equal(connector, input);
    }

    [Fact]
    public void AddInput_WithNullConnector_ThrowsArgumentNullException()
    {
        // Arrange
        var node = new TestableNode("node", "Node");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => node.ExposedAddInput(null!));
        Assert.Equal("connector", exception.ParamName);
    }

    [Fact]
    public void AddInput_WithOutputConnector_ThrowsArgumentException()
    {
        // Arrange
        var node = new TestableNode("node", "Node");
        var connector = new Connector("output", "Output", node, false, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => node.ExposedAddInput(connector));
        Assert.Contains("must be an input connector", exception.Message);
    }

    [Fact]
    public void AddInput_WithConnectorFromDifferentNode_ThrowsArgumentException()
    {
        // Arrange
        var node1 = new TestableNode("node1", "Node 1");
        var node2 = new TestableNode("node2", "Node 2");
        var connector = new Connector("input", "Input", node2, true, false);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => node1.ExposedAddInput(connector));
        Assert.Contains("belongs to another node", exception.Message);
    }

    [Fact]
    public void AddOutput_WithValidConnector_AddsToOutputs()
    {
        // Arrange
        var node = new TestableNode("node", "Node");
        var connector = new Connector("output", "Output", node, false, false);

        // Act
        node.ExposedAddOutput(connector);

        // Assert
        var output = Assert.Single(node.Outputs);
        Assert.Equal(connector, output);
    }

    [Fact]
    public void CreateInput_CreatesAndAddsInputConnector()
    {
        // Arrange
        var node = new TestableNode("node", "Node");
        var identifier = "input";
        var displayName = "Input";
        var ironOre = new Item("iron-ore", "Iron Ore");

        // Act
        var connector = node.ExposedCreateInput(identifier, displayName, true, new[] { ironOre });

        // Assert
        Assert.Equal(identifier, connector.Identifier);
        Assert.Equal(displayName, connector.DisplayName);
        Assert.Equal(node, connector.Parent);
        Assert.True(connector.IsInput);
        Assert.True(connector.AllowsMultipleConnections);
        Assert.Contains(ironOre, connector.AcceptedItems);
        Assert.Contains(connector, node.Inputs);
    }

    [Fact]
    public void CreateOutput_CreatesAndAddsOutputConnector()
    {
        // Arrange
        var node = new TestableNode("node", "Node");
        var identifier = "output";
        var displayName = "Output";
        var ironPlate = new Item("iron-plate", "Iron Plate");

        // Act
        var connector = node.ExposedCreateOutput(identifier, displayName, true, new[] { ironPlate });

        // Assert
        Assert.Equal(identifier, connector.Identifier);
        Assert.Equal(displayName, connector.DisplayName);
        Assert.Equal(node, connector.Parent);
        Assert.False(connector.IsInput);
        Assert.True(connector.AllowsMultipleConnections);
        Assert.Contains(ironPlate, connector.AcceptedItems);
        Assert.Contains(connector, node.Outputs);
    }

    [Fact]
    public void Validate_WithValidConnections_ReturnsTrue()
    {
        // Arrange
        var node1 = new TestableNode("node1", "Node 1");
        var node2 = new TestableNode("node2", "Node 2");
        
        var output = node1.ExposedCreateOutput("output", "Output", true);
        var input1 = node2.ExposedCreateInput("input1", "Input 1");
        var input2 = node2.ExposedCreateInput("input2", "Input 2");

        _ = new Connection(output, input1);
        _ = new Connection(output, input2);

        // Act & Assert
        Assert.True(node1.Validate());
        Assert.True(node2.Validate());
    }

    [Fact]
    public void Validate_WithInvalidConnections_ReturnsFalse()
    {
        // Arrange
        var node1 = new TestableNode("node1", "Node 1");
        var node2 = new TestableNode("node2", "Node 2");
        
        // Create a test connector that doesn't allow multiple connections
        var output = new TestConnector("output", "Output", node1, false, false);
        node1.ExposedAddOutput(output);
        
        var input1 = node2.ExposedCreateInput("input1", "Input 1");
        var input2 = node2.ExposedCreateInput("input2", "Input 2");

        // Create invalid state with multiple connections
        _ = new TestConnection(output, input1);
        _ = new TestConnection(output, input2);

        // Act & Assert
        Assert.False(node1.Validate());
    }
} 