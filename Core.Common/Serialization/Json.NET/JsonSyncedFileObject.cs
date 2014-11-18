using Core.ManagedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Serialization.Values
{

  public class JsonSyncedFileObject<T> : ReflectedManagedNotifyingObject, IDisposable
  {
    public string Path { get { return jf.Path; } set { jf.Path = value; } }
    JsonFileObjectValue<T> jf = new JsonFileObjectValue<T>();
    public JsonSyncedFileObject(T val)
      : base(val, typeof(T).GetProperties())
    {
      jf.ValueChanged += FileValueChanged;
    }


    protected override void OnPropertyAdded(IManagedProperty property)
    {
      property.ValueChanged += property_ValueChanged;
    }
    protected override void OnPropertyRemoved(IManagedProperty property)
    {
      property.ValueChanged -= property_ValueChanged;
    }

    void property_ValueChanged(object sender, ValueChangeEventArgs args)
    {
      Sync(Instance);
      WaitForPendingChanges();
    }

    object monitor = new object();
    private void Sync(object val)
    {

      bool taken = false;
      Monitor.TryEnter(monitor, 0, ref taken);
      if (!taken) return;

      if (val == Instance)
      {
        jf.Consume(val);
      }
      else
      {
        Push(new ReflectedManagedObject(val, val.GetType().GetProperties()), ManagedObjectMergeStrategies.Default);
      }

      Monitor.Exit(monitor);
    }

    public void WaitForPendingChanges()
    {
      Monitor.Enter(monitor);
      Monitor.Exit(monitor);

    }
    private void FileValueChanged(object sender, ValueChangeEventArgs args)
    {
      Sync(jf.Value);
    }


    public void Dispose()
    {

      jf.Dispose();
    }
  }
}
