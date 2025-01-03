using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Flow.App.Helpers;
using Flow.ViewModels.Graph;

namespace Flow.App.Views.Graph;

public partial class GraphCanvasView : UserControl
{
    private readonly PathGeometry _connectionPath;
    private readonly Path _dragPath;
    private bool _isDraggingConnection;
    private ConnectorViewModel? _dragSourceConnector;
    private Point _dragCurrentPoint;
    private Canvas? _nodesCanvas;
    private Canvas? _connectionsCanvas;
    private Point _contextMenuPosition;

    private GraphCanvasViewModel? ViewModel => DataContext as GraphCanvasViewModel;

    public GraphCanvasView()
    {
        InitializeComponent();

        // Initialize connection path for dragging
        _connectionPath = new PathGeometry();
        _connectionPath.Changed += OnConnectionPathChanged;

        _dragPath = new Path
        {
            Stroke = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
            StrokeThickness = 2,
            Data = _connectionPath,
            ZIndex = 1000, // Ensure it's drawn above nodes
            IsVisible = false, // Start hidden
            IsHitTestVisible = false, // Don't interfere with hit testing
            StrokeLineCap = PenLineCap.Round, // Add round line caps
            StrokeDashArray = null // Ensure no dash pattern
        };

        _dragPath.DataContextChanged += OnDragPathDataContextChanged;
        _dragPath.PropertyChanged += OnDragPathPropertyChanged;

        DataContextChanged += OnDataContextChanged;
    }

    private void OnConnectionPathChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("ConnectionPath Changed:");
        Console.WriteLine($"  Figures count: {_connectionPath.Figures.Count}");
        if (_connectionPath.Figures.Count > 0)
        {
            var figure = _connectionPath.Figures[0];
            Console.WriteLine($"  First figure - StartPoint: {figure.StartPoint}, IsClosed: {figure.IsClosed}");
            Console.WriteLine($"  Segments count: {figure.Segments.Count}");
        }
    }

    private void OnDragPathDataContextChanged(object? sender, EventArgs e)
    {
        Console.WriteLine("DragPath DataContext Changed");
        Console.WriteLine($"  IsVisible: {_dragPath.IsVisible}");
        Console.WriteLine($"  Has Data: {_dragPath.Data != null}");
    }

    private void OnDragPathPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        Console.WriteLine($"DragPath Property Changed: {e.Property.Name}");
        Console.WriteLine($"  Old Value: {e.OldValue}");
        Console.WriteLine($"  New Value: {e.NewValue}");
        Console.WriteLine($"  IsVisible: {_dragPath.IsVisible}");
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // Find the canvases
        _nodesCanvas = this.FindControl<Canvas>("NodesCanvas");
        _connectionsCanvas = this.FindControl<Canvas>("ConnectionsCanvas");
        var temporaryConnectionLayer = this.FindControl<Canvas>("TemporaryConnectionLayer");

        // Add the drag path to the temporary connection layer
        if (temporaryConnectionLayer != null)
        {
            temporaryConnectionLayer.Children.Add(_dragPath);
        }

        // Set up nodes canvas event handlers
        if (_nodesCanvas != null)
        {
            if (_nodesCanvas.ContextMenu != null)
            {
                _nodesCanvas.ContextMenu.Opening += OnContextMenuOpening;
            }
            _nodesCanvas.PointerPressed += OnCanvasPointerPressed;
            _nodesCanvas.PointerMoved += OnCanvasPointerMoved;
            _nodesCanvas.PointerReleased += OnCanvasPointerReleased;
        }

        // Initial render if we have a ViewModel
        if (ViewModel != null)
        {
            RenderAllConnections();
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            // Subscribe to connections collection changes
            if (ViewModel.Connections is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += OnConnectionsCollectionChanged;
            }

            // Subscribe to property changes for each connection
            foreach (var connection in ViewModel.Connections)
            {
                SubscribeToConnectionChanges(connection);
            }

            // Initial render
            RenderAllConnections();
        }
    }

    private void OnConnectionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (ConnectionViewModel connection in e.OldItems)
            {
                UnsubscribeFromConnectionChanges(connection);
            }
        }

        if (e.NewItems != null)
        {
            foreach (ConnectionViewModel connection in e.NewItems)
            {
                SubscribeToConnectionChanges(connection);
            }
        }

        RenderAllConnections();
    }

    private void SubscribeToConnectionChanges(ConnectionViewModel connection)
    {
        if (connection.Source.Node is INotifyPropertyChanged sourceNode)
        {
            sourceNode.PropertyChanged += OnNodePropertyChanged;
        }
        if (connection.Target.Node is INotifyPropertyChanged targetNode)
        {
            targetNode.PropertyChanged += OnNodePropertyChanged;
        }
    }

    private void UnsubscribeFromConnectionChanges(ConnectionViewModel connection)
    {
        if (connection.Source.Node is INotifyPropertyChanged sourceNode)
        {
            sourceNode.PropertyChanged -= OnNodePropertyChanged;
        }
        if (connection.Target.Node is INotifyPropertyChanged targetNode)
        {
            targetNode.PropertyChanged -= OnNodePropertyChanged;
        }
    }

    private void OnNodePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NodeViewModel.Position))
        {
            RenderAllConnections();
        }
    }

    private void RenderAllConnections()
    {
        if (_connectionsCanvas == null || ViewModel == null) return;

        _connectionsCanvas.Children.Clear();

        foreach (var connection in ViewModel.Connections)
        {
            var path = CreateConnectionPath(connection);
            _connectionsCanvas.Children.Add(path);
        }
    }

    private Path CreateConnectionPath(ConnectionViewModel connection)
    {
        var sourcePos = GetConnectorPosition(connection.Source);
        var targetPos = GetConnectorPosition(connection.Target);

        var pathGeometry = new PathGeometry();
        var pathFigure = new PathFigure { 
            StartPoint = sourcePos,
            IsClosed = false  // Explicitly set to false to prevent the path from closing
        };

        // Calculate control points for a smooth curve
        var deltaX = targetPos.X - sourcePos.X;
        var controlPointOffset = Math.Max(Math.Abs(deltaX) * 0.5, 50);
        
        var controlPoint1 = sourcePos + new Point(controlPointOffset, 0);
        var controlPoint2 = targetPos - new Point(controlPointOffset, 0);

        pathFigure.Segments.Add(new BezierSegment
        {
            Point1 = controlPoint1,
            Point2 = controlPoint2,
            Point3 = targetPos
        });

        pathGeometry.Figures.Add(pathFigure);

        return new Path
        {
            Data = pathGeometry,
            Stroke = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
            StrokeThickness = 2,
            StrokeLineCap = PenLineCap.Round,
            StrokeDashArray = null,
            IsHitTestVisible = false
        };
    }

    private void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_nodesCanvas == null) return;

        if (e.GetCurrentPoint(_nodesCanvas).Properties.IsRightButtonPressed)
        {
            _contextMenuPosition = e.GetPosition(_nodesCanvas);
            ViewModel?.SetContextMenuPosition(_contextMenuPosition);
        }
    }

    private void OnContextMenuOpening(object? sender, EventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.SetContextMenuPosition(_contextMenuPosition);
        }
    }

    private Point GetConnectorPosition(ConnectorViewModel connector)
    {
        if (_nodesCanvas == null || connector.Node is not NodeViewModel node) return new Point();

        // Get the node's position
        var nodePos = node.Position;
        
        // Find the node's visual element to get its actual width
        var nodeView = _nodesCanvas.GetVisualDescendants()
            .OfType<Control>()
            .FirstOrDefault(x => x.DataContext == node);
        
        // Default width if we can't find the node view
        var nodeWidth = nodeView?.Bounds.Width ?? 150;
        
        // Calculate vertical offset based on the connector's index
        var connectors = connector.Type == ConnectorType.Input ? node.InputConnectors : node.OutputConnectors;
        var index = connectors.IndexOf(connector);
        var verticalOffset = 32 + (index * 24) + 12; // 40px for header, 24px spacing between connectors, 12px for connector center
        
        // Calculate connector position relative to the node
        var connectorOffset = connector.Type == ConnectorType.Input
            ? new Point(12, verticalOffset) // Left side for inputs, 8px from edge
            : new Point(nodeWidth - 12, verticalOffset); // Right side for outputs, 8px from edge

        // Return absolute position
        return nodePos + connectorOffset;
    }

    private void UpdateConnectionPath(Point start, Point end)
    {
        // If dragging from an input connector, swap start and end points
        if (_dragSourceConnector?.Type == ConnectorType.Input)
        {
            var temp = start;
            start = end;
            end = temp;
        }

        // Create a new path figure
        var pathFigure = new PathFigure { 
            StartPoint = start,
            IsClosed = false
        };
        
        // Calculate control points for a smooth curve
        var deltaX = end.X - start.X;
        var controlPointOffset = Math.Max(Math.Abs(deltaX) * 0.5, 50);
        
        var controlPoint1 = start + new Point(controlPointOffset, 0);
        var controlPoint2 = end - new Point(controlPointOffset, 0);
        
        pathFigure.Segments.Add(new BezierSegment
        {
            Point1 = controlPoint1,
            Point2 = controlPoint2,
            Point3 = end
        });

        Console.WriteLine($"  Path: start={start}, end={end}");

        // Create a new PathGeometry to ensure change notification
        var newGeometry = new PathGeometry();
        newGeometry.Figures.Add(pathFigure);
        _dragPath.Data = newGeometry;
        
        // Ensure the path is visible and updated
        _dragPath.IsVisible = true;
        _dragPath.InvalidateVisual();
        _dragPath.StrokeLineCap = PenLineCap.Round;
        _dragPath.StrokeThickness = 2;
        _dragPath.Stroke = new SolidColorBrush(Color.FromRgb(102, 102, 102));
    }

    public void StartConnectionDrag(ConnectorViewModel sourceConnector, PointerPressedEventArgs e)
    {
        if (_nodesCanvas == null) return;

        _isDraggingConnection = true;
        _dragSourceConnector = sourceConnector;
        _dragCurrentPoint = e.GetPosition(_nodesCanvas);

        // Capture the pointer to ensure we get all pointer events
        e.Pointer.Capture(_nodesCanvas);

        var sourcePos = GetConnectorPosition(sourceConnector);
        Console.WriteLine($"StartConnectionDrag:");
        Console.WriteLine($"  Source connector: {sourceConnector.DisplayName} on {sourceConnector.Node?.Title}");
        Console.WriteLine($"  Source pos: {sourcePos.X}, {sourcePos.Y}");
        Console.WriteLine($"  Mouse pos: {_dragCurrentPoint.X}, {_dragCurrentPoint.Y}");
        Console.WriteLine("  Setting drag path visible");

        // Reset and initialize the drag path
        _connectionPath.Figures.Clear();
        _dragPath.IsVisible = true;
        _dragPath.StrokeThickness = 2;
        _dragPath.Stroke = new SolidColorBrush(Color.FromRgb(102, 102, 102));
        _dragPath.StrokeLineCap = PenLineCap.Round;
        _dragPath.StrokeDashArray = null;

        // Update the connection path
        UpdateConnectionPath(sourcePos, _dragCurrentPoint);
    }

    public void UpdateConnectionDrag(PointerEventArgs e)
    {
        if (!_isDraggingConnection || _dragSourceConnector == null || _nodesCanvas == null)
            return;

        _dragCurrentPoint = e.GetPosition(_nodesCanvas);
        var sourcePos = GetConnectorPosition(_dragSourceConnector);

        Console.WriteLine($"UpdateConnectionDrag:");
        Console.WriteLine($"  Source pos: {sourcePos.X}, {sourcePos.Y}");
        Console.WriteLine($"  Mouse pos: {_dragCurrentPoint.X}, {_dragCurrentPoint.Y}");
        Console.WriteLine($"  Drag path visible: {_dragPath.IsVisible}");

        // Update the connection path
        UpdateConnectionPath(sourcePos, _dragCurrentPoint);
    }

    public void EndConnectionDrag(PointerReleasedEventArgs e)
    {
        if (!_isDraggingConnection || _dragSourceConnector == null || _nodesCanvas == null)
            return;

        // Release the pointer capture
        e.Pointer.Capture(null);

        var position = e.GetPosition(_nodesCanvas);
        var targetConnector = FindConnectorAtPoint(position);
        
        Console.WriteLine($"EndConnectionDrag:");
        Console.WriteLine($"  Source connector: {_dragSourceConnector.DisplayName} on {_dragSourceConnector.Node?.Title}");
        Console.WriteLine($"  Target connector: {targetConnector?.DisplayName ?? ""} on {targetConnector?.Node?.Title ?? ""}");
        Console.WriteLine($"  Mouse pos: {position.X}, {position.Y}");

        if (targetConnector != null && CanConnect(_dragSourceConnector, targetConnector))
        {
            ViewModel?.CreateConnection(_dragSourceConnector, targetConnector);
        }

        // Reset dragging state
        _dragSourceConnector = null;
        _isDraggingConnection = false;
        _dragPath.IsVisible = false;
        _connectionPath.Figures.Clear();
        _dragPath.InvalidateVisual(); // Force visual update
    }

    private ConnectorViewModel? FindConnectorAtPoint(Point point)
    {
        if (_nodesCanvas == null) return null;

        // Find the visual element under the point
        var element = _nodesCanvas.InputHitTest(point);
        
        // Walk up the visual tree to find a ConnectorView
        var connectorView = (element as Visual)?.FindAncestorOfType<ConnectorView>();
        
        // Get the connector view model
        if (connectorView?.DataContext is ConnectorViewModel connector)
        {
            // Don't return the source connector
            if (connector != _dragSourceConnector)
            {
                return connector;
            }
        }

        return null;
    }

    private bool CanConnect(ConnectorViewModel source, ConnectorViewModel target)
    {
        // One must be input, one must be output
        if (source.Type == target.Type) return false;

        // Ensure proper input/output order
        var (output, input) = source.Type == ConnectorType.Output
            ? (source, target)
            : (target, source);

        // Check if connection is allowed
        return output.CanConnectTo(input);
    }

    // Node drag handling
    private bool _isDraggingNode;
    private Point _lastNodeDragPoint;
    private NodeViewModel? _draggedNode;

    public void OnNodePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_nodesCanvas == null) return;

        if (e.Source is Control { DataContext: NodeViewModel node } && 
            e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDraggingNode = true;
            _lastNodeDragPoint = e.GetPosition(_nodesCanvas);
            _draggedNode = node;
            e.Handled = true;
        }
    }

    public void OnNodePointerMoved(object? sender, PointerEventArgs e)
    {
        if (_nodesCanvas == null) return;

        if (_isDraggingNode && _draggedNode != null)
        {
            var currentPoint = e.GetPosition(_nodesCanvas);
            var delta = currentPoint - _lastNodeDragPoint;
            
            _draggedNode.Position = _draggedNode.Position + delta;
            _lastNodeDragPoint = currentPoint;
            
            e.Handled = true;
        }
    }

    public void OnNodePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDraggingNode = false;
        _draggedNode = null;
        e.Handled = true;
    }

    private void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isDraggingConnection)
        {
            UpdateConnectionDrag(e);
            e.Handled = true;
        }
    }

    private void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_isDraggingConnection)
        {
            EndConnectionDrag(e);
            e.Handled = true;
        }
    }
} 