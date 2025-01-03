using System;
using System.Collections.ObjectModel;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flow.Core.Models;
using Flow.Core.Services;

namespace Flow.ViewModels.Graph;

public partial class GraphCanvasViewModel : ViewModelBase
{
    private readonly INodeFactory _nodeFactory;
    private readonly IGraphManager _graphManager;

    [ObservableProperty]
    private NodeViewModel? _selectedNode;

    public ObservableCollection<NodeViewModel> Nodes { get; } = new();

    public GraphCanvasViewModel(INodeFactory nodeFactory, IGraphManager graphManager)
    {
        _nodeFactory = nodeFactory;
        _graphManager = graphManager;
    }

    [RelayCommand]
    private void AddRecipeNode()
    {
        var testItem = new Item("test_item", "Test Item");
        var input = new ItemStack(testItem, 1);
        var output = new ItemStack(testItem, 1);
        var machine = new Machine("test_machine", "Test Machine", 100);
        var recipe = new Recipe(
            "test_recipe",
            "Test Recipe",
            new[] { input },
            new[] { output },
            machine,
            TimeSpan.FromSeconds(1));
        var node = _nodeFactory.CreateRecipeNode(recipe, _graphManager);
        _graphManager.AddNode(node);
    }

    [RelayCommand]
    private void AddGenericNode()
    {
        var node = _nodeFactory.CreateNode(NodeType.Generic, _graphManager);
        _graphManager.AddNode(node);
    }

    [RelayCommand]
    private void AddSplergerNode()
    {
        var node = _nodeFactory.CreateNode(NodeType.Splerger, _graphManager);
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

    public void Clear()
    {
        Nodes.Clear();
    }
} 