namespace Flow.Core.Models;

public class ItemStack : IComparable<ItemStack>
{
    public Item Item { get; }
    public decimal Amount { get; }

    public ItemStack(Item item, decimal amount)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        Amount = amount;
    }

    public ItemStack WithAmount(decimal newAmount)
    {
        return new ItemStack(Item, newAmount);
    }

    public static ItemStack operator +(ItemStack left, ItemStack right)
    {
        if (left.Item != right.Item)
            throw new ArgumentException("Can only add ItemStacks with the same item type.");

        return new ItemStack(left.Item, left.Amount + right.Amount);
    }

    public static ItemStack operator -(ItemStack left, ItemStack right)
    {
        if (left.Item != right.Item)
            throw new ArgumentException("Can only subtract ItemStacks with the same item type.");

        var newAmount = left.Amount - right.Amount;
        if (newAmount <= 0)
            throw new ArgumentException("Subtraction must result in a positive amount.");

        return new ItemStack(left.Item, newAmount);
    }

    public static ItemStack operator *(ItemStack stack, decimal multiplier)
    {
        if (multiplier <= 0)
            throw new ArgumentException("Multiplier must be greater than zero.", nameof(multiplier));

        return new ItemStack(stack.Item, stack.Amount * multiplier);
    }

    public static ItemStack operator *(decimal multiplier, ItemStack stack)
    {
        return stack * multiplier;
    }

    public static ItemStack operator /(ItemStack stack, decimal divisor)
    {
        if (divisor <= 0)
            throw new ArgumentException("Divisor must be greater than zero.", nameof(divisor));

        return new ItemStack(stack.Item, stack.Amount / divisor);
    }

    public static bool operator >(ItemStack left, ItemStack right)
    {
        EnsureSameItemType(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(ItemStack left, ItemStack right)
    {
        EnsureSameItemType(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(ItemStack left, ItemStack right)
    {
        EnsureSameItemType(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(ItemStack left, ItemStack right)
    {
        EnsureSameItemType(left, right);
        return left.Amount <= right.Amount;
    }

    public int CompareTo(ItemStack? other)
    {
        if (other is null) return 1;
        EnsureSameItemType(this, other);
        return Amount.CompareTo(other.Amount);
    }

    private static void EnsureSameItemType(ItemStack left, ItemStack right)
    {
        if (left.Item != right.Item)
            throw new ArgumentException("Can only compare ItemStacks with the same item type.");
    }
} 