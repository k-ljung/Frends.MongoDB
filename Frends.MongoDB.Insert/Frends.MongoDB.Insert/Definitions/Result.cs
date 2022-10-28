using System.Collections.Generic;

namespace Frends.MongoDB.Insert.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Document(s) added.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// List of ID(s) of the added document(s).
    /// </summary>
    /// <example>6357ae6e8b77e824c8381113</example>
    public List<string> Id { get; private set; }

    internal Result(bool success, List<string> id)
    {
        Success = success;
        Id = id;
    }
}