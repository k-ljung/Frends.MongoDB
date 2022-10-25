namespace Frends.MongoDB.Insert.Definitions;

/// <summary>
/// InputType options.
/// </summary>
public enum InputType
{
    /// <summary>
    /// File containing data in JSON format.
    /// </summary>
    File,

    /// <summary>
    /// A single JSON string to be processed.
    /// </summary>
    Document,

    /// <summary>
    /// JSON string(s) to be processed.
    /// </summary>
    Documents
}