namespace Frends.MongoDB.Update.Definitions;

/// <summary>
/// Update options.
/// </summary>
public enum UpdateOptions
{
    /// <summary>
    /// Updates a single document that match a specified filter.
    /// </summary>
    UpdateOne,

    /// <summary>
    /// Update all documents that match a specified filter.
    /// </summary>
    UpdateMany,
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
    /// The file containing filter.
    /// </summary>
    File
}