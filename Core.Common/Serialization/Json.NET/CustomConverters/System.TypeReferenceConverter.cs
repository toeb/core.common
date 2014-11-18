using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
namespace Core.Serialization.Json
{
  [Export]
  [Priority(5)]
  [Export(typeof(JsonConverter))]
  public class TypeReferenceConverter : TypedJsonConverterBase<Type>
  {
    static TypeReferenceConverter instance;
    public static TypeReferenceConverter Instance
    {
      get
      {
        return instance ?? (instance = new TypeReferenceConverter() { ReflectionService = Core.ReflectionService.Instance });
      }
    }

    public override bool CanRead
    {
      get
      {
        return true;
      }
    }
    public override bool CanWrite
    {
      get
      {
        return true;
      }
    }

    protected override Type Read(JsonReader reader, Type objectType, JsonSerializer serializer)
    {
      JObject obj = JObject.Load(reader);

      var id = obj.Value<string>("Id");
      Guid result;
      if (Guid.TryParse(id, out result))
      {
        var type = ReflectionService.GetById(result);

        return type;
      }      
      return null;
    }

    protected override void Write(JsonWriter writer, Type type, JsonSerializer serializer)
    {

      writer.WriteStartObject();
      writer.WritePropertyName("Id");
      writer.WriteValue(type.GUID);
      writer.WriteEndObject();
    }
    [Import]
    public IReflectionService ReflectionService { get; set; }


  }
}
