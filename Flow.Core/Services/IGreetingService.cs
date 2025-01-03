using System.Threading.Tasks;

namespace Flow.Core.Services;

public interface IGreetingService
{
    Task<string> GetGreetingAsync(string name);
} 