using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TableStorageRepository<T> : ITableStorageRepository<T> where T : TableEntity, new()
{
    private CloudTable table;

    public TableStorageRepository(string tableName)
    {
        string? connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        table = tableClient.GetTableReference(tableName);
        table.CreateIfNotExistsAsync();
    }

    public async Task<T> GetAsync(string partitionKey, string rowKey)
    {
        TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
        TableResult result = await table.ExecuteAsync(retrieveOperation);
        return (T)result.Result;
    }

    public async Task<IEnumerable<T>> GetAllAsync(string partitionKey)
    {
        TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        TableContinuationToken token = null;
        var entities = new List<T>();

        do
        {
            TableQuerySegment<T> queryResult = await table.ExecuteQuerySegmentedAsync(query, token);
            entities.AddRange(queryResult.Results);
            token = queryResult.ContinuationToken;
        } while (token != null);

        return entities;
    }

    public async Task InsertAsync(T entity)
    {
        TableOperation insertOperation = TableOperation.Insert(entity);
        await table.ExecuteAsync(insertOperation);
    }

    public async Task UpdateAsync(T entity)
    {
        TableOperation updateOperation = TableOperation.Replace(entity);
        await table.ExecuteAsync(updateOperation);
    }

    public async Task DeleteAsync(string partitionKey, string rowKey)
    {
        var entity = await GetAsync(partitionKey, rowKey);
        if (entity != null)
        {
            TableOperation deleteOperation = TableOperation.Delete(entity);
            await table.ExecuteAsync(deleteOperation);
        }
    }
}
