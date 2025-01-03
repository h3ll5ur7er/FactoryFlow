using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Flow.Core.Models;

namespace Flow.ViewModels.Graph;

public enum ConnectorType
{
    Input,
    Output
}

public partial class ConnectorViewModel : ObservableObject
{
    private readonly NodeViewModel _node;
    private readonly ObservableCollection<ConnectionViewModel> _connections = new();
    private readonly ObservableCollection<Item> _acceptedItems = new();

    [ObservableProperty]
    private string _identifier;

    [ObservableProperty]
    private string _displayName;

    [ObservableProperty]
    private ConnectorType _type;

    [ObservableProperty]
    private bool _allowsMultipleConnections;

    public NodeViewModel Node => _node;
    public IReadOnlyCollection<ConnectionViewModel> Connections => _connections;
    public IReadOnlyCollection<Item> AcceptedItems => _acceptedItems;
    public bool IsConnected => _connections.Count > 0;

    public ConnectorViewModel(string identifier, string displayName, NodeViewModel node, ConnectorType type, bool allowsMultipleConnections, IEnumerable<Item>? acceptedItems = null)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier cannot be empty", nameof(identifier));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty", nameof(displayName));

        if (node == null)
            throw new ArgumentNullException(nameof(node));

        _identifier = identifier;
        _displayName = displayName;
        _node = node;
        _type = type;
        _allowsMultipleConnections = allowsMultipleConnections;

        if (acceptedItems != null)
        {
            foreach (var item in acceptedItems)
            {
                _acceptedItems.Add(item);
            }
        }
    }

    public bool CanConnectTo(ConnectorViewModel other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // Cannot connect to self
        if (this == other) return false;

        // Cannot connect to same node
        if (Node == other.Node) return false;

        // Cannot connect same types
        if (Type == other.Type) return false;

        // Check if multiple connections are allowed
        if (!AllowsMultipleConnections && IsConnected) return false;
        if (!other.AllowsMultipleConnections && other.IsConnected) return false;

        // If either connector accepts no items, they can connect
        if (!AcceptedItems.Any() || !other.AcceptedItems.Any())
            return true;

        // Check if there are any compatible item types
        return AcceptedItems.Intersect(other.AcceptedItems).Any();
    }

    public void AddConnection(ConnectionViewModel connection)
    {
        if (!CanAcceptConnection(connection)) return;
        _connections.Add(connection);
        OnPropertyChanged(nameof(IsConnected));
    }

    public void RemoveConnection(ConnectionViewModel connection)
    {
        if (_connections.Remove(connection))
        {
            OnPropertyChanged(nameof(IsConnected));
        }
    }

    private bool CanAcceptConnection(ConnectionViewModel connection)
    {
        if (connection == null)
            throw new ArgumentNullException(nameof(connection));

        // Verify this connector is part of the connection
        if (connection.Source != this && connection.Target != this) return false;

        // Check if multiple connections are allowed
        if (!AllowsMultipleConnections && IsConnected) return false;

        // Get the other connector
        var otherConnector = connection.Source == this ? connection.Target : connection.Source;

        // Check item type compatibility
        if (AcceptedItems.Any() && otherConnector.AcceptedItems.Any())
        {
            if (!AcceptedItems.Intersect(otherConnector.AcceptedItems).Any())
                return false;
        }

        return true;
    }
} 