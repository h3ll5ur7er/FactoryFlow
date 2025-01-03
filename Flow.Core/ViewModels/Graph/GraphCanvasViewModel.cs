using System;
using System.Collections.ObjectModel;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flow.Core.Models;
using Flow.Core.Services;

namespace Flow.ViewModels.Graph;

public partial class GraphCanvasViewModel : ObservableObject
{
    private readonly INodeFactory _nodeFactory;
    private readonly IGraphManager _graphManager;
    private readonly IGameRegistry _gameRegistry;

    [ObservableProperty]
    private NodeViewModel? _selectedNode;

    public ObservableCollection<NodeViewModel> Nodes { get; } = new();
    public ObservableCollection<ConnectionViewModel> Connections { get; } = new();

    public List<Recipe> AvailableRecipes => _gameRegistry.Recipes.ToList();

    public GraphCanvasViewModel(INodeFactory nodeFactory, IGraphManager graphManager, IGameRegistry gameRegistry)
    {
        _nodeFactory = nodeFactory;
        _graphManager = graphManager;
        _gameRegistry = gameRegistry;
    }

    [RelayCommand]
    private void AddRecipeNode(Recipe recipe)
    {
        var node = _nodeFactory.CreateRecipeNode(recipe, _graphManager);
        node.Position = _contextMenuPosition;
        _graphManager.AddNode(node);
    }

    [RelayCommand]
    private void AddGenericNode(Point position)
    {
        var node = _nodeFactory.CreateNode(NodeType.Generic, _graphManager);
        node.Position = position;
        _graphManager.AddNode(node);
    }

    [RelayCommand]
    private void AddSplergerNode(Point position)
    {
        var node = _nodeFactory.CreateNode(NodeType.Splerger, _graphManager);
        node.Position = position;
        _graphManager.AddNode(node);
    }

    public void AddNode(NodeViewModel node)
    {
        Nodes.Add(node);
    }

    public void RemoveNode(NodeViewModel node)
    {
        // Remove all connections associated with this node
        var connectionsToRemove = Connections
            .Where(c => c.Source.Node == node || c.Target.Node == node)
            .ToList();

        foreach (var connection in connectionsToRemove)
        {
            RemoveConnection(connection);
        }

        Nodes.Remove(node);
    }

    public void CreateConnection(ConnectorViewModel source, ConnectorViewModel target)
    {
        // Ensure one is input and one is output
        if (source.Type == target.Type) return;

        // Order them correctly (output -> input)
        var (output, input) = source.Type == ConnectorType.Output
            ? (source, target)
            : (target, source);

        // Check if connection is allowed
        if (!output.CanConnectTo(input)) return;

        // Create the connection
        var connection = new ConnectionViewModel(output, input);
        Connections.Add(connection);

        // Update connector states
        output.AddConnection(connection);
        input.AddConnection(connection);
    }

    public void RemoveConnection(ConnectionViewModel connection)
    {
        connection.Source.RemoveConnection(connection);
        connection.Target.RemoveConnection(connection);
        Connections.Remove(connection);
    }

    private Point _contextMenuPosition;
    public void SetContextMenuPosition(Point position)
    {
        _contextMenuPosition = position;
    }
} 