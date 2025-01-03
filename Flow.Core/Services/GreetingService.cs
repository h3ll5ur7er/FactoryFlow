using System.Threading.Tasks;

namespace Flow.Core.Services;

public class GreetingService : IGreetingService
{
    public Task<string> GetGreetingAsync(string name)
    {
        return Task.FromResult($"Hello, {name}! Welcome to Flow.");
    }
} 