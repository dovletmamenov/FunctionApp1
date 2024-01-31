using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionApp1.Interfaces
{
    public interface ITableStorage
    {
        public Task StoreRequestLogAsync(DateTime requestTime, HttpResponseMessage httpResponseMessage);

        public Task<IEnumerable<TableEntity>> GetLogsAsync(DateTime from, DateTime to);
    }
}
