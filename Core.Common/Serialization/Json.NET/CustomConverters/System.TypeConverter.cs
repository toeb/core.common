using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
namespace Core.Serialization.Json
{

  /// <summary>
  /// Converts System.Type from and to  json
  /// </summary>
  [Export]
  [Priority(5)]
  [Export(typeof(JsonConverter))]  
  public class TypeConverter : TypedJsonConverterBase<Type>
  {
    static TypeConverter instance;
    public static TypeConverter Instance
    {
      get
      {
        return instance ??( instance = new TypeConverter(Core.ReflectionService.Instance) );
      }
    }
    [ImportingConstructor]
    public TypeConverter([Import] IReflectionService reflection)
    {
      this.ReflectionService = reflection;
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
      var fullName = obj.Value<string>("FullName");

      Guid result;
      if (!Guid.TryParse(id, out result))
      {
        var type = ReflectionService.GetById(result);

        return type;
      }
      var t = ReflectionService.AllTypes.GetByFullName(fullName);
      return t;
    }

    protected override void Write(JsonWriter writer, Type value, JsonSerializer serializer)
    {
      var type = value as Type;
      writer.WriteStartObject();
      writer.WritePropertyName("Id");
      writer.WriteValue(type.GUID);
      writer.WritePropertyName("FullName");
      writer.WriteValue(type.FullName);
      writer.WritePropertyName("Name");
      writer.WriteValue(type.Name);
      writer.WritePropertyName("BaseType");
      if (type.BaseType != null)
      {
        writer.WriteStartObject();
        writer.WritePropertyName("Id");
        writer.WriteValue(type.BaseType.GUID);
        writer.WritePropertyName("FullName");
        writer.WriteValue(type.BaseType.FullName);
        writer.WriteEndObject();
      }
      else
      {
        writer.WriteNull();
      }
      writer.WritePropertyName("Properties");
      writer.WriteStartArray();
      // throw new NotImplementedException();
      /*foreach (var propertyInfo in type.PropertiesWith<ItemPropertyAttribute>())
      {
        writer.WriteStartObject();
        writer.WritePropertyName("PropertyName");
        writer.WriteValue(propertyInfo.Item1.Name);
        writer.WritePropertyName("PropertyType");
        writer.WriteStartObject();
        writer.WritePropertyName("Id");
        writer.WriteValue(propertyInfo.Item1.PropertyType.GUID);
        writer.WritePropertyName("FullName");
        writer.WriteValue(propertyInfo.Item1.PropertyType.FullName);
        writer.WriteEndObject();
        writer.WriteEndObject();
      }*/
      writer.WriteEndArray();

      writer.WriteEndObject();



    }

    public IReflectionService ReflectionService { get; set; }


  }
}
