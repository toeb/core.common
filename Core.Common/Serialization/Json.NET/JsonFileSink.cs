using Newtonsoft.Json;
using System.IO;

namespace Core.Values
{

  public class JsonFileSink<T> : AbstractFileSink<T>
  {
    JsonSerializer serializer;
    public JsonFileSink(string filePath, JsonSerializer serializer)
      : base(filePath)
    {
      this.serializer = serializer ?? new JsonSerializer() { Formatting = Formatting.Indented };
    }
    protected override void TypedFileWrite(Stream stream, T value)
    {
      var writer = new JsonTextWriter(new StreamWriter(stream));
      serializer.Serialize(writer, value);
      writer.Flush();
    }
  }
}
