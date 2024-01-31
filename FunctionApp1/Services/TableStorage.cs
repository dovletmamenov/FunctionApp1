using System;

namespace FunctionApp1.Services
{
    using Azure;
    using Azure.Data.Tables;
    using FunctionApp1.Interfaces;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TableStorage : ITableStorage
    {
        private readonly TableClient _tableClient;

        public TableStorage(string connectionString, string tableName)
        {
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task StoreRequestLogAsync(DateTime requestTime, HttpResponseMessage httpResponseMessage)
        {
            var entity = CreateEntity(requestTime, httpResponseMessage);
            await AddEntityAsync(entity);
        }

        public async Task<IEnumerable<TableEntity>> GetLogsAsync(DateTime from, DateTime to)
        {
            var logs = new List<TableEntity>();

            // Iterate over each day in the range
            for (var day = from.Date; day <= to.Date; day = day.AddDays(1))
            {
                string partitionKey = day.ToString("yyyyMMdd");

                // Query to filter logs based on the PartitionKey
                string filter = $"PartitionKey eq '{partitionKey}'";
                AsyncPageable<TableEntity> queryResults = _tableClient.QueryAsync<TableEntity>(filter);

                await foreach (TableEntity entity in queryResults)
                {                    
                    if (entity.Timestamp.Value.UtcDateTime >= from && entity.Timestamp.Value.UtcDateTime <= to)
                    {
                        logs.Add(entity);
                    }
                }
            }

            return logs;
        }

        private TableEntity CreateEntity(DateTime requestTime, HttpResponseMessage httpResponseMessage)
        {
            string partitionKey = requestTime.ToString("yyyyMMdd");
            string rowKey = requestTime.ToString("yyyyMMddHHmmssfff");

            var entity = new TableEntity(partitionKey, rowKey) 
            {
                { "StatusCode", httpResponseMessage.StatusCode.ToString() } 
            };

            return entity;
        }

        private async Task AddEntityAsync(TableEntity entity)
        {
            await _tableClient.AddEntityAsync(entity);
        }
    }

}
