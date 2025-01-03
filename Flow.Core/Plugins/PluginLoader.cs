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
        Console.WriteLine($"Registered plugin: {plugin.GameName} v{plugin.Version}");
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
        {
            Console.WriteLine($"Plugin directory not found: {directory}");
            throw new DirectoryNotFoundException($"Plugin directory '{directory}' not found.");
        }

        Console.WriteLine($"Searching for plugins in: {directory}");
        var pluginFiles = Directory.GetFiles(directory, "Flow.Games.*.dll");
        Console.WriteLine($"Found {pluginFiles.Length} potential plugin files");
        var loadedCount = 0;

        foreach (var file in pluginFiles)
        {
            Console.WriteLine($"Attempting to load plugin from: {file}");
            try
            {
                var assembly = Assembly.LoadFrom(file);
                Console.WriteLine($"Successfully loaded assembly: {assembly.FullName}");
                
                var pluginTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(IGamePlugin).IsAssignableFrom(t));
                Console.WriteLine($"Found {pluginTypes.Count()} plugin types in assembly");

                foreach (var pluginType in pluginTypes)
                {
                    Console.WriteLine($"Creating instance of plugin type: {pluginType.FullName}");
                    if (Activator.CreateInstance(pluginType) is IGamePlugin plugin)
                    {
                        RegisterPlugin(plugin);
                        loadedCount++;
                        Console.WriteLine($"Successfully loaded plugin: {plugin.GameName} v{plugin.Version}");
                        Console.WriteLine($"  Items: {plugin.Items.Count}");
                        Console.WriteLine($"  Machines: {plugin.Machines.Count}");
                        Console.WriteLine($"  Recipes: {plugin.Recipes.Count}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create plugin instance for type: {pluginType.FullName}");
                    }
                }
            }
            catch (Exception ex) when (
                ex is ReflectionTypeLoadException
                || ex is BadImageFormatException
                || ex is FileLoadException)
            {
                Console.WriteLine($"Failed to load plugin from {file}");
                Console.WriteLine($"Error: {ex.GetType().Name} - {ex.Message}");
                if (ex is ReflectionTypeLoadException loadEx)
                {
                    foreach (var loaderEx in loadEx.LoaderExceptions)
                    {
                        Console.WriteLine($"Loader Exception: {loaderEx?.Message}");
                    }
                }
            }
        }

        Console.WriteLine($"Finished loading plugins. Total loaded: {loadedCount}");
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
        Console.WriteLine($"Unregistered plugin: {gameName}");
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