using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace Flow.App.Helpers;

public static class ControlExtensions
{
    public static T? FindAncestorOfType<T>(this Control control) where T : class
    {
        var parent = control.Parent;
        while (parent != null)
        {
            if (parent is T typed)
                return typed;
            parent = parent.Parent;
        }
        return null;
    }

    public static T? FindDescendantOfType<T>(this Control control) where T : class
    {
        foreach (var child in control.GetVisualDescendants())
        {
            if (child is T typed)
                return typed;
        }
        return null;
    }
} 