using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.MongoDB.Insert.Definitions;

namespace Frends.MongoDB.Insert.Tests;

[TestClass]
public class UnitTests
{
    /*
        Run commands in ..\..\..\Files :
        'docker-compose up'
    */

    private static readonly Connection _connection = new()
    {
        ConnectionString = Environment.GetEnvironmentVariable("HiQ_MongoDBTest_ConnString"),
        Database = "testdb",
        CollectionName = "testcoll",
    };

    private readonly string _doc1 = "{ \"foo\":\"bar\", \"bar\": \"foo\" }";
    private readonly string _doc2 = "{ \"foo\":\"bar2\", \"bar\": \"foo2\" }";


    [TestMethod]
    public async Task Test_Insert_Document()
    {
        var _input = new Input()
        {
            InputType = InputType.Document,
            Document = _doc1,
            File = null,
        };

        var result = await MongoDB.Insert(_input, _connection, default);
        Assert.IsTrue(result.Success.Equals(true) && result.Id != null);
    }


    [TestMethod]
    public async Task Test_Insert_Documents()
    {
        var doc1 = new DocumentValues() { Value = _doc1 };
        var doc2 = new DocumentValues() { Value = _doc2 };

        var _input = new Input()
        {
            InputType = InputType.Documents,
            Documents = new[] { doc1, doc2 },
            Document = null,
            File = null,
        };

        var result = await MongoDB.Insert(_input, _connection, default);
        Assert.IsTrue(result.Success.Equals(true) && result.Id != null);
    }


    [TestMethod]
    public async Task Test_Insert_File()
    {
        var _input = new Input()
        {
            InputType = InputType.File,
            Document = null,
            File = @"..\..\..\Files\testdata.json",
        };

        var result = await MongoDB.Insert(_input, _connection, default);
        Assert.IsTrue(result.Success.Equals(true) && result.Id != null);
    }
}