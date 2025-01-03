using System;
using Avalonia;
using Flow.Core.Models;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class NodeFactory : INodeFactory
{
    public NodeViewModel CreateNode(NodeType type, IGraphManager graphManager)
    {
        var node = new NodeViewModel(graphManager)
        {
            Title = $"New {type} Node",
            NodeType = type,
            Position = new Point(100, 100)
        };

        switch (type)
        {
            case NodeType.Generic:
                node.AddInputConnector(new ConnectorViewModel("input", "Input", node, ConnectorType.Input, true));
                node.AddOutputConnector(new ConnectorViewModel("output", "Output", node, ConnectorType.Output, true));
                break;
            case NodeType.Splerger:
                node.AddInputConnector(new ConnectorViewModel("input", "Input", node, ConnectorType.Input, true));
                node.AddOutputConnector(new ConnectorViewModel("output1", "Output 1", node, ConnectorType.Output, true));
                node.AddOutputConnector(new ConnectorViewModel("output2", "Output 2", node, ConnectorType.Output, true));
                break;
        }

        return node;
    }

    public RecipeNodeViewModel CreateRecipeNode(Recipe recipe, IGraphManager graphManager)
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));

        var node = new RecipeNodeViewModel(graphManager)
        {
            Title = recipe.DisplayName,
            Recipe = recipe,
            Position = new Point(100, 100)
        };

        return node;
    }
} 