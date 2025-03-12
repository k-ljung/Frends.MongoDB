using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Insert.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frends.MongoDB.Insert.Tests;

[TestClass]
public class UnitTests
{
	/* 
        Run command 'docker-compose up -d' in \Frends.MongoDB.Insert.Tests\Files\
    */

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
    public async Task Test_Insert_Document()
    {
        var _input = new Input()
        {
            InputType = InputType.Document,
            Document = "{ 'foo':'bar1', 'bar': 'foo' }",
            File = null,
        };

        var result = await MongoDB.Insert(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.IsNotNull(result.Id);
		Assert.IsNotNull(GetSingleDocuments("{'foo':'bar1'}"));
	}


    [TestMethod]
    public async Task Test_Insert_Documents()
    {
        var doc1 = new DocumentValues() { Value = "{ 'foo':'bar2', 'bar': 'foo' }" };
        var doc2 = new DocumentValues() { Value = "{ 'foo':'bar2', 'bar': 'foo' }" };

        var _input = new Input()
        {
            InputType = InputType.Documents,
            Documents = new[] { doc1, doc2 },
            Document = null,
            File = null,
        };

		var result = await MongoDB.Insert(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.IsNotNull(result.Id);
		Assert.IsTrue(GetDocuments("bar2"));
	}


    [TestMethod]
    public async Task Test_Insert_File()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            Document = null,
            File = "..//..//..//Files//testdata.json",
        };

		var result = await MongoDB.Insert(_input, _connection, default);
		Assert.IsTrue(result.Success);
		Assert.IsNotNull(result.Id);
		Assert.IsTrue(GetDocuments("bar100"));
	}

	private static void DeleteTestData()
	{
		var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);

		List<string> filters = new()
		{
			"{'foo':'bar'}",
			"{'foo':'bar1'}",
			"{'foo':'bar2'}",
			"{'foo':'bar3'}",
			"{'foo':'bar100'}"
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