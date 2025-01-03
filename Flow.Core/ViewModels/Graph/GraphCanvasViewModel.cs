using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Flow.ViewModels.Graph;

public partial class GraphCanvasViewModel : ViewModelBase
{
    [ObservableProperty]
    private double _scale = 1.0;

    [ObservableProperty]
    private Point _offset;

    [ObservableProperty]
    private NodeViewModel? _selectedNode;

    public ObservableCollection<NodeViewModel> Nodes { get; } = new();

    public void AddNode(NodeViewModel node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        Nodes.Add(node);
        Debug.WriteLine($"Added node {node.Title} to graph. Node count: {Nodes.Count}");
    }

    public bool RemoveNode(NodeViewModel node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        // Disconnect all connectors
        foreach (var connector in node.InputConnectors.ToList())
        {
            connector.DisconnectAll();
        }
        foreach (var connector in node.OutputConnectors.ToList())
        {
            connector.DisconnectAll();
        }

        // Clear selection if this node was selected
        if (node == SelectedNode)
        {
            SelectNode(null);
        }

        var result = Nodes.Remove(node);
        Debug.WriteLine($"Removed node {node.Title} from graph. Node count: {Nodes.Count}");
        return result;
    }

    public void SelectNode(NodeViewModel? node)
    {
        if (node == SelectedNode)
            return;

        if (node != null && !Nodes.Contains(node))
            throw new ArgumentException("Node is not in the canvas.", nameof(node));

        if (SelectedNode != null)
            SelectedNode.IsSelected = false;

        SelectedNode = node;

        if (node != null)
        {
            node.IsSelected = true;
            Debug.WriteLine($"Selected node: {node.Title}");
        }
        else
        {
            Debug.WriteLine("Cleared node selection");
        }
    }
} 