﻿using Frends.MongoDB.Update.Definitions;
using System.ComponentModel;
using System;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Threading.Tasks;
using UpdateOptions = MongoDB.Driver.UpdateOptions;

namespace Frends.MongoDB.Update;

/// <summary>
/// MongoDB Task.
/// </summary>
public class MongoDB
{
    /// <summary>
    /// MongoDB update operation.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.MongoDB.Update)
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="cancellationToken">Token generated by Frends to stop this task.</param>
    /// <returns>Object { bool Success, long Count }</returns>
    public static async Task<Result> Update([PropertyTab] Input input, [PropertyTab] Connection connection, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.UpdateString))
            throw new Exception("Update string missing.");

        var collection = GetMongoCollection(connection.ConnectionString, connection.Database, connection.CollectionName);

        try
        {
            switch (input.InputType)
            {
                case InputType.File:
                    using (var streamReader = new StreamReader(input.File))
                    {
                        string line;
                        while ((line = await File.ReadAllTextAsync(input.File, cancellationToken)) != null)
                            if (line != null)
                                return new Result(true, await UpdateOperation(input, BsonDocument.Parse(line), collection, cancellationToken));
                        return new Result(false, 0);
                    }

                case InputType.Filter:
                    return new Result(true, await UpdateOperation(input, BsonDocument.Parse(input.Filter), collection, cancellationToken));

                case InputType.Filters:
                    long count = 0;
                    foreach (var document in input.Filters)
                        count += await UpdateOperation(input, BsonDocument.Parse(document.Value), collection, cancellationToken);
                    return new Result(true, count);

                default:
                    return new Result(false, 0);
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Update error: {ex}");
        }
    }

    private static async Task<long> UpdateOperation(Input input, BsonDocument filter, IMongoCollection<BsonDocument> collection, CancellationToken cancellationToken)
    {
        try
        {
            UpdateDefinition<BsonDocument> update = input.UpdateString;
			var options = new UpdateOptions
			{
				IsUpsert = input.Upsert
			};

			switch (input.UpdateOptions)
            {
                case Definitions.UpdateOptions.UpdateOne:
                    var updateOne = await collection.UpdateOneAsync(filter, update, options, cancellationToken: cancellationToken);
                    return updateOne.ModifiedCount;
                case Definitions.UpdateOptions.UpdateMany:
                    var updateMany = await collection.UpdateManyAsync(filter, update, options, cancellationToken: cancellationToken);
                    return updateMany.ModifiedCount;
                default:
                    return 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"UpdateOperation error: {ex}");
        }
    }

    private static IMongoCollection<BsonDocument> GetMongoCollection(string connectionString, string database, string collectionName)
    {
        try
        {
            var dataBase = GetMongoDatabase(connectionString, database);
            var collection = dataBase.GetCollection<BsonDocument>(collectionName);
            return collection;
        }
        catch (Exception ex)
        {
            throw new Exception($"GetMongoCollection error: {ex}");
        }
    }

    private static IMongoDatabase GetMongoDatabase(string connectionString, string database)
    {
        try
        {
            var mongoClient = new MongoClient(connectionString);
            var dataBase = mongoClient.GetDatabase(database);
            return dataBase;
        }
        catch (Exception ex)
        {
            throw new Exception($"GetMongoDatabase error: {ex}");
        }
    }
}