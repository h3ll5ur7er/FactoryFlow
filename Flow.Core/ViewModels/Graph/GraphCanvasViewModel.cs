using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Flow.ViewModels.Graph;

public partial class GraphCanvasViewModel : ObservableObject
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

        return Nodes.Remove(node);
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
            node.IsSelected = true;
    }
} 