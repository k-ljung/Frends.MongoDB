namespace Frends.MongoDB.Query.Definitions;
/// <summary>
/// Query options.
/// </summary>
public enum QueryOptions
{
	/// <summary>
	/// Returns the first document that match a specified filter even though multiple documents may match the specified filter.
	/// </summary>
	QueryOne,

	/// <summary>
	/// Returns all documents that match a specified filter.
	/// </summary>
	QueryMany,
}

/// <summary>
/// Represents the output mode of a JsonWriter.
/// </summary>
public enum JsonOutputMode
{
	/// <summary>
	/// Use a format that can be pasted in to the MongoDB shell.
	/// </summary>
	Shell,

	/// <summary>
	/// Output canonical extended JSON.
	/// </summary>
	CanonicalExtendedJson,

	/// <summary>
	/// Output relaxed extended JSON.
	/// </summary>
	RelaxedExtendedJson,
}