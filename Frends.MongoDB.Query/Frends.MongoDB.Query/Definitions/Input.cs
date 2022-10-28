namespace Frends.MongoDB.Query.Definitions;

/// <summary>
/// Input parameter.
/// </summary>
public class Input
{
    /// <summary>
    /// Filter document.
    /// </summary>
    /// <example>{'foo':'bar'}</example>
    public string Filter { get; set; }
}