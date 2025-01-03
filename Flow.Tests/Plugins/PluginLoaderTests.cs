using System;
using System.IO;
using System.Reflection;
using Flow.Core.Plugins;
using Flow.Tests.TestHelpers;
using Xunit;

namespace Flow.Tests.Plugins;

public class PluginLoaderTests
{
    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryDoesNotExist_ShouldThrowDirectoryNotFoundException()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var nonExistentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() => loader.LoadPluginsFromDirectory(nonExistentPath));
    }

    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryIsEmpty_ShouldReturnZero()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var emptyDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(emptyDir);

        try
        {
            // Act
            var loadedCount = loader.LoadPluginsFromDirectory(emptyDir);

            // Assert
            Assert.Equal(0, loadedCount);
        }
        finally
        {
            Directory.Delete(emptyDir);
        }
    }

    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryHasNonDllFiles_ShouldReturnZero()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, "test.txt"), "test");

        try
        {
            // Act
            var loadedCount = loader.LoadPluginsFromDirectory(dir);

            // Assert
            Assert.Equal(0, loadedCount);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryHasInvalidDll_ShouldReturnZero()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        File.WriteAllBytes(Path.Combine(dir, "invalid.dll"), new byte[0]);

        try
        {
            // Act
            var loadedCount = loader.LoadPluginsFromDirectory(dir);

            // Assert
            Assert.Equal(0, loadedCount);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryHasValidDllWithoutPlugins_ShouldReturnZero()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        
        // Copy System.Runtime.dll as an example of a valid DLL without plugins
        var runtimePath = typeof(string).Assembly.Location;
        File.Copy(runtimePath, Path.Combine(dir, "System.Runtime.dll"));

        try
        {
            // Act
            var loadedCount = loader.LoadPluginsFromDirectory(dir);

            // Assert
            Assert.Equal(0, loadedCount);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void LoadPluginsFromDirectory_WhenDirectoryHasValidPlugin_ShouldReturnOne()
    {
        // Arrange
        var gameRegistry = Flow.Tests.TestHelpers.MockFactory.CreateGameRegistry();
        var loader = new PluginLoader(gameRegistry.Object);
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        // Act
        var loadedCount = loader.LoadPluginsFromDirectory(dir);

        // Assert
        Assert.Equal(1, loadedCount);
        var availableGames = gameRegistry.Object.AvailableGames;
        Assert.Contains(availableGames, g => g.Name == "Test Factory Game");
    }
} 