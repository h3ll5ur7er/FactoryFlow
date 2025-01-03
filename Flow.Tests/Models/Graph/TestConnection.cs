using Flow.Core.Models.Graph;
using Xunit.Abstractions;

namespace Flow.Tests.Models.Graph;

/// <summary>
/// A simple implementation of IConnection for testing purposes.
/// </summary>
public class TestConnection : IConnection
{
    private readonly ITestOutputHelper? _output;

    public IConnector Source { get; }
    public IConnector Target { get; }
    public decimal FlowRate { get; }
    public bool IsEnabled { get; private set; }

    public TestConnection(IConnector source, IConnector target, decimal flowRate = 1m, ITestOutputHelper? output = null)
    {
        Source = source;
        Target = target;
        FlowRate = flowRate;
        IsEnabled = true;
        _output = output;

        // Add this connection to both connectors
        source.AddConnection(this);
        target.AddConnection(this);
    }

    public bool Validate()
    {
        // Basic validation rules:
        // 1. Flow rate must be positive
        if (FlowRate <= 0)
        {
            _output?.WriteLine($"[{Source.Identifier}->{Target.Identifier}] Invalid flow rate: {FlowRate}");
            return false;
        }

        // 2. Source must be an output connector
        if (Source.IsInput)
        {
            _output?.WriteLine($"[{Source.Identifier}->{Target.Identifier}] Source is not an output connector");
            return false;
        }

        // 3. Target must be an input connector
        if (!Target.IsInput)
        {
            _output?.WriteLine($"[{Source.Identifier}->{Target.Identifier}] Target is not an input connector");
            return false;
        }

        // 4. Both connectors must allow this connection
        if (!Source.CanConnectTo(Target))
        {
            _output?.WriteLine($"[{Source.Identifier}->{Target.Identifier}] Connectors cannot be connected");
            return false;
        }

        return true;
    }

    public void Remove()
    {
        Source.RemoveConnection(this);
        Target.RemoveConnection(this);
    }
} 