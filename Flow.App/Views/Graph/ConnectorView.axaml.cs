using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Flow.App.Helpers;
using Flow.ViewModels.Graph;

namespace Flow.App.Views.Graph;

public partial class ConnectorView : UserControl
{
    private bool _isDragging;
    private ConnectorViewModel? ViewModel => DataContext as ConnectorViewModel;
    private GraphCanvasView? _canvas;

    public ConnectorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _canvas = this.FindAncestorOfType<GraphCanvasView>();
    }

    private void OnConnectorPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDragging = true;
            e.Handled = true;

            // Start connection creation
            if (_canvas != null && ViewModel != null)
            {
                _canvas.StartConnectionDrag(ViewModel, e);
            }
        }
    }

    private void OnConnectorPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isDragging && _canvas != null && ViewModel != null)
        {
            _canvas.UpdateConnectionDrag(e);
            e.Handled = true;
        }
    }

    private void OnConnectorPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isDragging && _canvas != null && ViewModel != null)
        {
            _isDragging = false;
            e.Pointer.Capture(null); // Release the pointer capture
            _canvas.EndConnectionDrag(e);
            e.Handled = true;
        }
    }
} 