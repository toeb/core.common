using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{


  public static class CompositeEnumerator
  {
    public static CompositeEnumerator<T> Create<T>(params IEnumerator<T>[] enumerators)
    {
      return new CompositeEnumerator<T>(enumerators);
    }
  }
  public class CompositeEnumerator<T> : IEnumerator<T>
  {
    private IEnumerator<IEnumerator<T>> enumerators;
    public CompositeEnumerator(IEnumerable<IEnumerator<T>> enumerators)
      : this(enumerators.GetEnumerator())
    {
    }
    public CompositeEnumerator(IEnumerator<IEnumerator<T>> enumerators)
    {
      this.enumerators = enumerators;
    }
    public CompositeEnumerator(params IEnumerator<T>[] enumerators)
      : this(enumerators.AsEnumerable())
    {
    }

    public T Current
    {
      get { return enumerators.Current.Current; }
    }

    public void Dispose()
    {
      enumerators.Reset();
      while (enumerators.MoveNext())
      {
        enumerators.Current.Reset();
      }
      enumerators.Dispose();
    }

    object System.Collections.IEnumerator.Current
    {
      get { return Current; }
    }

    public bool MoveNext()
    {
      var current = enumerators.Current;
      if (current.MoveNext()) return true;
      if (!enumerators.MoveNext()) return false;
      return enumerators.Current.MoveNext();
    }

    public void Reset()
    {
      enumerators.Reset();
      while (enumerators.MoveNext())
      {
        enumerators.Current.Reset();
      }
      enumerators.Reset();
    }
  }
}
