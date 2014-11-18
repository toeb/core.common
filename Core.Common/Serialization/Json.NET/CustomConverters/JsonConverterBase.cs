using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Json
{
  public abstract class JsonConverterBase : JsonConverter
  {
    protected abstract bool CanConvertSingle(Type objectType);
    protected abstract object ReadJsonSingle(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
    protected abstract void WriteJsonSingle(JsonWriter writer, object value, JsonSerializer serializer);

    public override bool CanConvert(Type objectType)
    {
      if (CanConvertSingle(objectType)) return true;

      var enumerable = objectType.GetInterface("IEnumerable`1");
      var queryable = objectType.GetInterface("IQueryable`1");
      if(enumerable == null && queryable==null)return false;
      if(CanConvertSingle(enumerable.GetGenericArguments()[0]))return true;
      return false;

    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (CanConvertSingle(objectType) && CanRead)
      {
        return ReadJsonSingle(reader, objectType, existingValue, serializer);
      }
      else
      {
        if (objectType.Name.StartsWith("IEnumerable"))
        {

        }

      }

      

      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      serializer.Formatting = Formatting.Indented;
      if (CanConvertSingle(value.GetType()) && CanWrite)
      {
        WriteJsonSingle(writer,value,serializer);
        return;
      }

      writer.WriteStartArray();
      var enumerable = value as IEnumerable;
      foreach(var item in enumerable){        
        WriteJsonSingle(writer,item,serializer);
      }
      writer.WriteEndArray();
    }
  }
}
