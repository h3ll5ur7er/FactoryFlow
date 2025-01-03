using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Flow.Core.Models;

namespace Flow.ViewModels.Graph;

public partial class RecipeNodeViewModel : NodeViewModel
{
    private Recipe _recipe = null!;
    public Recipe Recipe
    {
        get => _recipe;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // Disconnect all existing connectors
            foreach (var connector in InputConnectors.ToList())
            {
                connector.DisconnectAll();
            }
            foreach (var connector in OutputConnectors.ToList())
            {
                connector.DisconnectAll();
            }

            InputConnectors.Clear();
            OutputConnectors.Clear();

            _recipe = value;
            Title = value.DisplayName;
            
            // Create new connectors for inputs
            foreach (var input in value.Inputs)
            {
                var connector = new ConnectorViewModel(ConnectorType.Input)
                {
                    AllowMultipleConnections = false
                };
                connector.AcceptedTypes.Add(input.Item.GetType());
                AddInputConnector(connector);
            }
            
            // Create new connectors for outputs
            foreach (var output in value.Outputs)
            {
                var connector = new ConnectorViewModel(ConnectorType.Output)
                {
                    AllowMultipleConnections = true
                };
                connector.AcceptedTypes.Add(output.Item.GetType());
                AddOutputConnector(connector);
            }
        }
    }

    public RecipeNodeViewModel(Recipe recipe)
        : base()
    {
        if (recipe == null)
            throw new ArgumentNullException(nameof(recipe));

        NodeType = NodeType.Recipe;
        Recipe = recipe;
    }
} 