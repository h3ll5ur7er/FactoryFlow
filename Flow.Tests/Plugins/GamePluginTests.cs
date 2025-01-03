using Flow.Core.Models;
using Flow.Core.Plugins;
using Flow.Games.TestGame;
using Xunit;

namespace Flow.Tests.Plugins;

public class GamePluginTests
{
    [Fact]
    public void LoadPlugin_WithValidData_LoadsSuccessfully()
    {
        // Arrange & Act
        var plugin = new TestGamePlugin();

        // Assert
        Assert.Equal("Test Factory Game", plugin.GameName);
        Assert.Equal(new Version(1, 0), plugin.Version);
        Assert.NotEmpty(plugin.Items);
        Assert.NotEmpty(plugin.Machines);
        Assert.NotEmpty(plugin.Recipes);
    }

    [Fact]
    public void LoadPlugin_RecipeReferencesValidItems()
    {
        // Arrange
        var plugin = new TestGamePlugin();
        var recipe = plugin.Recipes.First();

        // Assert
        foreach (var input in recipe.Inputs)
        {
            Assert.Contains(input.Item, plugin.Items);
        }

        foreach (var output in recipe.Outputs)
        {
            Assert.Contains(output.Item, plugin.Items);
        }
    }

    [Fact]
    public void LoadPlugin_RecipeReferencesValidMachine()
    {
        // Arrange
        var plugin = new TestGamePlugin();
        var recipe = plugin.Recipes.First();

        // Assert
        Assert.Contains(recipe.Machine, plugin.Machines);
    }
} 