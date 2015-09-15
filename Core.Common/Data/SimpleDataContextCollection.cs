using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Core.Common.Data
{
  public class SimpleDataContextCollection<T> : ObservableCollection<T>, IContainer
  {
    private SimpleDataContext context;
    private IDispatcher dispatcher;
    public SimpleDataContextCollection(IDispatcher dispatcher, SimpleDataContext context)
    {
      this.dispatcher = dispatcher;
      this.context = context;
    }
  
  
    protected override void MoveItem(int oldIndex, int newIndex)
    {
      dispatcher.Dispatch(() => base.MoveItem(oldIndex, newIndex));
    }
    protected override void SetItem(int index, T item)
    {
      dispatcher.Dispatch(() => base.SetItem(index, item));
    }
    protected override void ClearItems()
    {
      dispatcher.Dispatch(() => base.ClearItems());
    }
    HashSet<T> set = new HashSet<T>();
    protected override void InsertItem(int index, T item)
    {
      if (Contains(item)) return;
      dispatcher.Dispatch(() =>
      {
        set.Add(item);
        base.InsertItem(index, item);
      });
      if (context.Entry(item).State == EntityState.Detached) context.Entry(item).State = EntityState.Attached;
     
      Trace.WriteLine("item added to " + typeof(T) + " collection");
  
    }
    protected override void RemoveItem(int index)
    {
      dispatcher.Dispatch(() =>
      {
        var item = Items[index];
        set.Remove(item);
        context.Entry(item).State = EntityState.Deleted;
        base.RemoveItem(index);
        Trace.WriteLine("item removed from " + typeof(T) + " collection");
      });
    }





    public bool Add(object item)
    {
      if (Contains(item)) return false;
      base.Add((T)item);
      return true;
    }

    public bool Remove(object item)
    {
      return base.Remove((T)item);
    }

    public bool Contains(object item)
    {
      return set.Contains((T)item);
    }
  }
}
