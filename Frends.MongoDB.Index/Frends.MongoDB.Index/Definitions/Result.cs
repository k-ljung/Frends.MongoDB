namespace Frends.MongoDB.Index.Definitions;

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
    /// Name of the created index.
    /// </summary>
    /// <example>1</example>
    public string IndexName { get; private set; }

    internal Result(bool success, string indexName)
    {
        Success = success;
		IndexName = indexName;
    }
}