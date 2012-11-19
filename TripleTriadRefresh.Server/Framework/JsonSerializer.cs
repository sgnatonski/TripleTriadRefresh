using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TripleTriadRefresh.Server.Framework
{
    public class CamelCaseJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public CamelCaseJsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }
 
        public object Parse(string json, Type targetType)
        {
            return JsonConvert.DeserializeObject(json, targetType);
        }

        public void Serialize(object value, TextWriter writer)
        {
            writer.Write(JsonConvert.SerializeObject(value, _settings));
        }
    }
}
