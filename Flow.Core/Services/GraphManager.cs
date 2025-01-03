using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class GraphManager : IGraphManager
{
    private readonly INodeFactory _nodeFactory;
    private readonly IGameRegistry _gameRegistry;
    private GraphCanvasViewModel? _currentGraph;

    public GraphCanvasViewModel? CurrentGraph => _currentGraph;

    public GraphManager(INodeFactory nodeFactory, IGameRegistry gameRegistry)
    {
        _nodeFactory = nodeFactory;
        _gameRegistry = gameRegistry;
        CreateNewGraph();
    }

    public void CreateNewGraph()
    {
        _currentGraph = new GraphCanvasViewModel(_nodeFactory, this, _gameRegistry);
    }

    public void LoadGraph(string path)
    {
        // TODO: Implement graph loading
        _currentGraph = new GraphCanvasViewModel(_nodeFactory, this, _gameRegistry);
    }

    public void SaveGraph(string path)
    {
        // TODO: Implement graph saving
    }

    public void AddNode(NodeViewModel node)
    {
        _currentGraph?.AddNode(node);
    }

    public void RemoveNode(NodeViewModel node)
    {
        _currentGraph?.RemoveNode(node);
    }

    public void ClearGraph()
    {
        _currentGraph?.Nodes.Clear();
    }
} 