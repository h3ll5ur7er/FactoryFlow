using System;
using System.Collections.ObjectModel;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flow.Core.Services;

namespace Flow.ViewModels.Graph;

public enum NodeType
{
    Generic,
    Recipe,
    Splerger,
    SubGraph
}

public partial class NodeViewModel : ViewModelBase
{
    private readonly IGraphManager _graphManager;

    public NodeViewModel(IGraphManager graphManager)
    {
        _graphManager = graphManager;
    }

    [RelayCommand]
    private void Delete()
    {
        _graphManager.RemoveNode(this);
    }

    [ObservableProperty]
    private Point _position;

    [ObservableProperty]
    private Size _size = new(100, 100);

    private string _title = "New Node";
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw value == null 
                    ? new ArgumentNullException(nameof(value))
                    : new ArgumentException("Title cannot be empty", nameof(value));
            }
            SetProperty(ref _title, value);
        }
    }

    [ObservableProperty]
    private NodeType _nodeType;

    [ObservableProperty]
    private bool _isSelected;

    private double _multiplier = 1.0;
    public double Multiplier
    {
        get => _multiplier;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Multiplier must be greater than zero", nameof(value));
            }
            SetProperty(ref _multiplier, value);
        }
    }

    public ObservableCollection<ConnectorViewModel> InputConnectors { get; } = new();
    public ObservableCollection<ConnectorViewModel> OutputConnectors { get; } = new();

    public void AddInputConnector(ConnectorViewModel connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));
        
        if (connector.Type != ConnectorType.Input)
            throw new ArgumentException("Connector must be an input type", nameof(connector));

        InputConnectors.Add(connector);
    }

    public void AddOutputConnector(ConnectorViewModel connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));
        
        if (connector.Type != ConnectorType.Output)
            throw new ArgumentException("Connector must be an output type", nameof(connector));

        OutputConnectors.Add(connector);
    }

    public bool RemoveInputConnector(ConnectorViewModel connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));

        return InputConnectors.Remove(connector);
    }

    public bool RemoveOutputConnector(ConnectorViewModel connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));

        return OutputConnectors.Remove(connector);
    }

    public void SetPosition(double x, double y)
    {
        Position = new Point(x, y);
        OnPropertyChanged(nameof(Position));
    }
} 