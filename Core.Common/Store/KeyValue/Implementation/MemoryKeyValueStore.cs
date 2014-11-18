using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  public abstract class MemoryKeyValueStore<TKey, TValue> : IKeyValueStore<TKey, TValue>
  {
    /// <summary>
    /// stores the key value pairs
    /// </summary>
    private IDictionary<TKey, object> data = new Dictionary<TKey, object>();


    public virtual TKey Store(TKey key, TValue value)
    {
      data[key] = Pack(key, value);
      OnStore(key, value);
      return key;
    }

    public virtual TValue Load(TKey key)
    {
      if (!ContainsKey(key)) throw new KeyNotFoundException();
      var result = Unpack(key, data[key]);
      OnLoad(key, result);
      return result;
    }

    public virtual void Delete(TKey key)
    {
      data.Remove(key);
      OnDelete(key);
    }
    protected void RegisterEntry(TKey key, object packedValue){
      if(ContainsKey(key))return;
      data[key] = packedValue;
    }

    protected virtual void OnLoad(TKey key,TValue value) { }
    protected virtual void OnStore(TKey key, TValue value) { }
    protected virtual void OnDelete(TKey key) { }

    public virtual object Pack(TKey id, TValue value) { return Pack(value); }
    public virtual TValue Unpack(TKey id, object value) { return Unpack(value); }

    public abstract object Pack(TValue value);
    public abstract TValue Unpack(object packedObject);

    protected void RaiseValueChanged(TKey id, Change change) { if (ValueChanged != null)ValueChanged(this,id,change); }
    

    public virtual bool ContainsKey(TKey key)
    {
      return data.ContainsKey(key);
    }

    public virtual IQueryable<TKey> Keys
    {
      get { return data.Keys.AsQueryable(); }
    }
    public virtual IQueryable<TValue> Values
    {
      get
      {
        return data.Select(value => Unpack(value.Key,value.Value)).AsQueryable();
      }
    }




    public event ChangedDelegate<TKey,TValue> ValueChanged;
  }


}
