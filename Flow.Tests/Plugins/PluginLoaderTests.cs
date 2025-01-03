using Flow.Core.Plugins;
using Flow.Games.TestGame;
using Xunit;

namespace Flow.Tests.Plugins;

public class PluginLoaderTests
{
    private readonly string _testPluginPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "plugins"
    );

    public PluginLoaderTests()
    {
        // Ensure the plugins directory exists
        Directory.CreateDirectory(_testPluginPath);
    }

    [Fact]
    public void LoadPlugins_WithValidPlugin_LoadsSuccessfully()
    {
        // Arrange
        var loader = new PluginLoader();
        var plugin = new TestGamePlugin();

        // Act
        loader.RegisterPlugin(plugin);
        var loadedPlugin = loader.GetPlugin(plugin.GameName);

        // Assert
        Assert.NotNull(loadedPlugin);
        Assert.Equal(plugin.GameName, loadedPlugin.GameName);
        Assert.Equal(plugin.Version, loadedPlugin.Version);
    }

    [Fact]
    public void LoadPlugins_WithDuplicateGameName_ThrowsArgumentException()
    {
        // Arrange
        var loader = new PluginLoader();
        var plugin1 = new TestGamePlugin();
        var plugin2 = new TestGamePlugin();

        // Act & Assert
        loader.RegisterPlugin(plugin1);
        var exception = Assert.Throws<ArgumentException>(() => loader.RegisterPlugin(plugin2));
        Assert.Contains("already registered", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetPlugin_WithUnknownGameName_ThrowsKeyNotFoundException()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => loader.GetPlugin("Unknown Game"));
    }

    [Fact]
    public void GetAvailableGames_ReturnsRegisteredGames()
    {
        // Arrange
        var loader = new PluginLoader();
        var plugin = new TestGamePlugin();

        // Act
        loader.RegisterPlugin(plugin);
        var games = loader.GetAvailableGames();

        // Assert
        var game = Assert.Single(games);
        Assert.Equal(plugin.GameName, game.Name);
        Assert.Equal(plugin.Version, game.Version);
    }

    [Fact]
    public void UnregisterPlugin_RemovesPlugin()
    {
        // Arrange
        var loader = new PluginLoader();
        var plugin = new TestGamePlugin();
        loader.RegisterPlugin(plugin);

        // Act
        loader.UnregisterPlugin(plugin.GameName);

        // Assert
        Assert.Empty(loader.GetAvailableGames());
        Assert.Throws<KeyNotFoundException>(() => loader.GetPlugin(plugin.GameName));
    }

    [Fact]
    public void UnregisterPlugin_WithUnknownGame_ThrowsKeyNotFoundException()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => loader.UnregisterPlugin("Unknown Game"));
    }

    [Fact]
    public void LoadPluginsFromDirectory_WithNonexistentDirectory_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        var loader = new PluginLoader();
        var nonexistentPath = Path.Combine(_testPluginPath, "nonexistent");

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() => loader.LoadPluginsFromDirectory(nonexistentPath));
    }

    [Fact]
    public void LoadPluginsFromDirectory_WithEmptyDirectory_LoadsNoPlugins()
    {
        // Arrange
        var loader = new PluginLoader();
        var emptyPath = Path.Combine(_testPluginPath, "empty");
        Directory.CreateDirectory(emptyPath);

        // Act
        var loadedCount = loader.LoadPluginsFromDirectory(emptyPath);

        // Assert
        Assert.Equal(0, loadedCount);
        Assert.Empty(loader.GetAvailableGames());
    }

    [Fact]
    public void LoadPluginsFromDirectory_WithInvalidAssembly_ContinuesLoading()
    {
        // Arrange
        var loader = new PluginLoader();
        var invalidPath = Path.Combine(_testPluginPath, "invalid");
        Directory.CreateDirectory(invalidPath);
        File.WriteAllText(Path.Combine(invalidPath, "Flow.Games.Invalid.dll"), "Not a valid DLL");

        // Act
        var loadedCount = loader.LoadPluginsFromDirectory(invalidPath);

        // Assert
        Assert.Equal(0, loadedCount);
        Assert.Empty(loader.GetAvailableGames());
    }

    [Fact]
    public void LoadPluginsFromDirectory_WithTestGamePlugin_LoadsSuccessfully()
    {
        // Arrange
        var loader = new PluginLoader();
        var pluginPath = Path.Combine(_testPluginPath, "testgame");
        Directory.CreateDirectory(pluginPath);

        // Copy the test game assembly to the plugins directory
        var testGameAssembly = typeof(TestGamePlugin).Assembly.Location;
        var targetPath = Path.Combine(pluginPath, Path.GetFileName(testGameAssembly));
        File.Copy(testGameAssembly, targetPath, true);

        // Act
        var loadedCount = loader.LoadPluginsFromDirectory(pluginPath);
        var games = loader.GetAvailableGames().ToList();

        // Assert
        Assert.Equal(1, loadedCount);
        Assert.Single(games);

        var game = games[0];
        Assert.Equal("Test Factory Game", game.Name);
        Assert.Equal(new Version(1, 0), game.Version);

        var plugin = loader.GetPlugin(game.Name);
        Assert.NotEmpty(plugin.Items);
        Assert.NotEmpty(plugin.Machines);
        Assert.NotEmpty(plugin.Recipes);

        // Verify some specific content
        Assert.Contains(plugin.Items, i => i.Identifier == "iron-ore");
        Assert.Contains(plugin.Machines, m => m.Identifier == "stone-furnace");
        Assert.Contains(plugin.Recipes, r => r.Identifier == "iron-smelting");
    }
} 