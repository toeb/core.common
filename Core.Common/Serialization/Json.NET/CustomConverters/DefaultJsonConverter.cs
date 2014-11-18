
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Json
{

  class ReferenceResolver : IReferenceResolver
  {
    public void AddReference(object context, string reference, object value)
    {
      throw new NotImplementedException();
    }

    public string GetReference(object context, object value)
    {
      throw new NotImplementedException();
    }

    public bool IsReferenced(object context, object value)
    {
      throw new NotImplementedException();
    }

    public object ResolveReference(object context, string reference)
    {
      throw new NotImplementedException();
    }
  }
  class ContrReso : IContractResolver
  {

    public JsonContract ResolveContract(Type type)
    {
      throw new NotImplementedException();
    }
  }
  public class DefaultJsonConverter : JsonConverter
  {
    public DefaultJsonConverter()
    {

    }
    public override bool CanConvert(Type objectType)
    {
      return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return serializer.Deserialize(reader, objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      serializer.Serialize(writer, value);
    }
  }
}
