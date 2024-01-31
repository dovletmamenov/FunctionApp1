using FunctionApp1.Interfaces;
using FunctionApp1.Models.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionApp1.Mapper;
using System.Linq;

namespace FunctionApp1.Services
{
    public class AzureStorageLogRepository : ILogRepository
    {
        private readonly IBlobStorage _blobStorageService;
        
        private readonly ITableStorage _tableStorage;

        public AzureStorageLogRepository(IBlobStorage blobStorageService, ITableStorage tableStorage) 
        {
            _blobStorageService = blobStorageService;
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<RequestLog>> RetrieveRequestLogsAsync(DateTime from, DateTime to)
        {
            var logs = await _tableStorage.GetLogsAsync(from, to);
            return logs.Select(x => x.ToRequestLog()).ToList();
        }

        public async Task<string> RetrieveResponseContentAsync(DateTime requestTime)
        {
            return await _blobStorageService.RetrieveResponseContentAsync(requestTime);
        }

        public async Task StoreRequestResultAsync(DateTime time, HttpResponseMessage httpResponseMessage, string responsePayload)
        {
            // run saving in parallelel
            var saveTableTask = _tableStorage.StoreRequestLogAsync(time, httpResponseMessage);
            var saveBlobTask = _blobStorageService.StoreResponseContentAsync(time, responsePayload);
            

            // Wait for both tasks to complete
            await Task.WhenAll(saveTableTask, saveBlobTask);
        }        
    }
}
