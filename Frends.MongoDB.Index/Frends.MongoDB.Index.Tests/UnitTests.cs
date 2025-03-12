using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Index.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frends.MongoDB.Index.Tests;

[TestClass]
public class UnitTests
{
	/// <summary>
	/// Run command 'docker-compose up -d' in \Frends.MongoDB.Update.Tests\Files\
	/// </summary>

	private static readonly Connection _connection = new()
	{
		ConnectionString = "mongodb://admin:Salakala@localhost:27017/?authSource=admin",
		Database = "testdb",
		CollectionName = "testcoll",
	};

	[TestCleanup]
	public void CleanUp()
	{
		DeleteTestData();
	}

	[TestMethod]
    public async Task Test_Create_Single_Field_Index_Generate_Name()
    {
        var _input = new Input()
        {
            IndexAction = IndexAction.Create,
			Fields = new FieldNames[] 
			{ 
				new() { Value = "foo" }
			},
			DropExistingIndex = false
		};

		var result = await MongoDB.Index(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual("foo_1", result.IndexName);
	}

	[TestMethod]
	public async Task Test_Multi_Field_Index_Generate_Name()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "foo" },
				new() { Value = "bar", }
			},
			DropExistingIndex = false
		};

		var result = await MongoDB.Index(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual("foo_1_bar_1", result.IndexName);
	}

	[TestMethod]
	public async Task Test_Create_Index_With_Given_Name()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "foobar" }
			},
			IndexName = "foobar_index",
			DropExistingIndex = false
		};

		var result = await MongoDB.Index(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual("foobar_index", result.IndexName);
	}

	[TestMethod]
	public async Task Test_Try_Create_Index_Without_Fields()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			IndexName = "nofields",
			DropExistingIndex = false
		};

		var ex = await Assert.ThrowsExceptionAsync<Exception>(async () => await MongoDB.Index(_input, _connection, default));
		Assert.IsTrue(ex.Message.StartsWith("Index error: System.ArgumentException: Field name(s) missing."));
	}

	[TestMethod]
	public async Task Test_Try_Create_Index_Where_Already_Exists()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
					new() { Value = "existing" }
			},
			DropExistingIndex = false
		};

		await MongoDB.Index(_input, _connection, default);


		_input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "existing" }
			},
			IndexName = "existing_index",
			DropExistingIndex = false
		};

		var ex = await Assert.ThrowsExceptionAsync<Exception>(async () => await MongoDB.Index(_input, _connection, default));
		Assert.IsTrue(ex.Message.StartsWith("Index error: MongoDB.Driver.MongoCommandException: Command createIndexes failed: Index already exists with a different name: existing_1."));
	}

	[TestMethod]
	public async Task Test_Drop_Index_And_Create_With_Same_Name()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "dropandcreate" }
			},
			IndexName = "dropandcreate",
			DropExistingIndex = false
		};

		await MongoDB.Index(_input, _connection, default);


		_input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "dropandcreate" },
				new() { Value = "foo" }
			},
			IndexName = "dropandcreate",
			DropExistingIndex = true
		};

		var result = await MongoDB.Index(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual("dropandcreate", result.IndexName);
	}

	[TestMethod]
	public async Task Test_Drop_Index()
	{
		var _input = new Input()
		{
			IndexAction = IndexAction.Create,
			Fields = new FieldNames[]
			{
				new() { Value = "drop" }
			},
			IndexName = "drop",
			DropExistingIndex = false
		};

		await MongoDB.Index(_input, _connection, default);

		_input = new Input()
		{
			IndexAction = IndexAction.Drop,
			IndexName = "drop"
		};

		var result = await MongoDB.Index(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual("drop", result.IndexName);
	}

	private static void DeleteTestData()
	{
		var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);
		collection.Indexes.DropAll();
	}

	private static IMongoCollection<BsonDocument> GetMongoCollection(string connectionString, string database, string collectionName)
    {
        var dataBase = GetMongoDatabase(connectionString, database);
        var collection = dataBase.GetCollection<BsonDocument>(collectionName);
        return collection;
    }

    private static IMongoDatabase GetMongoDatabase(string connectionString, string database)
    {
        var mongoClient = new MongoClient(connectionString);
        var dataBase = mongoClient.GetDatabase(database);
        return dataBase;
    }
}