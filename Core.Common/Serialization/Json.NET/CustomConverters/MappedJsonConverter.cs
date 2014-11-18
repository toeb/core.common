using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using Core;
using Core.Annotations;
namespace Core.Configuration
{

  /// <summary>
  /// converts types to json objects including extra fields which can be specified via the extradata attributes
  /// </summary>
  public class MappedConverter : JsonConverter
  {
    public virtual bool UseProperty(PropertyInfo info) { return true; }
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

      var mappedObj = existingValue ?? System.Activator.CreateInstance(objectType);
      //get an array of the object's props so I can check if the JSON prop s/b mapped to it
      var properties = objectType.GetProperties().Where(UseProperty);

      var setter = CreateSetter(objectType);
      
      var computedProperties= new List<PropertyInfo>();

      //loop through my JSON string
      while (reader.Read())
      {
        //if I'm at a property...
        if (reader.TokenType == JsonToken.PropertyName)
        {
          //convert the property to lower case
          string propertyName = reader.Value.ToString();
          //is this a mapped prop?
          reader.Read();
          if (properties.Any(prop=>prop.Name==propertyName))
          {
            //get the property info and set the Mapped object's property value
            PropertyInfo pi = mappedObj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var value = serializer.Deserialize(reader, pi.PropertyType);
            pi.SetValue(mappedObj, value, null);
            computedProperties.Add(pi);
          }
          else
          {
            //otherwise, stuff it into the Dictionary
            var value = serializer.Deserialize(reader);
            setter.Set(mappedObj, propertyName, value);

          }

        }
      }

      var uncomputedProperties = properties.Except(computedProperties);
      HandleUncomputedProperties(uncomputedProperties, mappedObj);
      return mappedObj;
    }

    protected virtual void HandleUncomputedProperties(IEnumerable<PropertyInfo> uncomputedProperties, object obj)
    {
      
    }



    interface Setter
    {
      void Set(object target, string name, object value);
    }
    class PropSetter : Setter
    {
      public PropSetter(PropertyInfo info)
      {
        this.info = info;
      }
      public void Set(object target, string name, object value)
      {

        var dict = info.GetValue(target) as ICollection<KeyValuePair<string, object>>;
        bool createdDict = false;
        if (dict == null && info.CanWrite)
        {
          dict = new Dictionary<string, object>();
          createdDict = true;
        }

        dict.Add(new KeyValuePair<string, object>(name, value));

        if (createdDict)
        {
          info.SetValue(target, dict);
        }




      }

      public PropertyInfo info { get; set; }
    }
    class ClassSetter : Setter
    {
      public ClassSetter(Type type)
      {
        this.type = type;
      }

      public Type type { get; set; }

      public void Set(object target, string name, object value)
      {
        var col = target as ICollection<KeyValuePair<string, object>>;
        if (col == null) return;
        col.Add(new KeyValuePair<string, object>(name, value));

      }
    }
    class MethodSetter : Setter
    {
      private MethodInfo info;

      public MethodSetter(MethodInfo info)
      {
        this.info = info;
      }

      public void Set(object target, string name, object value)
      {
        var param = new object[] { new KeyValuePair<string, object>(name, value) };
        info.Invoke(target, param);
      }


    }
    class CompositeSetter : Setter
    {
      public ICollection<Setter> Setters = new List<Setter>();

      public void Set(object target, string name, object value)
      {
        foreach (var setter in Setters) setter.Set(target, name, value);
      }
    }

    Type keyValueCollectionType = typeof(ICollection<KeyValuePair<string, object>>);
    Type keyValueType = typeof(KeyValuePair<string, object>);

    Setter CreateSetter(Type objectType)
    {
      var composite = new CompositeSetter();

      foreach (var property in GetProperties(objectType))
      {
        var setter = new PropSetter(property);
        composite.Setters.Add(setter);
      }
      foreach (var method in GetMethods(objectType))
      {
        var setter = new MethodSetter(method);
        composite.Setters.Add(setter);
      }
      if (objectType.HasAttribute<ExtraDataAttribute>() && keyValueCollectionType.IsAssignableFrom(objectType))
      {
        var setter = new ClassSetter(objectType);
        composite.Setters.Add(setter);
      }
      return composite;
    }
    IEnumerable<MethodInfo> GetMethods(Type objectType)
    {
      return objectType.MethodsWith<ExtraDataAttribute>().Where(meth => meth.Item1.GetParameters().CountIs(1) && meth.Item1.GetParameters().Single().ParameterType.IsAssignableFrom(keyValueType)).Select(t => t.Item1);
    }
    IEnumerable<PropertyInfo> GetProperties(Type objectType)
    {
      return objectType.PropertiesWith<ExtraDataAttribute>().Where(prop => keyValueCollectionType.IsAssignableFrom(prop.Item1.PropertyType) && prop.Item1.CanRead).Select(t => t.Item1);
    }
    public override bool CanConvert(Type objectType)
    {
      return
        GetProperties(objectType).Any() ||
        objectType.HasAttribute<ExtraDataAttribute>() && keyValueCollectionType.IsAssignableFrom(objectType) ||
        GetMethods(objectType).Any();
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

    public virtual void WriteProperty(JsonWriter writer, PropertyInfo property, object value, JsonSerializer serializer)
    {
      Annotate(writer, property, value, serializer);
      writer.WritePropertyName(property.Name);
      serializer.Serialize(writer, value);
    }

    protected virtual void Annotate(JsonWriter writer, PropertyInfo property, object value, JsonSerializer serializer)
    {

    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      var type = value.GetType();
      writer.WriteStartObject();

      var properties =  type.GetProperties().Except(GetProperties(type)).Where(UseProperty);
      foreach (var property in properties)
      {

        if (!property.CanRead) continue;
        var propertyValue = property.GetValue(value);
        WriteProperty(writer, property, propertyValue,serializer);
      }

      var pairs = new List<KeyValuePair<string, object>>();

      if (keyValueCollectionType.IsAssignableFrom(type) && type.HasAttribute<ExtraDataAttribute>())
      {
        var thisCast = value as IEnumerable<KeyValuePair<string, object>>;
        pairs.AddRange(thisCast);
      }


      var props = GetProperties(type);
      foreach (var prop in props)
      {
        if (!prop.CanRead) continue;
        var vals = prop.GetValue(value) as IEnumerable<KeyValuePair<string, object>>;
        if (vals == null) continue;
        pairs.AddRange(vals);
      }



      var unique = pairs.Distinct((left, right) => left.Key == right.Key);

      foreach (var it in unique)
      {
        writer.WritePropertyName(it.Key);
        serializer.Serialize(writer, it.Value);
        //if (writer.WriteState == WriteState.Start) writer.WriteRaw(",");

      }
      writer.WriteEndObject();
    }
  }

}
