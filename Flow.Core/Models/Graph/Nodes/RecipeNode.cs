using Flow.Core.Models;

namespace Flow.Core.Models.Graph.Nodes;

/// <summary>
/// Represents a node that performs a production recipe.
/// </summary>
public class RecipeNode : Node
{
    private decimal _multiplier = 1;

    public Recipe Recipe { get; }

    /// <summary>
    /// Gets or sets the multiplier for this node (number of machines).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the value is not positive.</exception>
    public decimal Multiplier
    {
        get => _multiplier;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Multiplier must be positive.", nameof(value));
            _multiplier = value;
        }
    }

    /// <summary>
    /// Creates a new recipe node.
    /// </summary>
    /// <param name="recipe">The recipe to perform.</param>
    /// <exception cref="ArgumentNullException">Thrown when recipe is null.</exception>
    public RecipeNode(Recipe recipe) : base($"recipe-{recipe?.Identifier ?? throw new ArgumentNullException(nameof(recipe))}", recipe.DisplayName)
    {
        Recipe = recipe;

        Console.WriteLine($"[RecipeNode] Creating node for recipe: {recipe.DisplayName}");

        // Create input connectors for each recipe input
        foreach (var input in recipe.Inputs)
        {
            Console.WriteLine($"[RecipeNode] Creating input connector: {input.Item.DisplayName}");
            var connector = new Connector(
                $"input-{input.Item.Identifier}",
                input.Item.DisplayName,
                this,
                true,  // isInput
                true,  // multiple connections allowed for inputs
                new[] { input.Item }
            );
            AddInput(connector);
        }

        // Create output connectors for each recipe output
        foreach (var output in recipe.Outputs)
        {
            Console.WriteLine($"[RecipeNode] Creating output connector: {output.Item.DisplayName}");
            var connector = new Connector(
                $"output-{output.Item.Identifier}",
                output.Item.DisplayName,
                this,
                false,  // isInput
                true,   // multiple connections allowed for outputs
                new[] { output.Item }
            );
            AddOutput(connector);
        }
    }

    /// <summary>
    /// Calculates the throughput of this node based on the recipe and multiplier.
    /// </summary>
    /// <returns>The calculated throughput.</returns>
    public Throughput GetThroughput()
    {
        // Calculate items per minute based on recipe time and multiplier
        var cyclesPerMinute = 60m / (decimal)Recipe.ProcessingTime.TotalSeconds * Multiplier;

        var inputs = Recipe.Inputs.ToDictionary(
            input => input.Item,
            input => input.Amount * cyclesPerMinute
        );

        var outputs = Recipe.Outputs.ToDictionary(
            output => output.Item,
            output => output.Amount * cyclesPerMinute
        );

        return new Throughput(
            inputs,
            outputs,
            Recipe.Machine.PowerConsumption * Multiplier
        );
    }
} 