using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Frends.MongoDB.Index.Definitions;

/// <summary>
/// Input parameter.
/// </summary>
public class Input
{
	/// <summary>
	/// Creates or deletes an index.
	/// </summary>
	/// <example>IndexAction.Create</example>
	[DefaultValue(IndexAction.Create)]
	public IndexAction IndexAction { get; set; }

	/// <summary>
	/// Name of the index, if not specified, MongoDB will generate a name.
	/// </summary>
	[DefaultValue("")]
	public string IndexName { get; set; }

	/// <summary>
	/// An array of field name(s) to be included in the index.
	/// </summary>
	/// <example>name, postalCode</example>
	[UIHint(nameof(IndexAction), "", IndexAction.Create)]
	public FieldNames[] Fields { get; set; }

	/// <summary>
	/// Specifies whether to drop the existing index with the same name.
	/// </summary>
	[UIHint(nameof(IndexAction), "", IndexAction.Create)]
	public bool DropExistingIndex { get; set; } = false;
}

/// <summary>
/// Input.FieldName values.
/// </summary>
public class FieldNames
{
    /// <summary>
    /// Value.
    /// </summary>
    /// <example>name</example>
    public string Value { get; set; }
}