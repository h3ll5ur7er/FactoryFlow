using Flow.Core.Models;

namespace Flow.Core.Plugins;

/// <summary>
/// Represents a game plugin that provides items, machines, and recipes for a specific game.
/// </summary>
public interface IGamePlugin
{
    /// <summary>
    /// Gets the display name of the game.
    /// </summary>
    string GameName { get; }

    /// <summary>
    /// Gets the version of the plugin.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Gets all available items in the game.
    /// </summary>
    IReadOnlyCollection<Item> Items { get; }

    /// <summary>
    /// Gets all available machines in the game.
    /// </summary>
    IReadOnlyCollection<Machine> Machines { get; }

    /// <summary>
    /// Gets all available recipes in the game.
    /// </summary>
    IReadOnlyCollection<Recipe> Recipes { get; }
} 