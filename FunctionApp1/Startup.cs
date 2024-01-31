using FunctionApp1.Interfaces;
using FunctionApp1.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var apiUrl = Environment.GetEnvironmentVariable("API_URL");

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });

            builder.Services.AddSingleton<ITableStorage, TableStorage>(s =>
            {
                string connectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString");
                return new TableStorage(connectionString, "LogsTableName");
            });

            builder.Services.AddSingleton<IBlobStorage, AzureBlobStorage>(s =>
            {
                string connectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
                string containerName = Environment.GetEnvironmentVariable("BlobStorageContainerName");
                return new AzureBlobStorage(connectionString, containerName);
            });

            builder.Services.AddSingleton<ILogRepository, AzureStorageLogRepository>();
        }
    }
}
