using System.Collections.ObjectModel;

namespace Flow.Core.Models.Graph;

public class Graph
{
    private readonly List<INode> _nodes = new();
    private readonly List<IConnection> _connections = new();

    public IReadOnlyCollection<INode> Nodes => _nodes.AsReadOnly();
    public IReadOnlyCollection<IConnection> Connections => _connections.AsReadOnly();

    public void AddNode(INode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        if (_nodes.Contains(node)) throw new ArgumentException("Node already exists in graph", nameof(node));
        
        _nodes.Add(node);
    }

    public void RemoveNode(INode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        // Remove all connections associated with this node
        var connectionsToRemove = _connections
            .Where(c => c.Source.Parent == node || c.Target.Parent == node)
            .ToList();

        foreach (var connection in connectionsToRemove)
        {
            RemoveConnection(connection);
        }

        _ = _nodes.Remove(node);
    }

    public void AddConnection(IConnection connection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        if (_connections.Contains(connection)) throw new ArgumentException("Connection already exists in graph", nameof(connection));
        
        // Verify that both nodes are in this graph
        if (!_nodes.Contains(connection.Source.Parent))
            throw new ArgumentException("Source node is not in this graph", nameof(connection));
        if (!_nodes.Contains(connection.Target.Parent))
            throw new ArgumentException("Target node is not in this graph", nameof(connection));

        _connections.Add(connection);
    }

    public void RemoveConnection(IConnection connection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        
        connection.Remove();  // This will remove it from the connectors
        _ = _connections.Remove(connection);  // Remove from our list
    }

    public void Clear()
    {
        // Remove all connections first
        foreach (var connection in _connections.ToList())
        {
            RemoveConnection(connection);
        }

        _nodes.Clear();
    }
} 