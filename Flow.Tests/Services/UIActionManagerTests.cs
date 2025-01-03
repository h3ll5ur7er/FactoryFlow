using System;
using System.Threading.Tasks;
using Flow.Core.Services;
using Flow.Tests.TestHelpers;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.Services;

public class UIActionManagerTests
{
    [Fact]
    public async Task RegisterAction_ShouldAddAction()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var executed = false;

        // Act
        manager.RegisterAction(
            "test",
            () =>
            {
                executed = true;
                return Task.FromResult(true);
            },
            () => Task.FromResult(true)
        );
        await manager.ExecuteActionAsync("test");

        // Assert
        Assert.True(executed);
    }

    [Fact]
    public async Task UnregisterAction_ShouldRemoveAction()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            () => Task.FromResult(true)
        );

        // Act
        manager.UnregisterAction("test");

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => manager.ExecuteActionAsync("test"));
    }

    [Fact]
    public async Task ExecuteAction_WithInvalidAction_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => manager.ExecuteActionAsync("invalid"));
    }

    [Fact]
    public async Task ExecuteAction_WhenCannotExecute_ShouldNotExecute()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var executed = false;

        manager.RegisterAction(
            "test",
            () =>
            {
                executed = true;
                return Task.FromResult(true);
            },
            () => Task.FromResult(false)
        );

        // Act
        await manager.ExecuteActionAsync("test");

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task CanExecuteAction_WithInvalidAction_ShouldReturnFalse()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);

        // Act
        var result = await manager.CanExecuteActionAsync("invalid");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CanExecuteAction_WithValidAction_ShouldCheckCondition()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var canExecute = false;

        manager.RegisterAction(
            "test",
            () => Task.FromResult(true),
            () => Task.FromResult(canExecute)
        );

        // Act & Assert
        Assert.False(await manager.CanExecuteActionAsync("test"));

        canExecute = true;
        Assert.True(await manager.CanExecuteActionAsync("test"));
    }

    [Fact]
    public async Task ExecuteAction_WithGraphManager_ShouldExecuteAction()
    {
        // Arrange
        var nodeFactory = Flow.Tests.TestHelpers.MockFactory.CreateNodeFactory();
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var manager = new UIActionManager(graphManager.Object, nodeFactory.Object);
        var executed = false;

        manager.RegisterAction(
            "test",
            () =>
            {
                executed = true;
                return Task.FromResult(true);
            },
            () => Task.FromResult(true)
        );

        // Act
        await manager.ExecuteActionAsync("test");

        // Assert
        Assert.True(executed);
    }
} 