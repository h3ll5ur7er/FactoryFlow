using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class GraphManager : IGraphManager
{
    private readonly INodeFactory _nodeFactory;
    private GraphCanvasViewModel? _currentGraph;

    public GraphCanvasViewModel? CurrentGraph => _currentGraph;

    public GraphManager(INodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;
        CreateNewGraph();
    }

    public void CreateNewGraph()
    {
        _currentGraph = new GraphCanvasViewModel(_nodeFactory, this);
    }

    public void LoadGraph(string path)
    {
        // TODO: Implement graph loading
        _currentGraph = new GraphCanvasViewModel(_nodeFactory, this);
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
        _currentGraph?.Clear();
    }
} 