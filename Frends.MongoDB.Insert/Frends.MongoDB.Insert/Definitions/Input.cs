using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.MongoDB.Insert.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Input type.
    /// InputType.Document: A single JSON string to be processed.
    /// InputType.Documents: JSON string(s) to be processed.
    /// InputType.File: File containing data in JSON format.
    /// </summary>
    [DefaultValue(InputType.Document)]
    public InputType InputType { get; set; }

    /// <summary>
    /// A single JSON string to be processed.
    /// </summary>
    /// <example>{"foo": "bar", "bar": "foo"}</example>
    [UIHint(nameof(InputType), "", InputType.Document)]
    public string Document { get; set; }

    /// <summary>
    /// JSON string(s) to be processed.
    /// </summary>
    /// <example>{{"foo": "bar", "bar": "foo"}, {"foo": "bar", "bar": "foo"}}</example>
    [UIHint(nameof(InputType), "", InputType.Documents)]
    public DocumentValues[] Documents { get; set; }

    /// <summary>
    /// File containing data in JSON format.
    /// </summary>
    /// <example>c:\temp\file.json</example>
    [UIHint(nameof(InputType), "", InputType.File)]
    public string File { get; set; }
}

/// <summary>
/// Input.Documents values.
/// </summary>
public class DocumentValues
{
    /// <summary>
    /// Value.
    /// </summary>
    /// <example>{"foo": "bar", "bar": "foo"}</example>
    public string Value { get; set; }
}