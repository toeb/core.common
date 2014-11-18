using Core.Values;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace Core.Serialization.Values
{
  public class JsonFileObjectValue<T> : FileObjectValue, IValue<T>
  {
    public JsonFileObjectValue()
      : base(new ValueInfo(true,true,typeof(T),false))
    {
     ser.Formatting = Formatting.Indented;
    }
    public new T Value
    {
      get
      {
        return (T)base.Value;
      }
      set
      {
        base.Value = value;
      }
    }

    JsonSerializer ser = new JsonSerializer();
    protected override object Deserialize(Stream stream)
    {
      using (var reader = new JsonTextReader(new StreamReader(stream)))
      {
        return ser.Deserialize<T>(reader);
      };
    }

    protected override void Serialize(Stream stream, object data)
    {
      using (var writer = new JsonTextWriter(new StreamWriter(stream)))
      {
        
        ser.Serialize(writer, data, typeof(T));
      }
    }

  }

  public class JsonFileObjectValue : FileObjectValue
  {
    public JsonFileObjectValue()
      : base(new ValueInfo(true, true, typeof(object), false))
    {

    }
    JsonSerializer ser = new JsonSerializer();
    protected override object Deserialize(Stream stream)
    {
      using (var reader = new JsonTextReader(new StreamReader(stream)))
      {
        return ser.Deserialize(reader);
      };
    }

    protected override void Serialize(Stream stream, object data)
    {
      using (var writer = new JsonTextWriter(new StreamWriter(stream)))
      {
        ser.Serialize(writer, data);
      }
    }
  }
}
