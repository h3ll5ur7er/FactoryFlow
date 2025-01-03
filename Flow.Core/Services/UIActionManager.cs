using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public class UIActionManager : IUIActionManager
{
    private readonly Dictionary<string, (Func<Task> Action, Func<Task<bool>>? CanExecute)> _actions = new();
    private readonly IGraphManager _graphManager;
    private readonly INodeFactory _nodeFactory;

    public UIActionManager(IGraphManager graphManager, INodeFactory nodeFactory)
    {
        _graphManager = graphManager;
        _nodeFactory = nodeFactory;
    }

    public Task<bool> CanExecuteActionAsync(string actionName)
    {
        if (_actions.TryGetValue(actionName, out var action))
        {
            return action.CanExecute?.Invoke() ?? Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public async Task ExecuteActionAsync(string actionName)
    {
        if (!_actions.TryGetValue(actionName, out var action))
        {
            throw new KeyNotFoundException($"Action '{actionName}' not found.");
        }

        if (action.CanExecute == null || await action.CanExecute())
        {
            await action.Action();
        }
    }

    public void RegisterAction(string actionName, Func<Task> action, Func<Task<bool>>? canExecute = null)
    {
        _actions[actionName] = (action, canExecute);
    }

    public void UnregisterAction(string actionName)
    {
        if (!_actions.Remove(actionName))
        {
            throw new KeyNotFoundException($"Action '{actionName}' not found.");
        }
    }

    public Task AddNodeAsync(Point position)
    {
        var node = _nodeFactory.CreateNode(NodeType.Generic, _graphManager);
        node.Position = position;
        _graphManager.AddNode(node);
        return Task.CompletedTask;
    }

    public Task DeleteSelectedNodesAsync()
    {
        var selectedNode = _graphManager.CurrentGraph?.SelectedNode;
        if (selectedNode != null)
        {
            _graphManager.RemoveNode(selectedNode);
        }
        return Task.CompletedTask;
    }

    public Task CopySelectedNodesAsync()
    {
        // TODO: Implement node copying
        return Task.CompletedTask;
    }

    public Task PasteNodesAsync(Point position)
    {
        // TODO: Implement node pasting
        return Task.CompletedTask;
    }

    public Task UndoAsync()
    {
        // TODO: Implement undo
        return Task.CompletedTask;
    }

    public Task RedoAsync()
    {
        // TODO: Implement redo
        return Task.CompletedTask;
    }
} 