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
    public void Constructor_InitializesProperties()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var recipe = Flow.Tests.TestHelpers.MockFactory.CreateRecipe();

        // Act
        var viewModel = new RecipeNodeViewModel(graphManager.Object)
        {
            Recipe = recipe
        };

        // Assert
        Assert.Equal(recipe, viewModel.Recipe);
        Assert.Equal(NodeType.Recipe, viewModel.NodeType);
    }

    [Fact]
    public void Recipe_WhenSet_UpdatesTitle()
    {
        // Arrange
        var graphManager = Flow.Tests.TestHelpers.MockFactory.CreateGraphManager();
        var recipe = Flow.Tests.TestHelpers.MockFactory.CreateRecipe();
        var viewModel = new RecipeNodeViewModel(graphManager.Object);

        // Act
        viewModel.Recipe = recipe;

        // Assert
        Assert.Equal(recipe.DisplayName, viewModel.Title);
    }
} 