using Flow.Core.Models.Graph;
using Flow.Core.Models.Graph.Nodes;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class GraphManager : IGraphManager
{
    private readonly Graph _graph = new();
    private readonly INodeFactory _nodeFactory;
    private readonly Dictionary<INode, NodeViewModel> _nodeModelToViewModel = new();
    private readonly Dictionary<IConnector, ConnectorViewModel> _connectorModelToViewModel = new();
    private readonly Dictionary<IConnection, ConnectionViewModel> _connectionModelToViewModel = new();

    public GraphCanvasViewModel CurrentGraph { get; }

    public GraphManager(INodeFactory nodeFactory, IGameRegistry gameRegistry)
    {
        _nodeFactory = nodeFactory;
        CurrentGraph = new GraphCanvasViewModel(nodeFactory, this, gameRegistry);
    }

    public void AddNode(NodeViewModel nodeVM)
    {
        // Create the model node
        var node = CreateModelNode(nodeVM);
        
        // Add to graph
        _graph.AddNode(node);
        
        // Store the mapping
        _nodeModelToViewModel[node] = nodeVM;
        
        // Add to view
        CurrentGraph.AddNode(nodeVM);
    }

    public void RemoveNode(NodeViewModel nodeVM)
    {
        // Find the corresponding model node
        var node = _nodeModelToViewModel.FirstOrDefault(x => x.Value == nodeVM).Key;
        if (node == null) return;

        // Remove from graph (this will also remove connections)
        _graph.RemoveNode(node);
        
        // Remove from mappings
        _nodeModelToViewModel.Remove(node);
        
        // Remove from view
        CurrentGraph.RemoveNode(nodeVM);
    }

    public void CreateConnection(ConnectorViewModel sourceVM, ConnectorViewModel targetVM)
    {
        // Find model connectors
        var sourceConnector = _connectorModelToViewModel.FirstOrDefault(x => x.Value == sourceVM).Key;
        var targetConnector = _connectorModelToViewModel.FirstOrDefault(x => x.Value == targetVM).Key;
        
        if (sourceConnector == null || targetConnector == null) return;

        // Create model connection
        var connection = new Connection(sourceConnector, targetConnector);
        _graph.AddConnection(connection);

        // Create view model connection
        var connectionVM = new ConnectionViewModel(sourceVM, targetVM);
        _connectionModelToViewModel[connection] = connectionVM;
        
        // Add to view
        CurrentGraph.Connections.Add(connectionVM);
    }

    public void ClearGraph()
    {
        _graph.Clear();
        _nodeModelToViewModel.Clear();
        _connectorModelToViewModel.Clear();
        _connectionModelToViewModel.Clear();
        CurrentGraph.Nodes.Clear();
        CurrentGraph.Connections.Clear();
    }

    private INode CreateModelNode(NodeViewModel nodeVM)
    {
        Console.WriteLine($"[GraphManager] Creating model node for: {nodeVM.Title}");
        
        INode node = nodeVM switch
        {
            RecipeNodeViewModel recipeVM => new RecipeNode(recipeVM.Recipe),
            _ => new Node(nodeVM.Title, nodeVM.Title)  // Generic node for now
        };

        // Create view model connectors for each model connector
        foreach (var connector in node.Inputs)
        {
            Console.WriteLine($"[GraphManager] Creating view model for input connector: {connector.DisplayName}");
            var connectorVM = new ConnectorViewModel(
                connector.Identifier,
                connector.DisplayName,
                nodeVM,
                ConnectorType.Input,
                connector.AllowsMultipleConnections,
                connector.AcceptedItems
            );
            nodeVM.AddInputConnector(connectorVM);
            _connectorModelToViewModel[connector] = connectorVM;
        }

        foreach (var connector in node.Outputs)
        {
            Console.WriteLine($"[GraphManager] Creating view model for output connector: {connector.DisplayName}");
            var connectorVM = new ConnectorViewModel(
                connector.Identifier,
                connector.DisplayName,
                nodeVM,
                ConnectorType.Output,
                connector.AllowsMultipleConnections,
                connector.AcceptedItems
            );
            nodeVM.AddOutputConnector(connectorVM);
            _connectorModelToViewModel[connector] = connectorVM;
        }

        return node;
    }
} 