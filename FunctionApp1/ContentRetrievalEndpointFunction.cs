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
    public class ContentRetrievalEndpointFunction
    {
        private readonly ILogRepository _logRepository;

        public ContentRetrievalEndpointFunction(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        [FunctionName("ContentRetrievalEndpointFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var timeUtc = DateTime.Parse(req.Query["timeUtc"]);

            var logs = await _logRepository.RetrieveResponseContentAsync(timeUtc);

            return new OkObjectResult(logs);
        }
    }
}
