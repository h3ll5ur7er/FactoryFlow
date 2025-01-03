using System;

namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents a connection between two connectors in the graph.
/// </summary>
public class Connection : IConnection
{
    public IConnector Source { get; }
    public IConnector Target { get; }
    public decimal FlowRate { get; }
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Creates a new connection between two connectors.
    /// </summary>
    /// <param name="source">The source connector (must be an output).</param>
    /// <param name="target">The target connector (must be an input).</param>
    /// <param name="flowRate">The rate at which items flow through this connection (must be positive).</param>
    /// <exception cref="ArgumentNullException">Thrown when source or target is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the connection is invalid.</exception>
    public Connection(IConnector source, IConnector target, decimal flowRate = 1m)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Target = target ?? throw new ArgumentNullException(nameof(target));
        
        if (flowRate <= 0)
            throw new ArgumentException("Flow rate must be positive.", nameof(flowRate));
            
        if (source.IsInput)
            throw new ArgumentException("Source connector must be an output.", nameof(source));
            
        if (!target.IsInput)
            throw new ArgumentException("Target connector must be an input.", nameof(target));
            
        if (!source.CanConnectTo(target))
            throw new ArgumentException("The connectors cannot be connected.");

        FlowRate = flowRate;
        IsEnabled = true;

        // Add this connection to both connectors
        try
        {
            source.AddConnection(this);
            target.AddConnection(this);
        }
        catch (Exception)
        {
            // If adding to either connector fails, ensure we clean up
            // This prevents leaving the graph in an inconsistent state
            source.RemoveConnection(this);
            target.RemoveConnection(this);
            throw;
        }
    }

    public bool Validate()
    {
        if (FlowRate <= 0)
            return false;

        if (Source.IsInput || !Target.IsInput)
            return false;

        if (!Source.CanConnectTo(Target))
            return false;

        return true;
    }

    /// <summary>
    /// Removes this connection from both its source and target connectors.
    /// </summary>
    public void Remove()
    {
        Source.RemoveConnection(this);
        Target.RemoveConnection(this);
        IsEnabled = false;
    }
} 