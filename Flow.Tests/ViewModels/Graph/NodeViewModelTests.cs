using Flow.Core.Services;
using Flow.Tests.TestHelpers;
using Flow.ViewModels.Graph;
using Moq;
using Xunit;

namespace Flow.Tests.ViewModels.Graph;

public class NodeViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();

        // Act
        var viewModel = new NodeViewModel(graphManager.Object)
        {
            Title = "Test Node",
            NodeType = NodeType.Generic
        };

        // Assert
        Assert.Equal("Test Node", viewModel.Title);
        Assert.Equal(NodeType.Generic, viewModel.NodeType);
    }

    [Fact]
    public void DeleteCommand_ShouldCallRemoveNodeOnGraphManager()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new NodeViewModel(graphManager.Object)
        {
            Title = "Test Node",
            NodeType = NodeType.Generic
        };

        // Act
        viewModel.DeleteCommand.Execute(null);

        // Assert
        graphManager.Verify(m => m.RemoveNode(viewModel), Times.Once);
    }
} 