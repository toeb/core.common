using Core.Converters;
using Core.FileSystem;
using Core.Serialization.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  [Export]
  public class JsonFileKeyValueStore<TKey, TValue> : FileKeyValueStore<TKey, TValue>
  {
    class Entry
    {
      public TValue Value { get; set; }
      public Type Type { get; set; }
    }
    protected override string FileExtension
    {
      get { return "json"; }
    }
    [Import]
    TypeReferenceConverter TypeConverter
    {
      set;
      get;
    }
    public override TValue ReadFileEntry(Stream stream, TKey key)
    {
      var reader = new JsonTextReader(new StreamReader(stream));
      var result = Serializer.Deserialize<Entry>(reader);
      return result.Value;
    }

    public override void WriteFileEntry(Stream stream, TKey key, TValue value)
    {
      using (var writer = new StreamWriter(stream))
      {
        object objectValue = value;
        var type = objectValue == null ? typeof(TValue) : value.GetType();
        var entry = new Entry() { Value = value, Type = type };
        Serializer.Serialize(writer, entry);
      }

    }
    JsonSerializer serializer = null;
    public JsonSerializer Serializer
    {
      get
      {
        if (serializer == null)
        {
          serializer = new JsonSerializer();
          serializer.Converters.Add(TypeConverter);
        }
        return serializer;
      }
    }
  }
}
