using Avalonia.Data.Converters;
using Flow.Core.Models;

namespace Flow.ViewModels.Graph;

public class BasicRecipeFilter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is Recipe recipe)
        {
            // Basic recipes are those with only one input and one output
            return recipe.Inputs.Count == 1 && recipe.Outputs.Count == 1;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class AdvancedRecipeFilter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is Recipe recipe)
        {
            // Advanced recipes are those with multiple inputs or outputs
            return recipe.Inputs.Count > 1 || recipe.Outputs.Count > 1;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 