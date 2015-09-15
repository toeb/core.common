using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public class RandomAccessEnumerator<T> : IEnumerator<T>, IEnumerable<T>
  {
    public RandomAccessEnumerator(IEnumerator<T> enumerator)
    {
      Inner = enumerator;
      // if enumerator has alread started first element needs to be added
      try
      {
        Elements.Add(enumerator.Current);
      }catch(Exception){}
    }
    public T Current
    {
      get { return Inner.Current; }
    }

    public void Dispose()
    {
      Elements.Clear();
      Inner.Dispose();
    }

    object System.Collections.IEnumerator.Current
    {
      get { return Current; }
    }

    public bool MoveNext()
    {
      var result=  Inner.MoveNext();
      Elements.Add(Inner.Current);
      return result;
    }

    public void Reset()
    {
      Elements.Clear();
      Inner.Reset();
    }

    private IEnumerator<T> Inner { get; set; }
    private List<T> Elements = new List<T>();

    public IEnumerator<T> GetEnumerator()
    {
      foreach (var element in Elements) yield return element;
      while (MoveNext()) yield return Current;
    }

    public IEnumerable<T> ReadElements
    {
      get
      {
        return Elements;
      }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
