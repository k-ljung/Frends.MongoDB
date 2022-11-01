using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Update.Definitions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 0);
        Assert.IsTrue(GetDocuments("update").Equals(false));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 1);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 2);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 2);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 3);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 1);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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
        Assert.IsTrue(result.Success.Equals(true) && result.Count == 2);
        Assert.IsTrue(GetDocuments("update").Equals(true));
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