using Flow.Core.Models;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public interface INodeFactory
{
    NodeViewModel CreateNode(NodeType type, IGraphManager graphManager);
    RecipeNodeViewModel CreateRecipeNode(Recipe recipe, IGraphManager graphManager);
} 