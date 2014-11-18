using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Core.Collections
{
  /**
   * <summary> Observablecollection which uses the CoreDispatcher.Dispath 
   * 					 method to modify its items, which is important when using 
   * 					 multithreading.</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="T"> Generic type parameter.</typeparam>
   */
  public class DispatchingObservableCollection<T> : ObservableCollection<T>
  {
    public DispatchingObservableCollection()
    {
    }
    protected override void MoveItem(int oldIndex, int newIndex)
    {
      lock (this)
      {
        CoreDispatcher.Dispatch(() => base.MoveItem(oldIndex, newIndex));
      }
    }
    protected override void SetItem(int index, T item)
    {
      lock (this)
      {
        CoreDispatcher.Dispatch(() => base.SetItem(index, item));
      }
    }
    protected override void ClearItems()
    {
      lock (this)
      {
        CoreDispatcher.Dispatch(() => base.ClearItems());
      }
    }
    protected override void InsertItem(int index, T item)
    {
      lock (this)
      {
        CoreDispatcher.Dispatch(() => base.InsertItem(index, item));
      }
    }
    protected override void RemoveItem(int index)
    {
      lock (this)
      {
        CoreDispatcher.Dispatch(() => base.RemoveItem(index));
      }
    }

  }
}
