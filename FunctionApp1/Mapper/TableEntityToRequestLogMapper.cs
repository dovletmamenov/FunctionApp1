using FunctionApp1.Models.API;

namespace FunctionApp1.Mapper
{
    public static class TableEntityToRequestLogMapper
    {
        public static RequestLog ToRequestLog(this Azure.Data.Tables.TableEntity tableEntity)
        {
            var requestLog = new RequestLog
            {
                Time = tableEntity.Timestamp.Value.UtcDateTime,
                ResponseCode = tableEntity.GetString("ResponseCode"),
            };

            return requestLog;
        }
    }
}
