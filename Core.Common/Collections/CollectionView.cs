using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public class CollectionView<T> : ICollectionView<T>
  {

    public CollectionView(ICollection<T> collection, Func<T, bool> predicate)
    {
      InnerCollection = collection;
      Predicate = predicate;

    }
    public bool Add(T item)
    {
      if (!Predicate(item)) return false;
      InnerCollection.Add(item);
      return true;
    }

    public bool Remove(T item)
    {
      if (!Predicate(item)) return false;
      return InnerCollection.Remove(item);
    }

    void ICollection<T>.Add(T item)
    {
      if (!Add(item)) throw new ArgumentException("predicate must evaluate to true for item", "item");
    }

    public void Clear()
    {
      foreach (var item in this)
      {
        Remove(item);
      }
    }

    public bool Contains(T item)
    {
      return Predicate(item) && InnerCollection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      foreach (var item in this)
      {
        array[arrayIndex++] = item;
      }
    }

    public int Count
    {
      get { return InnerCollection.Count(Predicate); }
    }

    public bool IsReadOnly
    {
      get { return InnerCollection.IsReadOnly; }
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var item in InnerCollection)
      {
        if (Predicate(item)) yield return item;
      }
      yield break;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public ICollection<T> InnerCollection { get; set; }
    public Func<T, bool> Predicate { get; set; }

  }

}
