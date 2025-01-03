using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Flow.App.Views;
using Flow.Core.Services;
using Flow.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.App;

public partial class App : Application
{
    public new static App? Current => Application.Current as App;
    
    public IServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Configure services
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        // Initialize plugin system and game registry
        var pluginLoader = Services.GetRequiredService<PluginLoader>();
        var gameRegistry = Services.GetRequiredService<IGameRegistry>();
        var pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        Console.WriteLine($"Looking for plugins in: {pluginDirectory}");
        
        try
        {
            Directory.CreateDirectory(pluginDirectory);
            var loadedCount = pluginLoader.LoadPluginsFromDirectory(pluginDirectory);
            Console.WriteLine($"Loaded {loadedCount} plugins");
            
            var availableGames = gameRegistry.AvailableGames;
            foreach (var game in availableGames)
            {
                Console.WriteLine($"Available game: {game.Name} v{game.Version}");
            }

            if (gameRegistry.ActiveGame != null)
            {
                var activeGame = gameRegistry.ActiveGame;
                Console.WriteLine($"Active game: {activeGame.Name} v{activeGame.Version}");
                Console.WriteLine($"  Items: {gameRegistry.Items.Count}");
                Console.WriteLine($"  Machines: {gameRegistry.Machines.Count}");
                Console.WriteLine($"  Recipes: {gameRegistry.Recipes.Count}");
            }
            else
            {
                Console.WriteLine("No active game selected");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading plugins: {ex.Message}");
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<IGraphManager>().CurrentGraph
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register services
        services.AddSingleton<INodeFactory, NodeFactory>();
        services.AddSingleton<IGraphManager, GraphManager>();
        services.AddSingleton<IUIActionManager, UIActionManager>();
        services.AddSingleton<IGameRegistry, GameRegistry>();
        services.AddSingleton<PluginLoader>();
    }
}