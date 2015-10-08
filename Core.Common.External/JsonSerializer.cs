using Newtonsoft.Json;
using System.IO;

namespace Core.Common.Crypto
{
  public class NewtonsoftJsonSerializer : ISerializer
  {
    JsonSerializer serializer;
    private JsonSerializer Serializer
    {
      get
      {
        if (serializer != null) return serializer;
        serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.Objects;
        serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        return serializer;
      }
    }

    public object Deserialize(StringReader reader)
    {
      return serializer.Deserialize(new JsonTextReader(reader));
    }

    public void Serialize(StringWriter writer, object value)
    {
      serializer.Serialize(new JsonTextWriter(writer), value);
    }

  }
}