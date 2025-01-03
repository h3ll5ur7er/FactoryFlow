using System;
using System.Collections.Generic;

namespace Flow.Core.Models.Graph;

/// <summary>
/// Base class for nodes in the production flow graph.
/// </summary>
public class Node : INode
{
    private readonly List<IConnector> _inputs = new();
    private readonly List<IConnector> _outputs = new();

    public string Identifier { get; }
    public string DisplayName { get; }
    public IReadOnlyCollection<IConnector> Inputs => _inputs.AsReadOnly();
    public IReadOnlyCollection<IConnector> Outputs => _outputs.AsReadOnly();
    public double X { get; set; }
    public double Y { get; set; }

    /// <summary>
    /// Creates a new node.
    /// </summary>
    /// <param name="identifier">The unique identifier of the node.</param>
    /// <param name="displayName">The display name of the node.</param>
    /// <exception cref="ArgumentException">Thrown when identifier or displayName is invalid.</exception>
    public Node(string identifier, string displayName)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier cannot be empty", nameof(identifier));
            
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty", nameof(displayName));

        Identifier = identifier;
        DisplayName = displayName;
    }

    /// <summary>
    /// Adds an input connector to the node.
    /// </summary>
    /// <param name="connector">The connector to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when connector is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the connector is not an input or belongs to another node.</exception>
    public void AddInput(IConnector connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));
            
        if (!connector.IsInput)
            throw new ArgumentException("Connector must be an input connector", nameof(connector));

        if (connector.Parent != this)
            throw new ArgumentException("Connector belongs to another node", nameof(connector));

        _inputs.Add(connector);
    }

    /// <summary>
    /// Adds an output connector to the node.
    /// </summary>
    /// <param name="connector">The connector to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when connector is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the connector is an input or belongs to another node.</exception>
    public void AddOutput(IConnector connector)
    {
        if (connector == null)
            throw new ArgumentNullException(nameof(connector));
            
        if (connector.IsInput)
            throw new ArgumentException("Connector must be an output connector", nameof(connector));

        if (connector.Parent != this)
            throw new ArgumentException("Connector belongs to a different node", nameof(connector));

        _outputs.Add(connector);
    }

    /// <summary>
    /// Creates a new input connector for this node.
    /// </summary>
    /// <param name="identifier">The identifier for the connector.</param>
    /// <param name="displayName">The display name for the connector.</param>
    /// <param name="allowsMultipleConnections">Whether the connector allows multiple connections.</param>
    /// <param name="acceptedItems">The items that this connector accepts. If null, accepts any item.</param>
    /// <returns>The created connector.</returns>
    protected IConnector CreateInput(
        string identifier,
        string displayName,
        bool allowsMultipleConnections = false,
        IEnumerable<Item>? acceptedItems = null)
    {
        var connector = new Connector(identifier, displayName, this, true, allowsMultipleConnections, acceptedItems);
        AddInput(connector);
        return connector;
    }

    /// <summary>
    /// Creates a new output connector for this node.
    /// </summary>
    /// <param name="identifier">The identifier for the connector.</param>
    /// <param name="displayName">The display name for the connector.</param>
    /// <param name="allowsMultipleConnections">Whether the connector allows multiple connections.</param>
    /// <param name="acceptedItems">The items that this connector accepts. If null, accepts any item.</param>
    /// <returns>The created connector.</returns>
    protected IConnector CreateOutput(
        string identifier,
        string displayName,
        bool allowsMultipleConnections = false,
        IEnumerable<Item>? acceptedItems = null)
    {
        var connector = new Connector(identifier, displayName, this, false, allowsMultipleConnections, acceptedItems);
        AddOutput(connector);
        return connector;
    }

    public virtual bool Validate()
    {
        return Inputs.All(i => i.ValidateConnections()) && 
               Outputs.All(o => o.ValidateConnections());
    }
} 