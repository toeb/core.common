using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Core.Common;

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
    public IDispatcher Dispatcher { get; private set; }

    public DispatchingObservableCollection(IDispatcher dispatcher)
    {
      this.Dispatcher = dispatcher;
    }
    protected override void MoveItem(int oldIndex, int newIndex)
    {
      lock (this)
      {
        Dispatcher.Dispatch(() => base.MoveItem(oldIndex, newIndex));
      }
    }
    protected override void SetItem(int index, T item)
    {
      lock (this)
      {
        Dispatcher.Dispatch(() => base.SetItem(index, item));
      }
    }
    protected override void ClearItems()
    {
      lock (this)
      {
        Dispatcher.Dispatch(() => base.ClearItems());
      }
    }
    protected override void InsertItem(int index, T item)
    {
      lock (this)
      {
        Dispatcher.Dispatch(() => base.InsertItem(index, item));
      }
    }
    protected override void RemoveItem(int index)
    {
      lock (this)
      {
        Dispatcher.Dispatch(() => base.RemoveItem(index));
      }
    }

  }
}
