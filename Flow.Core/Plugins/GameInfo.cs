namespace Flow.Core.Plugins;

/// <summary>
/// Represents basic information about a registered game plugin.
/// </summary>
/// <param name="Name">The name of the game.</param>
/// <param name="Version">The version of the game plugin.</param>
public record GameInfo(string Name, Version Version); 