using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Flow.ViewModels.Graph;

namespace Flow.App.Views.Graph;

public partial class GraphCanvasView : UserControl
{
    private Canvas _nodesCanvas;
    private Point _lastPointerPosition;
    private bool _isDragging;
    private NodeViewModel? _draggedNode;

    public GraphCanvasView()
    {
        InitializeComponent();
        _nodesCanvas = this.FindControl<Canvas>("NodesCanvas");

        if (_nodesCanvas != null)
        {
            _nodesCanvas.PointerPressed += (s, e) =>
            {
                if (e.GetCurrentPoint(_nodesCanvas).Properties.IsRightButtonPressed)
                {
                    var position = e.GetPosition(_nodesCanvas);
                    if (DataContext is GraphCanvasViewModel vm)
                    {
                        vm.SetContextMenuPosition(position);
                    }
                }
            };
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _nodesCanvas = this.FindControl<Canvas>("NodesCanvas");
    }

    private void OnNodePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDragging = true;
            _lastPointerPosition = e.GetPosition(this);
            _draggedNode = (sender as Control)?.DataContext as NodeViewModel;
            e.Handled = true;
        }
    }

    private void OnNodePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isDragging && _draggedNode != null)
        {
            var currentPosition = e.GetPosition(this);
            var delta = currentPosition - _lastPointerPosition;
            
            _draggedNode.Position = new Point(
                _draggedNode.Position.X + delta.X,
                _draggedNode.Position.Y + delta.Y
            );
            
            _lastPointerPosition = currentPosition;
            e.Handled = true;
        }
    }

    private void OnNodePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isDragging && _draggedNode != null)
        {
            _isDragging = false;
            var draggedNode = _draggedNode;
            _draggedNode = null;
            e.Handled = true;
            
            Console.WriteLine($"Finished dragging node: {(draggedNode as RecipeNodeViewModel)?.Recipe?.DisplayName ?? draggedNode?.Title} at position {draggedNode?.Position}");
        }
    }
} 