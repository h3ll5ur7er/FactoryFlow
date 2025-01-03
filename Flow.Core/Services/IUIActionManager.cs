using System;
using System.Threading.Tasks;
using Avalonia;
using Flow.ViewModels.Graph;

namespace Flow.Core.Services;

public interface IUIActionManager
{
    Task<bool> CanExecuteActionAsync(string actionName);
    Task ExecuteActionAsync(string actionName);
    void RegisterAction(string actionName, Func<Task> action, Func<Task<bool>>? canExecute = null);
    void UnregisterAction(string actionName);
    
    // Common UI actions
    Task AddNodeAsync(Point position);
    Task DeleteSelectedNodesAsync();
    Task CopySelectedNodesAsync();
    Task PasteNodesAsync(Point position);
    Task UndoAsync();
    Task RedoAsync();
} 