using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Frends.MongoDB.Update.Definitions;

/// <summary>
/// Input parameter.
/// </summary>
public class Input
{
    /// <summary>
    /// Update single or multiple documents.
    /// </summary>
    /// <example>UpdateOptions.UpdateOne</example>
    [DefaultValue(UpdateOptions.UpdateOne)]
    public UpdateOptions UpdateOptions { get; set; }

    /// <summary>
    /// The value(s) to be updated to document(s).
    /// </summary>
    /// <example>"{ $set: { 'foo': 'updated'} }"</example>
    public string UpdateString { get; set; }

    /// <summary>
    /// Input type.
    /// </summary>
    /// <example>InputType.Filter</example>
    [DefaultValue(InputType.Filter)]
    public InputType InputType { get; set; }

    /// <summary>
    /// A single filter to be processed.
    /// </summary>
    /// <example>{'foo':'bar'}</example>
    [UIHint(nameof(InputType), "", InputType.Filter)]
    public string Filter { get; set; }

	/// <summary>
	/// An array of filter(s) to be processed one by one.
	/// </summary>
	/// <example>{{'foo':'bar'}, {'bar':'foo'}}</example>
	[UIHint(nameof(InputType), "", InputType.Filters)]
    public DocumentValues[] Filters { get; set; }

    /// <summary>
    /// The file containing filter.
    /// </summary>
    /// <example>c:\temp\file.json</example>
    [UIHint(nameof(InputType), "", InputType.File)]
    public string File { get; set; }

	/// <summary>
	/// Specifies the array filter for an update operation.
	/// In the example below this UpdateString is used: {$set: {'children.$[i].value': 'new_value'}} 
	/// </summary>
	/// <example>{'i.child': 1, 'i.value': 'val1'}</example>
	public string ArrayFilter { get; set; }

	/// <summary>
	/// Specifies whether the update operation performs an
	/// upsert operation if no documents match the query filter.
	/// </summary>
	public bool Upsert { get; set; } = false;
}

/// <summary>
/// Input.Documents values.
/// </summary>
public class DocumentValues
{
    /// <summary>
    /// Value.
    /// </summary>
    /// <example>{'foo':'bar', 'bar':'foo'}</example>
    public string Value { get; set; }
}