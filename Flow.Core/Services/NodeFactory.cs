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
                node.AddInputConnector(new ConnectorViewModel(ConnectorType.Input));
                node.AddOutputConnector(new ConnectorViewModel(ConnectorType.Output));
                break;
            case NodeType.Splerger:
                node.AddInputConnector(new ConnectorViewModel(ConnectorType.Input));
                node.AddOutputConnector(new ConnectorViewModel(ConnectorType.Output));
                node.AddOutputConnector(new ConnectorViewModel(ConnectorType.Output));
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

        node.AddInputConnector(new ConnectorViewModel(ConnectorType.Input));
        node.AddOutputConnector(new ConnectorViewModel(ConnectorType.Output));

        return node;
    }
} 