using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Delete.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frends.MongoDB.Delete.Tests;

[TestClass]
public class UnitTests
{
    /* 
        Run command 'docker-compose up -d' in \Frends.MongoDB.Delete.Tests\Files\
    */

    private static readonly Connection _connection = new()
    {
        ConnectionString = "mongodb://admin:Salakala@localhost:27017/?authSource=admin",
        Database = "testdb",
        CollectionName = "testcoll",
    };

    private readonly string _doc1 = "{ \"foo\":\"bar\", \"bar\": \"foo\" }";
    private readonly string _doc2 = "{ \"foo\":\"bar\", \"bar\": \"foo\" }";
    private readonly string _doc3 = "{ \"qwe\":\"rty\", \"asd\": \"fgh\" }";


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
    public async Task Test_Delete_NotFound()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            DeleteOptions = Definitions.DeleteOptions.DeleteOne,
            Filter = "{'not':'found'}",
            Filters = null,
            File = null,
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_Single_DeleteOne()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            DeleteOptions = Definitions.DeleteOptions.DeleteOne,
            Filter = "{'foo':'bar'}",
            Filters = null,
            File = null,
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_Single_DeleteMany()
    {
        var _input = new Input()
        {
            InputType = InputType.Filter,
            DeleteOptions = Definitions.DeleteOptions.DeleteMany,
            Filter = "{'foo':'bar'}",
            Filters = null,
            File = null,
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_Array_DeleteOne()
    {
        var doc1 = new DocumentValues() { Value = "{'foo':'bar'}" };
        var doc2 = new DocumentValues() { Value = "{'qwe':'rty'}" };

        var _input = new Input()
        {
            InputType = InputType.Filters,
            DeleteOptions = Definitions.DeleteOptions.DeleteOne,
            Filter = null,
            Filters = new[] { doc1, doc2 },
            File = null,
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_Array_DeleteMany()
    {
        var doc1 = new DocumentValues() { Value = "{'foo':'bar'}" };
        var doc2 = new DocumentValues() { Value = "{'qwe':'rty'}" };

        var _input = new Input()
        {
            InputType = InputType.Filters,
            DeleteOptions = Definitions.DeleteOptions.DeleteMany,
            Filter = null,
            Filters = new[] { doc1, doc2 },
            File = null,
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(3, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_File_DeleteOne()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            DeleteOptions = Definitions.DeleteOptions.DeleteOne,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public async Task Test_Delete_File_DeleteMany()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            DeleteOptions = Definitions.DeleteOptions.DeleteMany,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
        };

        var result = await MongoDB.Delete(_input, _connection, default);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void Test_InvalidConnectionString()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            DeleteOptions = Definitions.DeleteOptions.DeleteMany,
            Filter = null,
            Filters = null,
            File = "..//..//..//Files//testdata.json",
        };

        var connection = new Connection
        {
            ConnectionString = "mongodb://admin:Incorrect@localhost:27017/?authSource=invalid",
            CollectionName = _connection.CollectionName,
            Database = _connection.Database,
        };

        var ex = Assert.ThrowsExceptionAsync<Exception>(async () => await MongoDB.Delete(_input, connection, default));
        Assert.IsTrue(ex.Result.Message.StartsWith("Delete error: System.Exception: DeleteOperation error: MongoDB.Driver.MongoAuthenticationException: Unable to authenticate using sasl protocol mechanism SCRAM-SHA-1."));
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
    private static void DeleteTestData()
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
}