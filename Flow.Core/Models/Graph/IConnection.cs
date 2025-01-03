namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents a connection between two connectors in the graph.
/// </summary>
public interface IConnection
{
    /// <summary>
    /// Gets the source connector of the connection.
    /// </summary>
    IConnector Source { get; }

    /// <summary>
    /// Gets the target connector of the connection.
    /// </summary>
    IConnector Target { get; }

    /// <summary>
    /// Gets the rate at which items flow through this connection.
    /// </summary>
    decimal FlowRate { get; }

    /// <summary>
    /// Gets whether this connection is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Validates whether this connection is valid.
    /// </summary>
    /// <returns>True if the connection is valid, false otherwise.</returns>
    bool Validate();
} 