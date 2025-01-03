using Flow.Core.Models.Graph;

namespace Flow.Tests.Models.Graph;

/// <summary>
/// A simple implementation of INode for testing purposes.
/// </summary>
public class TestNode : INode
{
    private readonly List<IConnector> _inputs = new();
    private readonly List<IConnector> _outputs = new();

    public string Identifier { get; }
    public string DisplayName { get; }
    public IReadOnlyCollection<IConnector> Inputs => _inputs.AsReadOnly();
    public IReadOnlyCollection<IConnector> Outputs => _outputs.AsReadOnly();
    public double X { get; set; }
    public double Y { get; set; }

    public TestNode(string identifier, string displayName)
    {
        Identifier = identifier;
        DisplayName = displayName;
    }

    public void AddInput(IConnector connector)
    {
        _inputs.Add(connector);
    }

    public void AddOutput(IConnector connector)
    {
        _outputs.Add(connector);
    }

    public bool Validate()
    {
        return Inputs.All(i => i.ValidateConnections()) && 
               Outputs.All(o => o.ValidateConnections());
    }
} 