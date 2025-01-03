using Flow.Core.Models;
using Flow.Core.Plugins;

namespace Flow.Core.Services;

/// <summary>
/// Implementation of the game registry service.
/// </summary>
public class GameRegistry : IGameRegistry
{
    private readonly Dictionary<string, IGamePlugin> _games = new();
    private IGamePlugin? _activePlugin;

    public GameInfo? ActiveGame => _activePlugin != null ? new GameInfo(_activePlugin.GameName, _activePlugin.Version) : null;

    public IReadOnlyCollection<GameInfo> AvailableGames => _games.Values
        .Select(p => new GameInfo(p.GameName, p.Version))
        .ToList();

    public IReadOnlyCollection<Item> Items => _activePlugin?.Items ?? Array.Empty<Item>();
    public IReadOnlyCollection<Machine> Machines => _activePlugin?.Machines ?? Array.Empty<Machine>();
    public IReadOnlyCollection<Recipe> Recipes => _activePlugin?.Recipes ?? Array.Empty<Recipe>();

    public void RegisterGame(IGamePlugin plugin)
    {
        ArgumentNullException.ThrowIfNull(plugin);

        if (_games.ContainsKey(plugin.GameName))
        {
            throw new ArgumentException($"A game with name '{plugin.GameName}' is already registered.");
        }

        _games.Add(plugin.GameName, plugin);

        // If this is the first game registered, make it active
        if (_activePlugin == null)
        {
            SetActiveGame(plugin.GameName);
        }
    }

    public bool SetActiveGame(string gameName)
    {
        if (!_games.TryGetValue(gameName, out var plugin))
        {
            return false;
        }

        _activePlugin = plugin;
        return true;
    }

    public Recipe? GetRecipe(string identifier)
    {
        return _activePlugin?.Recipes.FirstOrDefault(r => r.Identifier == identifier);
    }

    public Item? GetItem(string identifier)
    {
        return _activePlugin?.Items.FirstOrDefault(i => i.Identifier == identifier);
    }

    public Machine? GetMachine(string identifier)
    {
        return _activePlugin?.Machines.FirstOrDefault(m => m.Identifier == identifier);
    }
} 