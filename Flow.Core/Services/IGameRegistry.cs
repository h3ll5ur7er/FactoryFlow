using Flow.Core.Models;
using Flow.Core.Plugins;

namespace Flow.Core.Services;

/// <summary>
/// Manages game metadata from loaded plugins, providing access to items, machines, and recipes.
/// </summary>
public interface IGameRegistry
{
    /// <summary>
    /// Gets the currently active game.
    /// </summary>
    GameInfo? ActiveGame { get; }

    /// <summary>
    /// Gets all available games.
    /// </summary>
    IReadOnlyCollection<GameInfo> AvailableGames { get; }

    /// <summary>
    /// Gets all items available in the active game.
    /// </summary>
    IReadOnlyCollection<Item> Items { get; }

    /// <summary>
    /// Gets all machines available in the active game.
    /// </summary>
    IReadOnlyCollection<Machine> Machines { get; }

    /// <summary>
    /// Gets all recipes available in the active game.
    /// </summary>
    IReadOnlyCollection<Recipe> Recipes { get; }

    /// <summary>
    /// Sets the active game.
    /// </summary>
    /// <param name="gameName">The name of the game to activate.</param>
    /// <returns>True if the game was activated successfully, false otherwise.</returns>
    bool SetActiveGame(string gameName);

    /// <summary>
    /// Registers a game plugin.
    /// </summary>
    /// <param name="plugin">The plugin to register.</param>
    void RegisterGame(IGamePlugin plugin);

    /// <summary>
    /// Gets a recipe by its identifier.
    /// </summary>
    /// <param name="identifier">The recipe identifier.</param>
    /// <returns>The recipe if found, null otherwise.</returns>
    Recipe? GetRecipe(string identifier);

    /// <summary>
    /// Gets an item by its identifier.
    /// </summary>
    /// <param name="identifier">The item identifier.</param>
    /// <returns>The item if found, null otherwise.</returns>
    Item? GetItem(string identifier);

    /// <summary>
    /// Gets a machine by its identifier.
    /// </summary>
    /// <param name="identifier">The machine identifier.</param>
    /// <returns>The machine if found, null otherwise.</returns>
    Machine? GetMachine(string identifier);
} 