using System.Collections.Generic;

namespace Frends.MongoDB.Query.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// Query completed.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// A list with the documents matching the search criteria .
    /// </summary>
    /// <example>"Object{ \"_id\" : { \"$oid\" : \"635b6e17083070195c6e98f0\" }, \"foo\" : \"bar\", \"bar\" : \"foo\" }"</example>
    public List<string> Data { get; private set; }

    internal Result(bool success, List<string> data)
    {
        Success = success;
        Data = data;
    }
}