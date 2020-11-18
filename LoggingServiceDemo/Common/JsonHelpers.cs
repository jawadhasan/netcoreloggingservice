using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Common
{
    public static class JsonHelpers
    {
        public static byte[] Serializer<T>(T obj) =>
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        public static T Deserialize<T>(byte[] data) =>
            JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
    }
}
