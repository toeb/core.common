using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Core.Common.Collections
{
  public static class TransformingCollection
  {
    public static ICollection<TOuter> Transform<TInner, TOuter>(this ICollection<TInner> inner, Func<TInner, TOuter> inner2outer, Func<TOuter, TInner> outer2inner)
      where TInner : class
      where TOuter : class
    {
      return new TransformingCollection<TOuter, TInner>(inner, outer2inner, inner2outer);
    }
  }
  public class TransformingCollection<TOuter, TInner> : ICollection<TOuter>, INotifyCollectionChanged, INotifyPropertyChanged where TOuter:class where TInner :class
  {
    private ICollection<TInner> InnerCollection;
    private Dictionary<object, object> cache = new Dictionary<object, object>();
    public TransformingCollection(
      ICollection<TInner> inner,
      Func<TOuter,TInner> outer2inner,
      Func<TInner,TOuter> inner2outer     
      
      )
    {
      this.outer2inner = outer2inner;
      this.inner2outer = inner2outer;
      this.InnerCollection = inner;
      var collectionChanged = inner as INotifyCollectionChanged;
      var propertyChanged = inner as INotifyPropertyChanged;
      if(collectionChanged!=null)collectionChanged.CollectionChanged += OnCollectionChanged;
      if(propertyChanged!=null)propertyChanged.PropertyChanged += OnPropertyChanged;
  
    }
  
    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (PropertyChanged != null) PropertyChanged(this, e);
    }
  
    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (CollectionChanged == null) return;
      var action = e.Action;
      var newItems = e.NewItems == null ? null : e.NewItems.Cast<TInner>().Select(it => MapInnerToOuter(it)).ToList();
      var startingIndex = e.NewStartingIndex;
      var oldItems = e.OldItems==null?null: e.OldItems.Cast<TInner>().Select(it => MapInnerToOuter(it)).ToList();
      var oldStartingIndex = e.OldStartingIndex;
      NotifyCollectionChangedEventArgs args;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          args = new NotifyCollectionChangedEventArgs(action, newItems);
          break;
        case NotifyCollectionChangedAction.Move:
          args = new NotifyCollectionChangedEventArgs(action, newItems, startingIndex,oldStartingIndex);
          break;
        case NotifyCollectionChangedAction.Remove:
          args = new NotifyCollectionChangedEventArgs(action, oldItems, oldStartingIndex);
          break;
        case NotifyCollectionChangedAction.Replace:
          args = new NotifyCollectionChangedEventArgs(action, newItems, oldItems, startingIndex);
          break;
        case NotifyCollectionChangedAction.Reset:
          args = new NotifyCollectionChangedEventArgs(action);
          break;
        default: throw new NotImplementedException();
      }
  
      CollectionChanged(this, args);
    }
  
  
    private TInner MapOuterToInner(TOuter outer)
    {
      if (!cache.ContainsKey(outer))
      {
        cache[outer] = outer2inner(outer);
      }
      return cache[outer] as TInner;
    }
  
    private Func<TOuter, TInner> outer2inner;
    private Func<TInner, TOuter> inner2outer;
  
  
    private TOuter MapInnerToOuter(TInner inner)
    {
  
      if (!cache.ContainsKey(inner))
      {
        cache[inner] = inner2outer(inner);
      }
      return cache[inner] as TOuter;
    }
  
  
    public event NotifyCollectionChangedEventHandler CollectionChanged;
  
    public event PropertyChangedEventHandler PropertyChanged;
  
    public void Add(TOuter item)
    {
      InnerCollection.Add(MapOuterToInner(item));
    }
  
    public void Clear()
    {
      InnerCollection.Clear();
    }
  
    public bool Contains(TOuter item)
    {
      return InnerCollection.Contains(MapOuterToInner(item));
    }
  
    public void CopyTo(TOuter[] array, int arrayIndex)
    {
      
      foreach(var outer in this){
        array[arrayIndex++] = outer;  
      }
    }
  
    public int Count
    {
      get { return InnerCollection.Count; }
    }
  
    public bool IsReadOnly
    {
      get { return InnerCollection.IsReadOnly; }
    }
  
    public bool Remove(TOuter item)
    {
      return InnerCollection.Remove(MapOuterToInner(item));
    }
  
    public IEnumerator<TOuter> GetEnumerator()
    {
      foreach (var inner in InnerCollection)
      {
        yield return MapInnerToOuter(inner);
      }
    }
  
  
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
  
    }
  }
}
