using System;
using System.Net.Http;
using System.Threading.Tasks;
using FunctionApp1.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly HttpClient _httpClient;
        private readonly ILogRepository _logRepository;

        public Function1(IHttpClientFactory httpClientFactory, ILogRepository logRepository)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logRepository = logRepository;
        }

        [FunctionName("FetchApiFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, ILogger log)
        {
            var currentTime = DateTime.UtcNow;
            var response = await _httpClient.GetAsync("");
            string content = await response.Content.ReadAsStringAsync();


            await _logRepository.StoreRequestResultAsync(currentTime, response, content);
        }
    }
}
