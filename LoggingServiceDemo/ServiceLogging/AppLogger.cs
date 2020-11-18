using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using Serilog;
using Serilog.Events;

namespace ServiceLogging
{
    public class AppLogger
    {
        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;

        static AppLogger()
        {

            _perfLogger = new LoggerConfiguration()
                .WriteTo.Elasticsearch("http://localhost:9200",
                    indexFormat: "hcssperf-{0:yyyy.MM.dd}",
                    inlineFields: true)
                .CreateLogger();

            _usageLogger = new LoggerConfiguration()
                 .WriteTo.Elasticsearch("http://localhost:9200",
                     indexFormat: "hcssusage-{0:yyyy.MM.dd}",
                     inlineFields: true)
                 .CreateLogger();

            _errorLogger = new LoggerConfiguration()
                 .WriteTo.Elasticsearch("http://localhost:9200",
                     indexFormat: "hcsserror-{0:yyyy.MM.dd}",
                     inlineFields: true)
                 .CreateLogger();

            _diagnosticLogger = new LoggerConfiguration()
                 .WriteTo.Elasticsearch("http://localhost:9200",
                     indexFormat: "hcssdiag-{0:yyyy.MM.dd}",
                     inlineFields: true)
                .CreateLogger();
        }

        public static void WritePerf(LogDetail infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }

        public static void WriteUsage(LogDetail infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }
        public static void WriteError(LogDetail infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            _errorLogger.Write(LogEventLevel.Error, "{@LogDetail}", infoToLog);

        }
        public static void WriteDiagnostic(LogDetail infoToLog)
        {
            //var writeDiagnostics =
            //    Convert.ToBoolean(Environment.GetEnvironmentVariable("DIAGNOSTICS_ON"));
            //if (!writeDiagnostics)
            //    return;

            _diagnosticLogger.Write(LogEventLevel.Information, "{@LogDetail}", infoToLog);
        }

        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);

            return ex.Message;
        }
        private static string FindProcName(Exception ex)
        {
            //var sqlEx = ex as SqlException;
            //if (sqlEx != null)
            //{
            //    var procName = sqlEx.Procedure;
            //    if (!string.IsNullOrEmpty(procName))
            //        return procName;
            //}

            //if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
            //{
            //    return (string)ex.Data["Procedure"];
            //}

            if (ex.InnerException != null)
                return FindProcName(ex.InnerException);

            return null;
        }
    }
}
