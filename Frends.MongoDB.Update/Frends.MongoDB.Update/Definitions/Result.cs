namespace Frends.MongoDB.Update.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Update complete.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Count of updated documents.
    /// </summary>
    /// <example>1</example>
    public long Count { get; private set; }

    internal Result(bool success, long count)
    {
        Success = success;
        Count = count;
    }
}