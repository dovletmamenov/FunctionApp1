using FunctionApp1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public class LogRetrievalEndpointFunction
    {
        private readonly ILogRepository _logRepository;

        public LogRetrievalEndpointFunction(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        [FunctionName("LogRetrievalEndpointFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var fromTime = DateTime.Parse(req.Query["fromUtc"]);
            var toTime = DateTime.Parse(req.Query["toUtc"]);

            var logs = await _logRepository.RetrieveRequestLogsAsync(fromTime, toTime);

            return new OkObjectResult(logs);
        }
    }
}
