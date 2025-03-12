using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Query.Definitions;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson;

namespace Frends.MongoDB.Query.Tests;

[TestClass]
public class UnitTests
{
	/* 
        Run command 'docker-compose up -d' in \Frends.MongoDB.Query.Tests\Files\
    */

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
		"{ 'foo':99, 'bar': 'foo' }",
		"{ 'qwe':'rty', 'asd': 'fgh' }"
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
    public void Test_Query_TwoResults()
    {
        var _input = new Input()
        {
            Filter = "{'foo':'bar'}",
			QueryOptions = QueryOptions.QueryMany
		};

        var result = MongoDB.Query(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Data.Count);
    }

    [TestMethod]
    public void Test_Query_OneResults()
    {
        var _input = new Input()
        {
            Filter = "{'qwe':'rty'}"
		};

        var result = MongoDB.Query(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.Data.Count);
    }

	[TestMethod]
	public void Test_Query_RelaxedExtendedJson()
	{
		var _input = new Input()
		{
			Filter = "{'foo':99}",
			JsonOutputMode = Definitions.JsonOutputMode.RelaxedExtendedJson,
		};

		var result = MongoDB.Query(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(1, result.Data.Count);
		Assert.IsFalse(result.Data[0].Contains("$numberInt"));
	}

	[TestMethod]
	public void Test_Query_CanonicalExtendedJson()
	{
		var _input = new Input()
		{
			Filter = "{'foo':99}",
			JsonOutputMode = Definitions.JsonOutputMode.CanonicalExtendedJson,
		};

		var result = MongoDB.Query(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.AreEqual(1, result.Data.Count);
		Assert.IsTrue(result.Data[0].Contains("$numberInt"));
	}

	[TestMethod]
    public void Test_Query_NotFoundFilter()
    {
        var _input = new Input()
        {
            Filter = "{'not':'found'}",
        };

        var result = MongoDB.Query(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, result.Data.Count);
    }

    [TestMethod]
    public void Test_EmptyQuery()
    {
        var ex = Assert.ThrowsException<ArgumentException>(() => MongoDB.Query(new Input { Filter = "" }, _connection, default));
        Assert.AreEqual("Query error: Filter can't be null.", ex.Message);
    }

    [TestMethod]
    public void Test_InvalidConnectionString()
    {
        var input = new Input()
        {
            Filter = "{'not':'found'}",
        };

        var connection = new Connection
        {
            ConnectionString = "mongodb://admin:Incorrect@localhost:27017/?authSource=invalid",
            CollectionName = _connection.CollectionName,
            Database = _connection.Database,
        };

        var ex = Assert.ThrowsException<Exception>(() => MongoDB.Query(input, connection, default));
        Assert.IsTrue(ex.Message.StartsWith("Query error: MongoDB.Driver.MongoAuthenticationException: Unable to authenticate using sasl protocol mechanism SCRAM-SHA-1."));
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
}