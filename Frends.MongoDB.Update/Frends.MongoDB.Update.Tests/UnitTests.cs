using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Update.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frends.MongoDB.Update.Tests;

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

	private readonly List<string> _documents = new() 
	{
		"{ 'foo':'bar', 'bar': 'foo' }",
		"{ 'foo':'bar', 'bar': 'foo' }",
		"{ 'qwe':'rty', 'asd': 'fgh' }",
		"{ 'array':'arr', 'children': [{'child':1, 'value': 'val1' }, {'child':2, 'value': 'val2' }] }"
	};

	[TestInitialize]
    public void StartUp()
    {
        InsertTestData();
    }

    [TestCleanup]
    public void CleanUp()
    {
		DeleteTestData();
	}

	[TestMethod]
    public async Task Test_Update_NotFound()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            UpdateOptions = Definitions.UpdateOptions.UpdateOne,
            Filter = "{'not':'found'}",
            Filters = null,
            File = null,
            UpdateString = "{$set: {foo:'update'}}"
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, result.Count);
        Assert.IsFalse(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_Single_UpdateOne()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            UpdateOptions = Definitions.UpdateOptions.UpdateOne,
			Filter = "{'foo':'bar'}",
			Filters = null,
            File = null,
            UpdateString = "{$set: {foo:'update'}}",
			Upsert = true
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_Single_UpdateMany()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            UpdateOptions = Definitions.UpdateOptions.UpdateMany,
            Filter = "{'foo':'bar'}",
            Filters = null,
            File = null,
            UpdateString = "{$set: {foo:'update'}}",
			Upsert = true
		};

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_Array_UpdateOne()
    {
        var doc1 = new DocumentValues() { Value = "{'foo':'bar'}" };
        var doc2 = new DocumentValues() { Value = "{'qwe':'rty'}" };

        var _input = new Input()
        {
            InputType = InputType.Filters,
            UpdateOptions = Definitions.UpdateOptions.UpdateOne,
            Filter = null,
            Filters = new[] { doc1, doc2 },
            File = null,
            UpdateString = "{$set: {foo:'update'}}"
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_Array_UpdateMany()
    {
        var doc1 = new DocumentValues() { Value = "{'foo':'bar'}" };
        var doc2 = new DocumentValues() { Value = "{'qwe':'rty'}" };

        var _input = new Input()
        {
            InputType = InputType.Filters,
            UpdateOptions = Definitions.UpdateOptions.UpdateMany,
            Filter = null,
            Filters = new[] { doc1, doc2 },
            File = null,
            UpdateString = "{$set: {foo:'update'}}"
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_File_UpdateOne()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            UpdateOptions = Definitions.UpdateOptions.UpdateOne,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
            UpdateString = "{$set: {foo:'update'}}"
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public async Task Test_Update_File_UpdateMany()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            UpdateOptions = Definitions.UpdateOptions.UpdateMany,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
            UpdateString = "{$set: {foo:'update'}}"
        };

        var result = await MongoDB.Update(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(GetDocuments("update"));
    }

    [TestMethod]
    public void Test_InvalidConnectionString()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            UpdateOptions = Definitions.UpdateOptions.UpdateMany,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
            UpdateString = "{$set: {foo:'update'}}"
        };

        var connection = new Connection
        {
			ConnectionString = "mongodb://admin:Incorrect@localhost:27017/?authSource=invalid",
			CollectionName = _connection.CollectionName,
            Database = _connection.Database,
        };

        var ex = Assert.ThrowsExceptionAsync<Exception>(async () => await MongoDB.Update(_input, connection, default));
        Assert.IsTrue(ex.Result.Message.StartsWith("Update error: System.Exception: UpdateOperation error: MongoDB.Driver.MongoAuthenticationException: Unable to authenticate using sasl protocol mechanism SCRAM-SHA-1."));
    }

	[TestMethod]
	public async Task Test_Upsert_Create_Document()
	{
		var _input = new Input()
		{
			InputType = InputType.Filter,
			UpdateOptions = Definitions.UpdateOptions.UpdateOne,
			Filter = "{'foobar':'upsert_create'}",
			Filters = null,
			File = null,
			UpdateString = "{$set: {foobar:'upsert_create'}}",
			Upsert = true
		};

		var result = await MongoDB.Update(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(1, result.Count);
		Assert.IsTrue(GetDocuments("upsert_create"));
	}

	[TestMethod]
	public async Task Test_Upsert_Dont_Create_Document()
	{
		var _input = new Input()
		{
			InputType = InputType.Filter,
			UpdateOptions = Definitions.UpdateOptions.UpdateOne,
			Filter = "{'foobar':'upsert_none'}",
			Filters = null,
			File = null,
			UpdateString = "{$set: {foo:'upsert_none'}}",
			Upsert = false
		};

		var result = await MongoDB.Update(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(0, result.Count);
		Assert.IsFalse(GetDocuments("upsert_none"));
	}

	[TestMethod]
	public void Test_Failing_When_No_Filter()
	{
		var _input = new Input()
		{
			InputType = InputType.Filter,
			UpdateOptions = Definitions.UpdateOptions.UpdateMany,
			Filter = "",
			Filters = null,
			File = null,
			UpdateString = "{$set: {foo:'update'}}",
			Upsert = true
		};

		var ex = Assert.ThrowsExceptionAsync<Exception>(async () => await MongoDB.Update(_input, _connection, default));
		Assert.IsTrue(ex.Result.Message.StartsWith("Update error: System.ArgumentException: Filter string missing."));
	}

	[TestMethod]
	public async Task Test_Update_Item_In_Child_Array()
	{
		var _input = new Input()
		{
			InputType = InputType.Filter,
			UpdateOptions = Definitions.UpdateOptions.UpdateOne,
			Filter = "{'array':'arr'}",
			Filters = null,
			File = null,
			UpdateString = "{$set: {'children.$[i].value': 'new_value'}}",
			ArrayFilter = "{'i.child': 1, 'i.value': 'val1'}",
			Upsert = false
		};

		var result = await MongoDB.Update(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(1, result.Count);

		var document = GetSingleDocuments(_input.Filter);
		Assert.IsNotNull(document);
		Assert.AreEqual("new_value", document["children"].AsBsonArray[0]["value"].AsString);
	}

	[TestMethod]
	public async Task Test_Dont_Update_Item_In_Child_Array()
	{
		var _input = new Input()
		{
			InputType = InputType.Filter,
			UpdateOptions = Definitions.UpdateOptions.UpdateOne,
			Filter = "{'array':'arr'}",
			Filters = null,
			File = null,
			UpdateString = "{$set: {'children.$[i].value': 'new_value'}}",
			ArrayFilter = "{'i.child': 1, 'i.value': 'val2'}",
			Upsert = false
		};

		var result = await MongoDB.Update(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(0, result.Count);
	}

	private void InsertTestData()
    {
        try
        {
            var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);

			foreach (var doc in _documents)
			{
				collection.InsertOne(BsonDocument.Parse(doc));
			}
		}
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static void DeleteTestData()
    {
        var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);

		List<string> filters = new() 
		{
			"{'bar':'foo'}",
			"{'qwe':'rty'}",
			"{'asd':'fgh'}",
			"{foo:'update'}",
			"{'foobar':'upsert_create'}",
			"{'array':'arr'}"
		};

		foreach (var filter in filters)
		{
			collection.DeleteMany(filter);
		}
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

    private static bool GetDocuments(string updated)
    {
        var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);
        var documents = collection.Find(new BsonDocument()).ToList();
        var i = documents.Any(x => x.Values.Contains(updated));
        return i;
    }

	private static BsonDocument? GetSingleDocuments(string filter)
	{
		var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);
		return collection.Find(BsonDocument.Parse(filter)).FirstOrDefault();
	}
}