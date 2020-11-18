using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client;

namespace ServiceLogging
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        //NATS Items
        private IAsyncSubscription _subscription;
        private readonly IEncodedConnection _connection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _connection = NatsConnectionHelper.BuildConnection();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _subscription = _connection.SubscribeAsync(LogConfiguration.Current.LoggingCommandSubject, eventHandler);
            _logger.LogInformation("waiting for log events...");
        }


        private EventHandler<EncodedMessageEventArgs> eventHandler = async (sender, args) =>
        {
            var logDetail = (LogDetail)args.ReceivedObject;

            try
            {
               Console.WriteLine($"LogDetail received [{logDetail.LogType}] [{logDetail.Product}] {logDetail.Message}");

               switch (logDetail.LogType)
               {
                   case LogTypes.Performance:
                       AppLogger.WritePerf(logDetail);//persistence
                       break;
                   case LogTypes.Usage:
                       AppLogger.WriteUsage(logDetail);
                       break;
                   case LogTypes.Error:
                       AppLogger.WriteError(logDetail);
                       break;
                   case LogTypes.Diagnostic:
                       AppLogger.WriteDiagnostic(logDetail);
                       break;
                   default:
                       AppLogger.WriteDiagnostic(logDetail);
                       break;
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }

        };

    }
}

