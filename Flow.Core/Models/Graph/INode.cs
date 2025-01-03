namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents a node in the production flow graph.
/// </summary>
public interface INode
{
    /// <summary>
    /// Gets the unique identifier of the node.
    /// </summary>
    string Identifier { get; }

    /// <summary>
    /// Gets the display name of the node.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets the input connectors of the node.
    /// </summary>
    IReadOnlyCollection<IConnector> Inputs { get; }

    /// <summary>
    /// Gets the output connectors of the node.
    /// </summary>
    IReadOnlyCollection<IConnector> Outputs { get; }

    /// <summary>
    /// Gets or sets the X position of the node in the graph.
    /// </summary>
    double X { get; set; }

    /// <summary>
    /// Gets or sets the Y position of the node in the graph.
    /// </summary>
    double Y { get; set; }

    /// <summary>
    /// Validates the node's current state.
    /// </summary>
    /// <returns>True if the node is in a valid state, false otherwise.</returns>
    bool Validate();
} 