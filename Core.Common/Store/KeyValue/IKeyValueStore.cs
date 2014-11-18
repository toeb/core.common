using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  

  public enum Change{
    Modified,
    Deleted,
    Added
  }
  public delegate void ChangedDelegate<TKey, TValue>(IKeyValueStore<TKey, TValue> sender, TKey id, Change change);

  public interface IKeyValueStore<TKey, TValue>
  {
    IQueryable<TKey> Keys { get; }
    IQueryable<TValue> Values { get; }
    bool ContainsKey(TKey key);
    TKey Store(TKey key, TValue value);
    void Delete(TKey key);
    TValue Load(TKey key);
    event ChangedDelegate<TKey,TValue> ValueChanged;  
  }

}
