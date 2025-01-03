using System;
using System.Collections.Generic;

namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents a connector on a node that can be connected to other connectors.
/// </summary>
public class Connector : IConnector
{
    private readonly List<IConnection> _connections = new();
    private readonly List<Item> _acceptedItems = new();

    public string Identifier { get; }
    public string DisplayName { get; }
    public INode Parent { get; }
    public bool IsInput { get; }
    public bool AllowsMultipleConnections { get; }
    public IReadOnlyCollection<Item> AcceptedItems => _acceptedItems.AsReadOnly();
    public IReadOnlyCollection<IConnection> Connections => _connections.AsReadOnly();

    /// <summary>
    /// Creates a new connector.
    /// </summary>
    /// <param name="identifier">The unique identifier of the connector.</param>
    /// <param name="displayName">The display name of the connector.</param>
    /// <param name="parent">The node that owns this connector.</param>
    /// <param name="isInput">Whether this is an input connector.</param>
    /// <param name="allowsMultipleConnections">Whether this connector allows multiple connections.</param>
    /// <param name="acceptedItems">The items that this connector accepts. If empty, accepts any item.</param>
    /// <exception cref="ArgumentException">Thrown when identifier or displayName is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when parent is null.</exception>
    public Connector(
        string identifier,
        string displayName,
        INode parent,
        bool isInput,
        bool allowsMultipleConnections,
        IEnumerable<Item>? acceptedItems = null)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier cannot be empty.", nameof(identifier));
            
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty.", nameof(displayName));

        Identifier = identifier;
        DisplayName = displayName;
        Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        IsInput = isInput;
        AllowsMultipleConnections = allowsMultipleConnections;

        if (acceptedItems != null)
            _acceptedItems.AddRange(acceptedItems);
    }

    public void AddConnection(IConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        if (!AllowsMultipleConnections && _connections.Count > 0)
            throw new InvalidOperationException($"Connector '{Identifier}' does not allow multiple connections.");

        if ((IsInput && connection.Target != this) || (!IsInput && connection.Source != this))
            throw new ArgumentException("Connection does not reference this connector correctly.");

        _connections.Add(connection);
    }

    public void RemoveConnection(IConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connections.Remove(connection);
    }

    public bool CanConnectTo(IConnector other)
    {
        ArgumentNullException.ThrowIfNull(other);

        // Cannot connect to self
        if (other == this)
            return false;

        // Cannot connect input to input or output to output
        if (IsInput == other.IsInput)
            return false;

        // If either connector accepts no items, they can connect
        if (!AcceptedItems.Any() || !other.AcceptedItems.Any())
            return true;

        // Check if there are any compatible item types
        return AcceptedItems.Intersect(other.AcceptedItems).Any();
    }

    public bool ValidateConnections()
    {
        // Validate number of connections
        if (!AllowsMultipleConnections && Connections.Count > 1)
            return false;

        // Validate each connection
        foreach (var connection in Connections)
        {
            // Check if this connector is properly referenced in the connection
            var isSource = connection.Source == this;
            var isTarget = connection.Target == this;
            
            if (!isSource && !isTarget)
                return false;

            // Check if the connection direction matches the connector type
            if (IsInput && !isTarget)
                return false;
                
            if (!IsInput && !isSource)
                return false;

            // Validate the connection itself
            if (!connection.Validate())
                return false;
        }

        return true;
    }
} 