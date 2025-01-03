using Flow.Core.Models;

namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents a connector on a node that can be connected to other connectors.
/// </summary>
public interface IConnector
{
    /// <summary>
    /// The unique identifier of the connector.
    /// </summary>
    string Identifier { get; }

    /// <summary>
    /// The display name of the connector.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// The node that owns this connector.
    /// </summary>
    INode Parent { get; }

    /// <summary>
    /// Whether this is an input connector (true) or an output connector (false).
    /// </summary>
    bool IsInput { get; }

    /// <summary>
    /// Whether this connector allows multiple connections.
    /// </summary>
    bool AllowsMultipleConnections { get; }

    /// <summary>
    /// The items that this connector accepts.
    /// If empty, the connector accepts any item.
    /// </summary>
    IReadOnlyCollection<Item> AcceptedItems { get; }

    /// <summary>
    /// The connections attached to this connector.
    /// </summary>
    IReadOnlyCollection<IConnection> Connections { get; }

    /// <summary>
    /// Adds a connection to this connector.
    /// </summary>
    /// <param name="connection">The connection to add.</param>
    /// <exception cref="InvalidOperationException">Thrown when the connector does not allow multiple connections and already has a connection.</exception>
    void AddConnection(IConnection connection);

    /// <summary>
    /// Removes a connection from this connector.
    /// </summary>
    /// <param name="connection">The connection to remove.</param>
    void RemoveConnection(IConnection connection);

    /// <summary>
    /// Checks if this connector can connect to another connector.
    /// </summary>
    /// <param name="other">The other connector to check.</param>
    /// <returns>True if the connectors can be connected, false otherwise.</returns>
    bool CanConnectTo(IConnector other);

    /// <summary>
    /// Validates all connections attached to this connector.
    /// </summary>
    /// <returns>True if all connections are valid, false otherwise.</returns>
    bool ValidateConnections();
} 