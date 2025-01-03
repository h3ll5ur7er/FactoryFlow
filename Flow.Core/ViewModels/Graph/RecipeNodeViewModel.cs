using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Flow.Core.Models;
using Flow.Core.Services;

namespace Flow.ViewModels.Graph;

public class RecipeNodeViewModel : NodeViewModel
{
    private Recipe? _recipe;
    public Recipe Recipe
    {
        get => _recipe ?? throw new InvalidOperationException("Recipe is not set");
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            SetProperty(ref _recipe, value);
            Title = value.DisplayName;
        }
    }

    public RecipeNodeViewModel(IGraphManager graphManager) : base(graphManager)
    {
        NodeType = NodeType.Recipe;
        Title = "New Recipe Node";  // Set a default title until Recipe is set
    }
} 