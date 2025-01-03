using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Flow.App.Views.Graph;

public partial class RecipeNodeView : UserControl
{
    public RecipeNodeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
} 