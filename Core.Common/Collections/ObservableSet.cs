using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  
  public class ObservableSet<T, TSet> : ISet<T>, INotifyCollectionChanged, INotifyPropertyChanged where TSet:ISet<T>,new()
  {
    private ISet<T> innerSet = new TSet();

    void RaiseCollectionChanged(IList<T> before, IList<T> after)
    {
      var args = OnDiff(before, after);
      if (args == null) return;
      if (before.Count != after.Count) RaisePropertyChanged("Count");
      if (this.CollectionChanged == null) return;
      CollectionChanged(this, args);

    }
    void RaisePropertyChanged(string prop)
    {
      if (this.PropertyChanged == null) return;
      PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
    public bool Add(T item)
    {
      return Observe(() => innerSet.Add(item));
    }

    NotifyCollectionChangedEventArgs OnDiff(IList<T> before, IList<T> after)
    {
      var addedElements = after.Except(before).ToList();
      var removedElements = before.Except(after).ToList();

      if (addedElements.Count() > 0 && removedElements.Count() > 0)
      {
        // elements replaced
        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, addedElements, removedElements);
      }
      if (addedElements.Count() > 0)
      {
        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedElements);
      }
      if (removedElements.Count() > 0)
      {
        return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedElements);
      }
      return null;
    }


    public void ExceptWith(IEnumerable<T> other)
    {
      Observe(() => innerSet.ExceptWith(other));
    }

    public void IntersectWith(IEnumerable<T> other)
    {
      Observe(() => innerSet.IntersectWith(other));
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      return innerSet.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      return innerSet.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
      return innerSet.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
      return innerSet.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
      return innerSet.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<T> other)
    {
      return innerSet.SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      Observe(() => innerSet.SymmetricExceptWith(other));
    }

    public void UnionWith(IEnumerable<T> other)
    {
      Observe(() => innerSet.UnionWith(other));
    }

    void ICollection<T>.Add(T item)
    {
      Add(item);
    }

    public void Clear()
    {
      var before = innerSet.ToList();
      innerSet.Clear();
      RaiseCollectionChanged(before, innerSet.ToList());
    }

    public bool Contains(T item)
    {
      return innerSet.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      innerSet.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return innerSet.Count; }
    }


    private void Observe(Action action)
    {
      var before = innerSet.ToList();
      action();
      var after = innerSet.ToList();
      RaiseCollectionChanged(before, after);
    }
    private TResult Observe<TResult>(Func<TResult> action)
    {
      var result = default(TResult);
      Observe(new Action(() => { result = action(); }));
      return result;
    }

    public bool Remove(T item)
    {
      return Observe(() => innerSet.Remove(item));
    }

    public IEnumerator<T> GetEnumerator()
    {
      return innerSet.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return ((System.Collections.IEnumerable)innerSet).GetEnumerator();
    }


    public bool IsReadOnly
    {
      get { return ((ICollection<T>)innerSet).IsReadOnly; }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public event NotifyCollectionChangedEventHandler CollectionChanged;
  }
}
