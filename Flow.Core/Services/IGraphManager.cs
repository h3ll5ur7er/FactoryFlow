using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public interface IGraphManager
{
    GraphCanvasViewModel CurrentGraph { get; }
    void CreateNewGraph();
    void LoadGraph(string path);
    void SaveGraph(string path);
    void AddNode(NodeViewModel node);
    void RemoveNode(NodeViewModel node);
    void ClearGraph();
} 