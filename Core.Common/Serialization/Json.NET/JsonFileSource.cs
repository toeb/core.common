using Newtonsoft.Json;
using System.IO;

namespace Core.Values
{
  public class JsonFileSource<T> : AbstractFileSource<T>
  {
    JsonSerializer serializer;
    public JsonFileSource(WatchableFile file, JsonSerializer serializer)
      : base(file, new SourceInfo(true, typeof(T), false))
    {
      this.serializer = serializer ?? new JsonSerializer() { Formatting = Formatting.Indented };
    }

    protected override T TypedReadFile(Stream stream)
    {
      using (var reader = new JsonTextReader(new StreamReader(stream)))
      {
        return serializer.Deserialize<T>(reader);
      }
    }
  }
}
