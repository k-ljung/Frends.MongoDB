using System.ComponentModel;

namespace Frends.MongoDB.Query.Definitions;

/// <summary>
/// Input parameter.
/// </summary>
public class Input
{
	/// <summary>
	/// Query single or multiple documents.
	/// </summary>
	/// <example>QueryOptions.QueryMany</example>
	[DefaultValue(QueryOptions.QueryMany)]
	public QueryOptions QueryOptions { get; set; } = QueryOptions.QueryMany;


	/// <summary>
	/// Filter document.
	/// </summary>
	/// <example>{'foo':'bar'}</example>
	public string Filter { get; set; }
}