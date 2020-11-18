using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Common.Logging
{
    public class PerfTracker
    {
        private readonly Stopwatch _sw;
        private readonly LogDetail _infoToLog;

        public PerfTracker(LogDetail details)
        {
            _sw = Stopwatch.StartNew();
            _infoToLog = details;
            _infoToLog.LogType = LogTypes.Performance;

            var beginTime = DateTime.Now;
            if (_infoToLog.AdditionalInfo == null)
            {
                _infoToLog.AdditionalInfo = new Dictionary<string, object>
                {
                    {"Started", beginTime.ToString(CultureInfo.InvariantCulture) }
                };
            }
            else
            {
                _infoToLog.AdditionalInfo.Add("Started", beginTime.ToString(CultureInfo.InvariantCulture));
            }
        }

        public void Stop()
        {
            _sw.Stop();
            _infoToLog.ElapsedMilliseconds = _sw.ElapsedMilliseconds;
            LogMsgPublisher.PublishMessage(_infoToLog);
        }

    }
}
