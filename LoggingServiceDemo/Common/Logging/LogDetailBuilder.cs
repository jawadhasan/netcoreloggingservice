using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging
{
    public static class LogDetailBuilder
    {
        public static LogDetail Create(
            LogTypes logType,
            string product, 
            string layer, 
            string location,
            string activityName, 
            string correlationId = null, 
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new LogDetail()
            {
                LogType = logType,
                Product = product,
                Layer = layer,
                Location = location,
                Message = activityName,
                Hostname = Environment.MachineName,
                CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid().ToString("N") : correlationId,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>()
            };
            return detail;
        }
    }
}
