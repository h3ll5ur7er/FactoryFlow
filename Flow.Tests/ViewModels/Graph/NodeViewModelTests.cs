using System;
using Avalonia;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class NodeViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var node = new NodeViewModel();

        // Assert
        Assert.Equal(new Point(0, 0), node.Position);
        Assert.Equal(new Size(100, 100), node.Size);
        Assert.Equal("New Node", node.Title);
        Assert.Equal(NodeType.Generic, node.NodeType);
        Assert.False(node.IsSelected);
        Assert.Equal(1.0, node.Multiplier);
        Assert.Empty(node.InputConnectors);
        Assert.Empty(node.OutputConnectors);
    }

    [Fact]
    public void Position_WhenSet_ShouldUpdateValue()
    {
        // Arrange
        var node = new NodeViewModel();
        var newPosition = new Point(100, 200);

        // Act
        node.Position = newPosition;

        // Assert
        Assert.Equal(newPosition, node.Position);
    }

    [Fact]
    public void Size_WhenSet_ShouldUpdateValue()
    {
        // Arrange
        var node = new NodeViewModel();
        var newSize = new Size(150, 250);

        // Act
        node.Size = newSize;

        // Assert
        Assert.Equal(newSize, node.Size);
    }

    [Fact]
    public void Title_WhenSetToNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.Title = null!);
    }

    [Fact]
    public void Title_WhenSetToEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.Title = string.Empty);
    }

    [Fact]
    public void Title_WhenSetToValidValue_ShouldUpdateValue()
    {
        // Arrange
        var node = new NodeViewModel();
        var newTitle = "Test Node";

        // Act
        node.Title = newTitle;

        // Assert
        Assert.Equal(newTitle, node.Title);
    }

    [Fact]
    public void Multiplier_WhenSetToZero_ShouldThrowArgumentException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.Multiplier = 0);
    }

    [Fact]
    public void Multiplier_WhenSetToNegative_ShouldThrowArgumentException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.Multiplier = -1);
    }

    [Fact]
    public void Multiplier_WhenSetToValidValue_ShouldUpdateValue()
    {
        // Arrange
        var node = new NodeViewModel();
        var newMultiplier = 2.5;

        // Act
        node.Multiplier = newMultiplier;

        // Assert
        Assert.Equal(newMultiplier, node.Multiplier);
    }

    [Fact]
    public void AddInputConnector_WhenConnectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.AddInputConnector(null!));
    }

    [Fact]
    public void AddInputConnector_WhenConnectorIsOutputType_ShouldThrowArgumentException()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Output);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.AddInputConnector(connector));
    }

    [Fact]
    public void AddInputConnector_WhenConnectorIsValid_ShouldAddToInputConnectors()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Act
        node.AddInputConnector(connector);

        // Assert
        Assert.Single(node.InputConnectors);
        Assert.Contains(connector, node.InputConnectors);
    }

    [Fact]
    public void AddOutputConnector_WhenConnectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.AddOutputConnector(null!));
    }

    [Fact]
    public void AddOutputConnector_WhenConnectorIsInputType_ShouldThrowArgumentException()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => node.AddOutputConnector(connector));
    }

    [Fact]
    public void AddOutputConnector_WhenConnectorIsValid_ShouldAddToOutputConnectors()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Output);

        // Act
        node.AddOutputConnector(connector);

        // Assert
        Assert.Single(node.OutputConnectors);
        Assert.Contains(connector, node.OutputConnectors);
    }

    [Fact]
    public void RemoveInputConnector_WhenConnectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.RemoveInputConnector(null!));
    }

    [Fact]
    public void RemoveInputConnector_WhenConnectorExists_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Input);
        node.AddInputConnector(connector);

        // Act
        var result = node.RemoveInputConnector(connector);

        // Assert
        Assert.True(result);
        Assert.Empty(node.InputConnectors);
    }

    [Fact]
    public void RemoveInputConnector_WhenConnectorDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Act
        var result = node.RemoveInputConnector(connector);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveOutputConnector_WhenConnectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var node = new NodeViewModel();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => node.RemoveOutputConnector(null!));
    }

    [Fact]
    public void RemoveOutputConnector_WhenConnectorExists_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Output);
        node.AddOutputConnector(connector);

        // Act
        var result = node.RemoveOutputConnector(connector);

        // Assert
        Assert.True(result);
        Assert.Empty(node.OutputConnectors);
    }

    [Fact]
    public void RemoveOutputConnector_WhenConnectorDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var node = new NodeViewModel();
        var connector = new ConnectorViewModel(ConnectorType.Output);

        // Act
        var result = node.RemoveOutputConnector(connector);

        // Assert
        Assert.False(result);
    }
} 