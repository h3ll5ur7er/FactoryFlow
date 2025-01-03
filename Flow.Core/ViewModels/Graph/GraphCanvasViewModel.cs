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
        Nodes.Remove(node);
    }

    private Point _contextMenuPosition;
    public void SetContextMenuPosition(Point position)
    {
        _contextMenuPosition = position;
    }
} 