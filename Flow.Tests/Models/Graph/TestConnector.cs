using Flow.Core.Models;
using Flow.Core.Models.Graph;
using Xunit.Abstractions;

namespace Flow.Tests.Models.Graph;

/// <summary>
/// A simple implementation of IConnector for testing purposes.
/// </summary>
public class TestConnector : IConnector
{
    private readonly List<IConnection> _connections = new();
    private readonly List<Item> _acceptedItems = new();
    private readonly ITestOutputHelper? _output;

    public string Identifier { get; }
    public string DisplayName { get; }
    public INode Parent { get; }
    public bool IsInput { get; }
    public bool AllowsMultipleConnections { get; }
    public IReadOnlyCollection<Item> AcceptedItems => _acceptedItems.AsReadOnly();
    public IReadOnlyCollection<IConnection> Connections => _connections.AsReadOnly();

    public TestConnector(
        string identifier,
        string displayName,
        INode parent,
        bool isInput,
        bool allowsMultipleConnections,
        IEnumerable<Item>? acceptedItems = null,
        ITestOutputHelper? output = null)
    {
        Identifier = identifier;
        DisplayName = displayName;
        Parent = parent;
        IsInput = isInput;
        AllowsMultipleConnections = allowsMultipleConnections;
        _output = output;

        if (acceptedItems != null)
            _acceptedItems.AddRange(acceptedItems);
    }

    public void AddConnection(IConnection connection)
    {
        if (!AllowsMultipleConnections && _connections.Any())
            throw new InvalidOperationException("This connector does not allow multiple connections.");

        _connections.Add(connection);
    }

    public void RemoveConnection(IConnection connection)
    {
        _connections.Remove(connection);
    }

    public bool CanConnectTo(IConnector other)
    {
        // Basic validation rules:
        // 1. Cannot connect to self
        if (other == this)
        {
            _output?.WriteLine($"[{Identifier}] Cannot connect to self");
            return false;
        }

        // 2. Cannot connect input to input or output to output
        if (IsInput == other.IsInput)
        {
            _output?.WriteLine($"[{Identifier}] Cannot connect {(IsInput ? "input" : "output")} to {(other.IsInput ? "input" : "output")}");
            return false;
        }

        // 3. Must have compatible item types
        // If either connector accepts no items, they can connect
        // This is useful for testing or special node types
        if (!AcceptedItems.Any() || !other.AcceptedItems.Any())
            return true;

        // Check if there are any compatible item types
        var hasCompatibleItems = AcceptedItems.Intersect(other.AcceptedItems).Any();
        if (!hasCompatibleItems)
        {
            _output?.WriteLine($"[{Identifier}] No compatible item types with {other.Identifier}");
            return false;
        }

        return true;
    }

    public bool ValidateConnections()
    {
        // Validate number of connections
        if (!AllowsMultipleConnections && Connections.Count > 1)
        {
            _output?.WriteLine($"[{Identifier}] Too many connections: {Connections.Count}");
            return false;
        }

        // Validate each connection
        foreach (var connection in Connections)
        {
            // Check if this connector is properly referenced in the connection
            var isSource = connection.Source == this;
            var isTarget = connection.Target == this;
            if (!isSource && !isTarget)
            {
                _output?.WriteLine($"[{Identifier}] Not referenced in connection");
                return false;
            }

            // Check if the connection direction matches the connector type
            if (IsInput && !isTarget)
            {
                _output?.WriteLine($"[{Identifier}] Input connector not used as target");
                return false;
            }
            if (!IsInput && !isSource)
            {
                _output?.WriteLine($"[{Identifier}] Output connector not used as source");
                return false;
            }

            // Validate the connection itself
            if (!connection.Validate())
            {
                _output?.WriteLine($"[{Identifier}] Connection validation failed");
                return false;
            }
        }

        return true;
    }
} 