using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using NATS.Client;
using Newtonsoft.Json;

namespace Common
{

    public static class LogMsgPublisher
    {
        public static bool PublishMessage(LogDetail logDetail)
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = LogConfiguration.Current.MessageBrokerUrl;
            var commandsSubject = LogConfiguration.Current.LoggingCommandSubject;
            using (IEncodedConnection c = new ConnectionFactory().CreateEncodedConnection(opts))
            {
                c.OnSerialize = JsonSerializer;
                c.Publish(commandsSubject, logDetail);
                c.Flush();
            }
            return true;
        }

        private static byte[] JsonSerializer<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Encoding.UTF8.GetBytes(json);
        }
    }


}
