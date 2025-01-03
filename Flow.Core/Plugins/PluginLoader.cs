using System.Reflection;

namespace Flow.Core.Plugins;

/// <summary>
/// Manages the loading and unloading of game plugins.
/// </summary>
public class PluginLoader
{
    private readonly Dictionary<string, IGamePlugin> _plugins = new();

    /// <summary>
    /// Registers a new game plugin.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when plugin is null.</exception>
    /// <exception cref="ArgumentException">Thrown when a plugin with the same game name is already registered.</exception>
    public void RegisterPlugin(IGamePlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        if (_plugins.ContainsKey(plugin.GameName))
            throw new ArgumentException($"A plugin for game '{plugin.GameName}' is already registered.");

        _plugins.Add(plugin.GameName, plugin);
    }

    /// <summary>
    /// Loads plugins from a directory.
    /// </summary>
    /// <param name="directory">The directory to search for plugin assemblies.</param>
    /// <returns>The number of plugins loaded.</returns>
    public int LoadPluginsFromDirectory(string directory)
    {
        ArgumentNullException.ThrowIfNull(directory);
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Plugin directory '{directory}' not found.");

        var pluginFiles = Directory.GetFiles(directory, "Flow.Games.*.dll");
        var loadedCount = 0;

        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(IGamePlugin).IsAssignableFrom(t));

                foreach (var pluginType in pluginTypes)
                {
                    if (Activator.CreateInstance(pluginType) is IGamePlugin plugin)
                    {
                        RegisterPlugin(plugin);
                        loadedCount++;
                    }
                }
            }
            catch (Exception ex) when (
                ex is ReflectionTypeLoadException
                || ex is BadImageFormatException
                || ex is FileLoadException)
            {
                // Log the error but continue loading other plugins
                Console.Error.WriteLine($"Failed to load plugin from {file}: {ex.Message}");
            }
        }

        return loadedCount;
    }

    /// <summary>
    /// Unregisters a game plugin.
    /// </summary>
    /// <param name="gameName">The name of the game to unregister.</param>
    /// <exception cref="KeyNotFoundException">Thrown when no plugin is registered for the specified game.</exception>
    public void UnregisterPlugin(string gameName)
    {
        if (!_plugins.Remove(gameName))
            throw new KeyNotFoundException($"No plugin found for game '{gameName}'.");
    }

    /// <summary>
    /// Gets a registered game plugin.
    /// </summary>
    /// <param name="gameName">The name of the game.</param>
    /// <returns>The registered game plugin.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no plugin is registered for the specified game.</exception>
    public IGamePlugin GetPlugin(string gameName)
    {
        if (!_plugins.TryGetValue(gameName, out var plugin))
            throw new KeyNotFoundException($"No plugin found for game '{gameName}'.");

        return plugin;
    }

    /// <summary>
    /// Gets information about all registered games.
    /// </summary>
    /// <returns>A collection of game information.</returns>
    public IEnumerable<GameInfo> GetAvailableGames()
    {
        return _plugins.Values
            .Select(p => new GameInfo(p.GameName, p.Version))
            .ToList();
    }
} 