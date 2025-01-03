using System.Collections.ObjectModel;

namespace Flow.Core.Models.Graph;

/// <summary>
/// Represents the throughput of a node or graph, including item flow rates and power consumption.
/// </summary>
public class Throughput
{
    private readonly Dictionary<Item, decimal> _inputsPerMinute = new();
    private readonly Dictionary<Item, decimal> _outputsPerMinute = new();

    /// <summary>
    /// Gets the input items per minute for each item type.
    /// </summary>
    public IReadOnlyDictionary<Item, decimal> InputsPerMinute => new ReadOnlyDictionary<Item, decimal>(_inputsPerMinute);

    /// <summary>
    /// Gets the output items per minute for each item type.
    /// </summary>
    public IReadOnlyDictionary<Item, decimal> OutputsPerMinute => new ReadOnlyDictionary<Item, decimal>(_outputsPerMinute);

    /// <summary>
    /// Gets the total power consumption in watts.
    /// </summary>
    public decimal PowerConsumption { get; }

    /// <summary>
    /// Creates a new throughput instance.
    /// </summary>
    /// <param name="inputsPerMinute">The input items per minute for each item type.</param>
    /// <param name="outputsPerMinute">The output items per minute for each item type.</param>
    /// <param name="powerConsumption">The total power consumption in watts.</param>
    public Throughput(
        IEnumerable<KeyValuePair<Item, decimal>> inputsPerMinute,
        IEnumerable<KeyValuePair<Item, decimal>> outputsPerMinute,
        decimal powerConsumption)
    {
        ArgumentNullException.ThrowIfNull(inputsPerMinute);
        ArgumentNullException.ThrowIfNull(outputsPerMinute);

        foreach (var (item, rate) in inputsPerMinute)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (rate <= 0)
                throw new ArgumentException($"Input rate for {item.DisplayName} must be positive.", nameof(inputsPerMinute));
            _inputsPerMinute[item] = rate;
        }

        foreach (var (item, rate) in outputsPerMinute)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (rate <= 0)
                throw new ArgumentException($"Output rate for {item.DisplayName} must be positive.", nameof(outputsPerMinute));
            _outputsPerMinute[item] = rate;
        }

        if (powerConsumption < 0)
            throw new ArgumentException("Power consumption cannot be negative.", nameof(powerConsumption));

        PowerConsumption = powerConsumption;
    }
} 