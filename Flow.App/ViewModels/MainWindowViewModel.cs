using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flow.Core.Services;
using System.Threading.Tasks;

namespace Flow.App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IGreetingService _greetingService;

    [ObservableProperty]
    private string _greeting = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    public MainWindowViewModel(IGreetingService greetingService)
    {
        _greetingService = greetingService;
    }

    [RelayCommand]
    private async Task UpdateGreeting()
    {
        Greeting = await _greetingService.GetGreetingAsync(Name);
    }
} 