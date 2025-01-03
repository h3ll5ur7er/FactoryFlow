using System;
using System.Linq;
using Flow.Core.Models;
using Flow.ViewModels.Graph;
using Xunit;
using Flow.Core.Services;
using Flow.Tests.TestHelpers;
using Moq;

namespace Flow.Tests.ViewModels.Graph;

public class RecipeNodeViewModelTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var recipe = Flow.Tests.TestHelpers.MockFactory.CreateRecipe();

        // Act
        var viewModel = new RecipeNodeViewModel(graphManager.Object)
        {
            Title = "Test Recipe Node",
            Recipe = recipe.Object
        };

        // Assert
        Assert.Equal("Test Recipe Node", viewModel.Title);
        Assert.Equal(NodeType.Recipe, viewModel.NodeType);
        Assert.Equal(recipe.Object, viewModel.Recipe);
    }

    [Fact]
    public void Recipe_WhenSetToNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var viewModel = new RecipeNodeViewModel(graphManager.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => viewModel.Recipe = null!);
    }
} 