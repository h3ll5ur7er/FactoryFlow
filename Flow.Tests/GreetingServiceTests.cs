using Flow.Core.Services;
using System.Threading.Tasks;
using Xunit;

namespace Flow.Tests;

public class GreetingServiceTests
{
    [Fact]
    public async Task GetGreetingAsync_ReturnsExpectedGreeting()
    {
        // Arrange
        var service = new GreetingService();
        var name = "Test User";

        // Act
        var result = await service.GetGreetingAsync(name);

        // Assert
        Assert.Equal($"Hello, {name}! Welcome to Flow.", result);
    }
} 