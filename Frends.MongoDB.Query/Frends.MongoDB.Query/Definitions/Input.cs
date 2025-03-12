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
	/// <example>DeleteOptions.DeleteOne</example>
	[DefaultValue(QueryOptions.QueryMany)]
	public QueryOptions QueryOptions { get; set; }


	/// <summary>
	/// Filter document.
	/// </summary>
	/// <example>{'foo':'bar'}</example>
	public string Filter { get; set; }

	/// <summary>
	/// Json output mode.
	/// </summary>
	/// <example>JsonOutputMode.RelaxedExtendedJson</example>
	[DefaultValue(JsonOutputMode.RelaxedExtendedJson)]
	public JsonOutputMode JsonOutputMode { get; set; }
}