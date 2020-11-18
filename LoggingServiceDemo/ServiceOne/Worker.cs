using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServiceOne
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = $"Worker running at: {DateTimeOffset.Now}";
                _logger.LogInformation(msg);


                //Prepare and Publish LogDetail
                var logDetail = LogDetailBuilder.Create(LogTypes.Diagnostic, "ServiceOne", nameof(Worker), nameof(ExecuteAsync), msg);

                //ToAddException
                //logDetail.Exception = ex;

                LogMsgPublisher.PublishMessage(logDetail);


                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
