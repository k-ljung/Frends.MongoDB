using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.MongoDB.Delete.Definitions;

/// <summary>
/// Input parameter.
/// </summary>
public class Input
{
    /// <summary>
    /// Delete single or multiple documents.
    /// </summary>
    /// <example>DeleteOptions.DeleteOne</example>
    [DefaultValue(DeleteOptions.DeleteOne)]
    public DeleteOptions DeleteOptions { get; set; }

    /// <summary>
    /// Input type.
    /// </summary>
    /// <example>InputType.Document</example>
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