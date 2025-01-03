namespace Flow.Core.Models;

public class Item
{
    public string Identifier { get; }
    public string DisplayName { get; }

    public Item(string identifier, string displayName)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Item identifier cannot be null or empty.", nameof(identifier));
            
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Item display name cannot be null or empty.", nameof(displayName));

        Identifier = identifier;
        DisplayName = displayName;
    }
} 