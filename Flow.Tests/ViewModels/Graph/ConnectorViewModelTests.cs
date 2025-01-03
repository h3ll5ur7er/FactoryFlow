using System;
using Flow.ViewModels.Graph;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class ConnectorViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithCorrectType()
    {
        // Arrange & Act
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Assert
        Assert.Equal(ConnectorType.Input, connector.Type);
        Assert.False(connector.IsConnected);
        Assert.False(connector.AllowMultipleConnections);
        Assert.Empty(connector.Connections);
        Assert.Empty(connector.AcceptedTypes);
    }

    [Fact]
    public void CanConnectTo_WhenOtherIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => connector.CanConnectTo(null!));
    }

    [Fact]
    public void CanConnectTo_WhenSameConnector_ShouldReturnFalse()
    {
        // Arrange
        var connector = new ConnectorViewModel(ConnectorType.Input);

        // Act
        var result = connector.CanConnectTo(connector);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenSameType_ShouldReturnFalse()
    {
        // Arrange
        var input1 = new ConnectorViewModel(ConnectorType.Input);
        var input2 = new ConnectorViewModel(ConnectorType.Input);

        // Act
        var result = input1.CanConnectTo(input2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenAlreadyConnectedAndNoMultiple_ShouldReturnFalse()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output1 = new ConnectorViewModel(ConnectorType.Output);
        var output2 = new ConnectorViewModel(ConnectorType.Output);
        
        input.TryConnect(output1);

        // Act
        var result = input.CanConnectTo(output2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanConnectTo_WhenAllowMultipleConnections_ShouldReturnTrue()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input) { AllowMultipleConnections = true };
        var output1 = new ConnectorViewModel(ConnectorType.Output);
        var output2 = new ConnectorViewModel(ConnectorType.Output);
        
        input.TryConnect(output1);

        // Act
        var result = input.CanConnectTo(output2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanConnectTo_WhenNoAcceptedTypes_ShouldReturnTrue()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);

        // Act
        var result = input.CanConnectTo(output);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanConnectTo_WhenCompatibleTypes_ShouldReturnTrue()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);
        
        input.AcceptedTypes.Add(typeof(string));
        output.AcceptedTypes.Add(typeof(string));

        // Act
        var result = input.CanConnectTo(output);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanConnectTo_WhenIncompatibleTypes_ShouldReturnFalse()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);
        
        input.AcceptedTypes.Add(typeof(string));
        output.AcceptedTypes.Add(typeof(int));

        // Act
        var result = input.CanConnectTo(output);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryConnect_WhenValid_ShouldConnectBothWays()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);

        // Act
        var result = input.TryConnect(output);

        // Assert
        Assert.True(result);
        Assert.Single(input.Connections);
        Assert.Single(output.Connections);
        Assert.Contains(output, input.Connections);
        Assert.Contains(input, output.Connections);
        Assert.True(input.IsConnected);
        Assert.True(output.IsConnected);
    }

    [Fact]
    public void TryConnect_WhenInvalid_ShouldReturnFalseAndNotConnect()
    {
        // Arrange
        var input1 = new ConnectorViewModel(ConnectorType.Input);
        var input2 = new ConnectorViewModel(ConnectorType.Input);

        // Act
        var result = input1.TryConnect(input2);

        // Assert
        Assert.False(result);
        Assert.Empty(input1.Connections);
        Assert.Empty(input2.Connections);
        Assert.False(input1.IsConnected);
        Assert.False(input2.IsConnected);
    }

    [Fact]
    public void TryDisconnect_WhenConnected_ShouldDisconnectBothWays()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);
        input.TryConnect(output);

        // Act
        var result = input.TryDisconnect(output);

        // Assert
        Assert.True(result);
        Assert.Empty(input.Connections);
        Assert.Empty(output.Connections);
        Assert.False(input.IsConnected);
        Assert.False(output.IsConnected);
    }

    [Fact]
    public void TryDisconnect_WhenNotConnected_ShouldReturnFalse()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input);
        var output = new ConnectorViewModel(ConnectorType.Output);

        // Act
        var result = input.TryDisconnect(output);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisconnectAll_ShouldRemoveAllConnections()
    {
        // Arrange
        var input = new ConnectorViewModel(ConnectorType.Input) { AllowMultipleConnections = true };
        var output1 = new ConnectorViewModel(ConnectorType.Output);
        var output2 = new ConnectorViewModel(ConnectorType.Output);
        
        input.TryConnect(output1);
        input.TryConnect(output2);

        // Act
        input.DisconnectAll();

        // Assert
        Assert.Empty(input.Connections);
        Assert.Empty(output1.Connections);
        Assert.Empty(output2.Connections);
        Assert.False(input.IsConnected);
        Assert.False(output1.IsConnected);
        Assert.False(output2.IsConnected);
    }
} 