using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Flow.ViewModels.Graph;

namespace Flow.App.Views.Graph;

public partial class NodeView : UserControl
{
    public NodeView()
    {
        InitializeComponent();
        Console.WriteLine("NodeView: Created");

        this.DataContextChanged += (s, e) =>
        {
            if (DataContext is NodeViewModel node)
            {
                // Console.WriteLine($"NodeView: DataContext set to NodeViewModel:");
                // Console.WriteLine($"  - Title: {node.Title}");
                // Console.WriteLine($"  - Position: {node.Position}");
                // Console.WriteLine($"  - NodeType: {node.NodeType}");
                // Console.WriteLine($"  - Size: {node.Size}");
                // Console.WriteLine($"  - IsSelected: {node.IsSelected}");
                // Console.WriteLine($"  - Input Connectors: {node.InputConnectors.Count}");
                // Console.WriteLine($"  - Output Connectors: {node.OutputConnectors.Count}");
                
                // if (node is RecipeNodeViewModel recipeNode)
                // {
                //     Console.WriteLine($"  - Recipe: {recipeNode.Recipe.DisplayName}");
                // }
            }
            else
            {
                Console.WriteLine($"NodeView: DataContext set to {DataContext?.GetType().Name ?? "null"}");
            }
        };
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            // Let the right-click event bubble up to show the context menu
            return;
        }
        
        base.OnPointerPressed(e);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
} 