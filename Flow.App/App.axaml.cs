using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Flow.App.Views;
using Flow.Core.Services;
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
    }
}