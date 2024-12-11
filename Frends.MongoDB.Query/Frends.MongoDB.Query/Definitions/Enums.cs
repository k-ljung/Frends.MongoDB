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
