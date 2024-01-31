using FunctionApp1.Models.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionApp1.Interfaces
{
    public interface ILogRepository
    {
        public Task StoreRequestResultAsync(DateTime time, HttpResponseMessage httpResponseMessage, string responsePayload);

        public Task<IEnumerable<RequestLog>> RetrieveRequestLogsAsync(DateTime from, DateTime to);

        public Task<string> RetrieveResponseContentAsync(DateTime requestTime);
    }
}
