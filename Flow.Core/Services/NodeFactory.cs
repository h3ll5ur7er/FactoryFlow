using Flow.Core.Models;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class NodeFactory : INodeFactory
{
    public NodeViewModel CreateNode(NodeType type)
    {
        return new NodeViewModel
        {
            NodeType = type,
            Title = $"New {type} Node"
        };
    }

    public RecipeNodeViewModel CreateRecipeNode(Recipe recipe)
    {
        return new RecipeNodeViewModel(recipe);
    }
} 