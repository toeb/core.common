using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public class CollectionTransform<TOut, TIn> : ICollection<TOut>
  {

    public CollectionTransform(ICollection<TIn> collection, Func<TIn, TOut> convertTo, Func<TOut, TIn> convertFrom)
    {
      InnerCollection = collection;
      ConvertTo = convertTo;
      ConvertFrom = convertFrom;
    }

    protected virtual TIn Transform(TOut item)
    {
      return ConvertFrom(item);
    }
    protected virtual TOut Transform(TIn item)
    {
      return ConvertTo(item);
    }


    public Func<TIn, TOut> ConvertTo { get; set; }
    public Func<TOut, TIn> ConvertFrom { get; set; }

    public ICollection<TIn> InnerCollection { get; set; }

    public void Add(TOut item)
    {
      InnerCollection.Add(Transform(item));
    }

    public void Clear()
    {
      foreach (var item in this)
      {
        InnerCollection.Remove(Transform(item));
      }
    }

    public bool Contains(TOut item)
    {
      return InnerCollection.Contains(Transform(item));
    }


    public void CopyTo(TOut[] array, int arrayIndex)
    {
      foreach (var item in this)
      {
        array[arrayIndex++] = item;
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

    public bool Remove(TOut item)
    {
      return InnerCollection.Remove(Transform(item));
    }

    public IEnumerator<TOut> GetEnumerator()
    {
      foreach (var item in InnerCollection)
      {
        yield return Transform(item);
      }
      yield break;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

}
