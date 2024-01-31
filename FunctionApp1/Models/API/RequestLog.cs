using System;

namespace FunctionApp1.Models.API
{
    public class RequestLog
    {
        public DateTime Time { get; set; }
        public string RequestUri { get; set; }
        public string Method { get; set; }
        public string ResponseCode { get; set; }
        public string ResponsePayload { get; set; }
    }
}
