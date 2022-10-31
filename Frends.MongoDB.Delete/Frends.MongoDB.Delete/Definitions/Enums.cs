namespace Frends.MongoDB.Delete.Definitions;

/// <summary>
/// Delete options.
/// </summary>
public enum DeleteOptions
{
    /// <summary>
    /// Delete at most a single document that match a specified filter even though multiple documents may match the specified filter.
    /// </summary>
    DeleteOne,

    /// <summary>
    /// Delete all documents that match a specified filter.
    /// </summary>
    DeleteMany,
}

/// <summary>
/// InputType options.
/// </summary>
public enum InputType
{
    /// <summary>
    /// A single filter to be processed.
    /// </summary>
    Filter,

    /// <summary>
    /// An array of filter(s) to be processed one by one.
    /// </summary>
    Filters,

    /// <summary>
    /// The file containing filter(s). Each line is a single filter and will be processed one by one.
    /// </summary>
    File
}