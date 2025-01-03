using Flow.Core.Models;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public interface INodeFactory
{
    NodeViewModel CreateNode(NodeType type);
    RecipeNodeViewModel CreateRecipeNode(Recipe recipe);
    // Add other node creation methods as needed
} 