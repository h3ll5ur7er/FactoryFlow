namespace Flow.Core.Models;

public class Machine
{
    public string Identifier { get; }
    public string DisplayName { get; }
    public decimal PowerConsumption { get; }

    public Machine(string identifier, string displayName, decimal powerConsumption)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Machine identifier cannot be null or empty.", nameof(identifier));
            
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Machine display name cannot be null or empty.", nameof(displayName));

        if (powerConsumption < 0)
            throw new ArgumentException("Machine power consumption cannot be negative.", nameof(powerConsumption));

        Identifier = identifier;
        DisplayName = displayName;
        PowerConsumption = powerConsumption;
    }
} 