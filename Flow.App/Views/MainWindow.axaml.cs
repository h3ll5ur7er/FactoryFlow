using System;
using System.Diagnostics;
using Avalonia.Controls;
using Flow.Core.Models;
using Flow.Core.Services;
using Flow.ViewModels.Graph;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        try
        {
            // Get services
            var services = App.Current?.Services ?? throw new InvalidOperationException("Services not initialized!");
            
            var graphManager = services.GetRequiredService<IGraphManager>();
            var nodeFactory = services.GetRequiredService<INodeFactory>();

            Console.WriteLine($"GraphManager: {graphManager != null}");
            Console.WriteLine($"NodeFactory: {nodeFactory != null}");

            // Create a test recipe
            var inputItem = new Item("input", "Input Item");
            var outputItem = new Item("output", "Output Item");
            var machine = new Machine("machine", "Test Machine", 100);
            var recipe = new Recipe(
                "test",
                "Test Recipe",
                new[] { new ItemStack(inputItem, 1) },
                new[] { new ItemStack(outputItem, 1) },
                machine,
                TimeSpan.FromSeconds(1));

            // Create a test node
            var node = nodeFactory.CreateRecipeNode(recipe, graphManager);
            node.Position = new Avalonia.Point(100, 100);
            Console.WriteLine($"Created node: {node.Title} at position {node.Position}");

            // Add node to graph
            graphManager.AddNode(node);
            Console.WriteLine($"Added node to graph. Node count: {graphManager.CurrentGraph?.Nodes.Count ?? 0}");

            // Set DataContext
            DataContext = graphManager.CurrentGraph;
            Console.WriteLine($"Set DataContext to graph. DataContext type: {DataContext?.GetType().Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
        }
    }
} 