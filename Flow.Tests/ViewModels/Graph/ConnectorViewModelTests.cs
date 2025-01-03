using System;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class ConnectorViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithCorrectValues()
    {
        // Arrange
        var node = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        
        // Act
        var connector = new ConnectorViewModel("test", "Test Connector", node, ConnectorType.Input, false);

        // Assert
        Assert.Equal("test", connector.Identifier);
        Assert.Equal("Test Connector", connector.DisplayName);
        Assert.Equal(ConnectorType.Input, connector.Type);
        Assert.False(connector.AllowsMultipleConnections);
        Assert.False(connector.IsConnected);
        Assert.Empty(connector.Connections);
        Assert.Equal(node, connector.Node);
    }

    [Theory]
    [InlineData(null, "Test")]
    [InlineData("", "Test")]
    [InlineData(" ", "Test")]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string? identifier, string displayName)
    {
        // Arrange
        var node = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new ConnectorViewModel(identifier!, displayName, node, ConnectorType.Input, false));
        Assert.Contains("Identifier", exception.Message);
    }

    [Theory]
    [InlineData("test", null)]
    [InlineData("test", "")]
    [InlineData("test", " ")]
    public void Constructor_WithInvalidDisplayName_ThrowsArgumentException(string identifier, string? displayName)
    {
        // Arrange
        var node = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new ConnectorViewModel(identifier, displayName!, node, ConnectorType.Input, false));
        Assert.Contains("Display name", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullNode_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new ConnectorViewModel("test", "Test", null!, ConnectorType.Input, false));
        Assert.Equal("node", exception.ParamName);
    }

    [Fact]
    public void CanConnectTo_WhenSameConnector_ReturnsFalse()
    {
        // Arrange
        var node = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var connector = new ConnectorViewModel("test", "Test", node, ConnectorType.Input, false);

        // Act
        var result = connector.CanConnectTo(connector);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenSameNode_ReturnsFalse()
    {
        // Arrange
        var node = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var input = new ConnectorViewModel("input", "Input", node, ConnectorType.Input, false);
        var output = new ConnectorViewModel("output", "Output", node, ConnectorType.Output, false);

        // Act
        var result = input.CanConnectTo(output);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenSameType_ReturnsFalse()
    {
        // Arrange
        var node1 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node2 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var input1 = new ConnectorViewModel("input1", "Input 1", node1, ConnectorType.Input, false);
        var input2 = new ConnectorViewModel("input2", "Input 2", node2, ConnectorType.Input, false);

        // Act
        var result = input1.CanConnectTo(input2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenAlreadyConnectedAndNoMultipleConnections_ReturnsFalse()
    {
        // Arrange
        var node1 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node2 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node3 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var output = new ConnectorViewModel("output", "Output", node1, ConnectorType.Output, false);
        var input1 = new ConnectorViewModel("input1", "Input 1", node2, ConnectorType.Input, false);
        var input2 = new ConnectorViewModel("input2", "Input 2", node3, ConnectorType.Input, false);

        // Create first connection
        var connection = new ConnectionViewModel(output, input1);
        output.AddConnection(connection);
        input1.AddConnection(connection);

        // Act
        var result = output.CanConnectTo(input2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenValidConnection_ReturnsTrue()
    {
        // Arrange
        var node1 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node2 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var output = new ConnectorViewModel("output", "Output", node1, ConnectorType.Output, true);
        var input = new ConnectorViewModel("input", "Input", node2, ConnectorType.Input, true);

        // Act
        var result = output.CanConnectTo(input);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddConnection_WhenValid_AddsConnection()
    {
        // Arrange
        var node1 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node2 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var output = new ConnectorViewModel("output", "Output", node1, ConnectorType.Output, true);
        var input = new ConnectorViewModel("input", "Input", node2, ConnectorType.Input, true);
        var connection = new ConnectionViewModel(output, input);

        // Act
        output.AddConnection(connection);
        input.AddConnection(connection);

        // Assert
        Assert.Single(output.Connections);
        Assert.Single(input.Connections);
        Assert.Contains(connection, output.Connections);
        Assert.Contains(connection, input.Connections);
        Assert.True(output.IsConnected);
        Assert.True(input.IsConnected);
    }

    [Fact]
    public void RemoveConnection_RemovesConnectionAndUpdatesState()
    {
        // Arrange
        var node1 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var node2 = new NodeViewModel(Flow.Tests.TestHelpers.MockFactory.CreateGraphManager().Object);
        var output = new ConnectorViewModel("output", "Output", node1, ConnectorType.Output, true);
        var input = new ConnectorViewModel("input", "Input", node2, ConnectorType.Input, true);
        var connection = new ConnectionViewModel(output, input);
        output.AddConnection(connection);
        input.AddConnection(connection);

        // Act
        output.RemoveConnection(connection);
        input.RemoveConnection(connection);

        // Assert
        Assert.Empty(output.Connections);
        Assert.Empty(input.Connections);
        Assert.False(output.IsConnected);
        Assert.False(input.IsConnected);
    }
} 