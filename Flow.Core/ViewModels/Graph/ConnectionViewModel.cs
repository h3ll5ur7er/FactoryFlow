using CommunityToolkit.Mvvm.ComponentModel;

namespace Flow.ViewModels.Graph;

public partial class ConnectionViewModel : ObservableObject
{
    [ObservableProperty]
    private ConnectorViewModel _source;

    [ObservableProperty]
    private ConnectorViewModel _target;

    [ObservableProperty]
    private bool _isEnabled = true;

    [ObservableProperty]
    private decimal _flowRate = 1.0m;

    public ConnectionViewModel(ConnectorViewModel source, ConnectorViewModel target)
    {
        _source = source;
        _target = target;
    }

    public bool Validate()
    {
        // Basic validation rules
        if (Source.Type == Target.Type) return false;
        if (!Source.CanConnectTo(Target)) return false;
        if (FlowRate <= 0) return false;

        return true;
    }
} 