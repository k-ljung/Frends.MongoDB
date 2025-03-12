using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.MongoDB.Index.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// Connection string.
    /// </summary>
    /// <example>mongodb://foo:bar@localhost:00000/?authSource=admin</example>
    [PasswordPropertyText]
    public string ConnectionString { get; set; }

	/// <summary>
	/// Database.
	/// </summary>
	/// <example>foo</example>
	public string Database { get; set; }

    /// <summary>
    /// Collection name.
    /// </summary>
    /// <example>bar</example>
    public string CollectionName { get; set; }
}