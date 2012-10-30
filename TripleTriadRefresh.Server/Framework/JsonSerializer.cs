using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SignalR;
using System;
using System.Collections.Generic;
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
   
    public string Stringify(object obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }
  
    public object Parse(string json)
    {
        return JsonConvert.DeserializeObject(json);
    }
 
    public object Parse(string json, Type targetType)
    {
        return JsonConvert.DeserializeObject(json, targetType);
    }

    public T Parse<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
}
