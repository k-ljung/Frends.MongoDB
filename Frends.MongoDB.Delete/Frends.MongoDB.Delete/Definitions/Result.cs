namespace Frends.MongoDB.Delete.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Delete complete.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Count of deleted documents.
    /// </summary>
    /// <example>1</example>
    public long Count { get; private set; }

    internal Result(bool success, long count)
    {
        Success = success;
        Count = count;
    }
}