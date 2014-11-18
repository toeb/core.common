
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Serialization.Json
{
  public abstract class TypedJsonConverterBase<T> : JsonConverterBase
  {

    protected override bool CanConvertSingle(Type objectType)
    {
      var type = typeof(T);
      return type.IsAssignableFrom(objectType);
    }

    protected override object ReadJsonSingle(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
      if (existingValue != null) throw new NotImplementedException();
      return Read(reader, objectType, serializer);
    }

    protected override void WriteJsonSingle(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {
      Write(writer, (T)value, serializer);
    }

    protected abstract T Read(Newtonsoft.Json.JsonReader reader, Type objectType, Newtonsoft.Json.JsonSerializer serializer);
    protected abstract void Write(Newtonsoft.Json.JsonWriter writer, T value, Newtonsoft.Json.JsonSerializer serializer);

  }
}
