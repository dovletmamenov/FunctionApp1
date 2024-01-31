using System;
using System.Threading.Tasks;

namespace FunctionApp1.Interfaces
{
    public interface IBlobStorage
    {
        Task StoreResponseContentAsync(DateTime requestTime, string responseContent);
        Task<string> RetrieveResponseContentAsync(DateTime requestTime);
    }
}
