using Core.Identification;
using Core.Store.KeyValue.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  public static class KeyValueStore
  {
    public static IKeyValueStore<TKey, TValue> Memory<TKey, TValue>()
      where TValue : IIdentityAssignable<TKey>
    {
      return new CloningMemoryKeyValueStore<TKey, TValue>();
    }
  }
}
