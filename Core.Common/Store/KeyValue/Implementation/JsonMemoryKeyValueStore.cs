using Core.Serialization.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  /// <summary>
  /// A memory based key value storage. it uses json to serialize objects into a memory
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  [Export]
  [Export(typeof(IKeyValueStore<,>))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class JsonMemoryKeyValueStore<TKey, TValue> : MemoryKeyValueStore<TKey, TValue>
  {
    [Import]
    TypeReferenceConverter TypeReferenceConverter { get; set; }

    class Entry
    {
      public Type Type { get; set; }
      public string Value { get; set; }
    }

    public override object Pack(TValue value)
    {
      Type type = typeof(TValue);
      if (value != null)
      {
        type = value.GetType();
      }
      var result = JsonConvert.SerializeObject(value);
      return new Entry() { Type = type, Value = result };
    }

    public override TValue Unpack(object packedObject)
    {
      var entry = packedObject as Entry;
      var result =(TValue) JsonConvert.DeserializeObject(entry.Value, entry.Type);

      return result;
    }
  }

}
