using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public interface IGraphManager
{
    GraphCanvasViewModel CurrentGraph { get; }
    void AddNode(NodeViewModel node);
    void RemoveNode(NodeViewModel node);
    void CreateConnection(ConnectorViewModel source, ConnectorViewModel target);
    void ClearGraph();
} 