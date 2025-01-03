using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class GraphManager : IGraphManager
{
    private GraphCanvasViewModel _currentGraph;

    public GraphCanvasViewModel CurrentGraph => _currentGraph;

    public GraphManager()
    {
        _currentGraph = new GraphCanvasViewModel();
    }

    public void CreateNewGraph()
    {
        _currentGraph = new GraphCanvasViewModel();
    }

    public void LoadGraph(string path)
    {
        // TODO: Implement graph loading
        throw new NotImplementedException();
    }

    public void SaveGraph(string path)
    {
        // TODO: Implement graph saving
        throw new NotImplementedException();
    }

    public void AddNode(NodeViewModel node)
    {
        _currentGraph.AddNode(node);
    }

    public void RemoveNode(NodeViewModel node)
    {
        _currentGraph.RemoveNode(node);
    }

    public void ClearGraph()
    {
        _currentGraph.Nodes.Clear();
    }
} 