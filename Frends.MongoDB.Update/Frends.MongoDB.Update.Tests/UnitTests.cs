using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Update.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frends.MongoDB.Update.Tests;

[TestClass]
public class UnitTests
{
    /* 
        Run command 'docker-compose up -d' in \Frends.MongoDB.Update.Tests\Files\
    */

    private static readonly Connection _connection = new()
    {
        ConnectionString = "mongodb://admin:Salakala@localhost:27017/?authSource=admin",
        Database = "testdb",
        CollectionName = "testcoll",
    };

    private readonly string _doc1 = "{ 'foo':'bar', 'bar': 'foo' }";
    private readonly string _doc2 = "{ 'foo':'bar', 'bar': 'foo' }";
    private readonly string _doc3 = "{ 'qwe':'rty', 'asd': 'fgh' }";


    [TestInitialize]
    public void StartUp()
    {
        InsertTestData();
    }

    [TestCleanup]
    public void CleanUp()
    {
        UpdateTestData();
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
            UpdateString = "{$set: {foo:'update'}}"
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
            UpdateString = "{$set: {foo:'update'}}"
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

    private void InsertTestData()
    {
        try
        {
            var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);

            var doc1 = BsonDocument.Parse(_doc1);
            var doc2 = BsonDocument.Parse(_doc2);
            var doc3 = BsonDocument.Parse(_doc3);

            collection.InsertOne(doc1);
            collection.InsertOne(doc2);
            collection.InsertOne(doc3);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private static void UpdateTestData()
    {
        var collection = GetMongoCollection(_connection.ConnectionString, _connection.Database, _connection.CollectionName);

        var filter1 = "{'bar':'foo'}";
        var filter2 = "{'qwe':'rty'}";
        var filter3 = "{'asd':'fgh'}";
        collection.DeleteMany(filter1);
        collection.DeleteMany(filter2);
        collection.DeleteMany(filter3);
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
}