namespace Flow.Core.Models;

public class Recipe
{
    public string Identifier { get; }
    public string DisplayName { get; }
    public IReadOnlyCollection<ItemStack> Inputs { get; }
    public IReadOnlyCollection<ItemStack> Outputs { get; }
    public Machine Machine { get; }
    public TimeSpan ProcessingTime { get; }

    public Recipe(
        string identifier,
        string displayName,
        IEnumerable<ItemStack> inputs,
        IEnumerable<ItemStack> outputs,
        Machine machine,
        TimeSpan processingTime)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Recipe identifier cannot be null or empty.", nameof(identifier));
            
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Recipe display name cannot be null or empty.", nameof(displayName));

        var inputList = (inputs ?? throw new ArgumentNullException(nameof(inputs))).ToList();
        if (inputList.Count == 0)
            throw new ArgumentException("Recipe must have at least one input.", nameof(inputs));

        var outputList = (outputs ?? throw new ArgumentNullException(nameof(outputs))).ToList();
        if (outputList.Count == 0)
            throw new ArgumentException("Recipe must have at least one output.", nameof(outputs));

        Machine = machine ?? throw new ArgumentNullException(nameof(machine));

        if (processingTime <= TimeSpan.Zero)
            throw new ArgumentException("Recipe processing time must be positive.", nameof(processingTime));

        Identifier = identifier;
        DisplayName = displayName;
        Inputs = inputList.AsReadOnly();
        Outputs = outputList.AsReadOnly();
        ProcessingTime = processingTime;
    }
} 