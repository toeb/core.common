using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue.Implementation
{

  public class CloningMemoryKeyValueStore<TKey, TValue> : MemoryKeyValueStore<TKey, TValue>
  {

    public override object Pack(TValue value)
    {
      return value.DeepClone();
    }

    public override TValue Unpack(object packedObject)
    {
      return ((TValue)packedObject).DeepClone();
    }
  }
}
