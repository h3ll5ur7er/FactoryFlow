using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Flow.Core.Models;

namespace Flow.ViewModels.Graph;

public enum ConnectorType
{
    Input,
    Output
}

public partial class ConnectorViewModel : ViewModelBase
{
    [ObservableProperty]
    private ConnectorType _type;

    [ObservableProperty]
    private bool _allowMultipleConnections;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private Item? _itemType;

    private readonly ObservableCollection<ConnectorViewModel> _connections = new();
    public IReadOnlyCollection<ConnectorViewModel> Connections => _connections;

    public bool IsConnected => _connections.Count > 0;

    public HashSet<Type> AcceptedTypes { get; } = new();

    public ConnectorViewModel(ConnectorType type)
    {
        _type = type;
    }

    public bool CanConnectTo(ConnectorViewModel other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        // Can't connect to self
        if (other == this)
            return false;

        // Must connect input to output
        if (Type == other.Type)
            return false;

        // Check if either connector already has connections and doesn't allow multiple
        if (IsConnected && !AllowMultipleConnections)
            return false;
        if (other.IsConnected && !other.AllowMultipleConnections)
            return false;

        // If no accepted types are specified, allow all connections
        if (AcceptedTypes.Count == 0 && other.AcceptedTypes.Count == 0)
            return true;

        // Check type compatibility
        return AcceptedTypes.Overlaps(other.AcceptedTypes);
    }

    public bool TryConnect(ConnectorViewModel other)
    {
        if (!CanConnectTo(other))
            return false;

        _connections.Add(other);
        other._connections.Add(this);
        OnPropertyChanged(nameof(IsConnected));
        return true;
    }

    public bool TryDisconnect(ConnectorViewModel other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var removed = _connections.Remove(other);
        if (removed)
        {
            other._connections.Remove(this);
            OnPropertyChanged(nameof(IsConnected));
        }
        return removed;
    }

    public void DisconnectAll()
    {
        foreach (var connection in _connections.ToArray())
        {
            connection._connections.Remove(this);
            connection.OnPropertyChanged(nameof(IsConnected));
        }
        _connections.Clear();
        OnPropertyChanged(nameof(IsConnected));
    }
} 