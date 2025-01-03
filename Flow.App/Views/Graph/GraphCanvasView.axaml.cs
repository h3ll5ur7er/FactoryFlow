using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Flow.ViewModels.Graph;

namespace Flow.App.Views.Graph;

public partial class GraphCanvasView : UserControl
{
    private Point? _dragStart;
    private Point? _nodeStartPosition;
    private NodeViewModel? _draggedNode;

    public GraphCanvasView()
    {
        InitializeComponent();
        Console.WriteLine("GraphCanvasView: Created");
        
        this.DataContextChanged += (s, e) =>
        {
            Console.WriteLine($"GraphCanvasView: DataContext changed");
            if (DataContext is GraphCanvasViewModel vm)
            {
                Console.WriteLine($"GraphCanvasView: DataContext is GraphCanvasViewModel with {vm.Nodes.Count} nodes");
                foreach (var node in vm.Nodes)
                {
                    Console.WriteLine($"Node in collection:");
                    Console.WriteLine($"  - Type: {node.GetType().Name}");
                    Console.WriteLine($"  - Title: {node.Title}");
                    Console.WriteLine($"  - Position: {node.Position}");
                    Console.WriteLine($"  - NodeType: {node.NodeType}");
                    if (node is RecipeNodeViewModel recipeNode)
                    {
                        Console.WriteLine($"  - Recipe: {recipeNode.Recipe.DisplayName}");
                    }
                }

                // Log ItemsControl state
                var itemsControl = this.FindControl<ItemsControl>("NodesItemsControl");
                if (itemsControl != null)
                {
                    Console.WriteLine($"ItemsControl found:");
                    Console.WriteLine($"  - ItemsSource bound: {itemsControl.ItemsSource != null}");
                    Console.WriteLine($"  - ItemTemplate: {itemsControl.ItemTemplate != null}");
                    Console.WriteLine($"  - IsVisible: {itemsControl.IsVisible}");
                    Console.WriteLine($"  - Bounds: {itemsControl.Bounds}");
                }
                else
                {
                    Console.WriteLine("WARNING: ItemsControl not found!");
                }
            }
            else
            {
                Console.WriteLine($"GraphCanvasView: DataContext is {DataContext?.GetType().Name ?? "null"}");
            }
        };
    }

    private void OnNodePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Control control || control.DataContext is not NodeViewModel node)
            return;

        if (e.GetCurrentPoint(control).Properties.IsRightButtonPressed)
        {
            // Let the right-click event bubble up to show the context menu
            return;
        }

        Console.WriteLine($"Started dragging node: {node.Title} from position {node.Position}");
        _dragStart = e.GetPosition(this.FindControl<Canvas>("NodesCanvas"));
        _nodeStartPosition = node.Position;
        _draggedNode = node;
        e.Pointer.Capture(control);
    }

    private void OnNodePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_dragStart == null || _nodeStartPosition == null || _draggedNode == null)
            return;

        var currentPosition = e.GetPosition(this.FindControl<Canvas>("NodesCanvas"));
        var delta = currentPosition - _dragStart.Value;
        var newPosition = _nodeStartPosition.Value + delta;
        _draggedNode.SetPosition(newPosition.X, newPosition.Y);
    }

    private void OnNodePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is Control control)
        {
            e.Pointer.Capture(null);
            if (_draggedNode != null)
            {
                Console.WriteLine($"Finished dragging node: {_draggedNode.Title} at position {_draggedNode.Position}");
            }
        }

        _dragStart = null;
        _nodeStartPosition = null;
        _draggedNode = null;
    }
} 