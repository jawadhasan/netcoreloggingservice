using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using NATS.Client;

namespace Common
{
    public class NatsConnectionHelper
    {

        public static IEncodedConnection BuildConnection()
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            //opts.Url = _Options.Value.MessageBrokerUrl;
            opts.Url = LogConfiguration.Current.MessageBrokerUrl;
            var connection = new ConnectionFactory().CreateEncodedConnection();
            connection.OnDeserialize = JsonHelpers.Deserialize<LogDetail>;
            return connection;
        }
    }
}
