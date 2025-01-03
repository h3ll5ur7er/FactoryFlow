using Flow.Core.Models;
using Xunit;

namespace Flow.Tests.Models;

public class ItemStackTests
{
    private readonly Item _testItem = new("test-item", "Test Item");
    private readonly Item _otherItem = new("other-item", "Other Item");

    [Fact]
    public void Constructor_WithValidParameters_CreatesItemStack()
    {
        // Arrange
        var amount = 42.5m;

        // Act
        var stack = new ItemStack(_testItem, amount);

        // Assert
        Assert.Equal(_testItem, stack.Item);
        Assert.Equal(amount, stack.Amount);
    }

    [Fact]
    public void Constructor_WithNullItem_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new ItemStack(null!, 1.0m));
        Assert.Equal("item", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-42.5)]
    public void Constructor_WithZeroOrNegativeAmount_ThrowsArgumentException(decimal amount)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => new ItemStack(_testItem, amount));
        Assert.Contains("amount", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void WithAmount_CreatesNewStackWithDifferentAmount()
    {
        // Arrange
        var originalStack = new ItemStack(_testItem, 1.0m);
        var newAmount = 2.5m;

        // Act
        var newStack = originalStack.WithAmount(newAmount);

        // Assert
        Assert.Equal(_testItem, newStack.Item);
        Assert.Equal(newAmount, newStack.Amount);
        Assert.NotSame(originalStack, newStack);
    }

    [Fact]
    public void AddOperator_WithSameItem_CombinesAmounts()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_testItem, 5.5m);

        // Act
        var result = stack1 + stack2;

        // Assert
        Assert.Equal(_testItem, result.Item);
        Assert.Equal(16.0m, result.Amount);
    }

    [Fact]
    public void AddOperator_WithDifferentItems_ThrowsArgumentException()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_otherItem, 5.5m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack1 + stack2);
        Assert.Contains("same item", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(2.0)]
    [InlineData(0.5)]
    public void MultiplyOperator_WithNumber_MultipliesAmount(decimal multiplier)
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);
        var expected = 10.0m * multiplier;

        // Act
        var result = stack * multiplier;

        // Assert
        Assert.Equal(_testItem, result.Item);
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void MultiplyOperator_WithZero_ThrowsArgumentException()
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack * 0);
        Assert.Contains("multiplier", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.5)]
    public void MultiplyOperator_WithNegativeNumber_ThrowsArgumentException(decimal multiplier)
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack * multiplier);
        Assert.Contains("multiplier", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SubtractOperator_WithSameItem_SubtractsAmounts()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_testItem, 3.5m);

        // Act
        var result = stack1 - stack2;

        // Assert
        Assert.Equal(_testItem, result.Item);
        Assert.Equal(7.0m, result.Amount);
    }

    [Fact]
    public void SubtractOperator_WithDifferentItems_ThrowsArgumentException()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_otherItem, 3.5m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack1 - stack2);
        Assert.Contains("same item", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SubtractOperator_ResultingInNegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 3.5m);
        var stack2 = new ItemStack(_testItem, 10.5m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack1 - stack2);
        Assert.Contains("result in a positive amount", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(2.0)]
    [InlineData(0.5)]
    public void DivideOperator_WithNumber_DividesAmount(decimal divisor)
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);
        var expected = 10.0m / divisor;

        // Act
        var result = stack / divisor;

        // Assert
        Assert.Equal(_testItem, result.Item);
        Assert.Equal(expected, result.Amount);
    }

    [Fact]
    public void DivideOperator_WithZero_ThrowsArgumentException()
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack / 0);
        Assert.Contains("divisor", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.5)]
    public void DivideOperator_WithNegativeNumber_ThrowsArgumentException(decimal divisor)
    {
        // Arrange
        var stack = new ItemStack(_testItem, 10.0m);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => stack / divisor);
        Assert.Contains("divisor", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComparisonOperators_WithSameItem_ComparesAmounts()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_testItem, 5.5m);
        var stack3 = new ItemStack(_testItem, 10.5m);

        // Assert
        Assert.True(stack1 > stack2);
        Assert.True(stack2 < stack1);
        Assert.True(stack1 >= stack3);
        Assert.True(stack1 <= stack3);
        Assert.False(stack1 > stack3);
        Assert.False(stack1 < stack3);
    }

    [Fact]
    public void ComparisonOperators_WithDifferentItems_ThrowsArgumentException()
    {
        // Arrange
        var stack1 = new ItemStack(_testItem, 10.5m);
        var stack2 = new ItemStack(_otherItem, 5.5m);

        // Assert
        Assert.Throws<ArgumentException>(() => stack1 > stack2);
        Assert.Throws<ArgumentException>(() => stack1 < stack2);
        Assert.Throws<ArgumentException>(() => stack1 >= stack2);
        Assert.Throws<ArgumentException>(() => stack1 <= stack2);
    }
} 