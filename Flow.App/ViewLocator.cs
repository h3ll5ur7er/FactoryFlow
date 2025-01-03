using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Flow.ViewModels;

namespace Flow.App;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? param)
    {
        if (param is null)
        {
            Debug.WriteLine("ViewLocator: param is null");
            return new TextBlock { Text = "No Data" };
        }

        var viewModelName = param.GetType().FullName!;
        var viewModelAssembly = param.GetType().Assembly;
        var viewName = viewModelName.Replace("ViewModel", "View");
        
        Debug.WriteLine($"ViewLocator: Looking for view {viewName}");
        Debug.WriteLine($"ViewLocator: ViewModel assembly: {viewModelAssembly.FullName}");

        // Try to find the view in the same assembly as the app
        var appAssembly = Assembly.GetExecutingAssembly();
        var viewType = appAssembly.GetTypes()
            .FirstOrDefault(t => t.FullName == viewName || t.FullName?.EndsWith(viewName) == true);

        if (viewType != null)
        {
            Debug.WriteLine($"ViewLocator: Found view type {viewType.FullName} in app assembly");
            return (Control)Activator.CreateInstance(viewType)!;
        }

        Debug.WriteLine($"ViewLocator: View not found for {viewName}");
        return new TextBlock { Text = "Not Found: " + viewName };
    }

    public bool Match(object? data)
    {
        var isMatch = data is ViewModelBase;
        Debug.WriteLine($"ViewLocator: Match called for {data?.GetType().FullName}, result: {isMatch}");
        return isMatch;
    }
} 