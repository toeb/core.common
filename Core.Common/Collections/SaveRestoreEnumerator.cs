using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public class LookAheadEnumerator<T> : ISaveRestoreEnumerator<T>
  {
    public LookAheadEnumerator(IEnumerator<T> inner)
    {
      this.Inner = inner;
      ElementQueue.Enqueue(inner.Current);
    }
    public T Current
    {
      get { return Inner.Current; }
    }

    public void Dispose()
    {
      Inner.Dispose();
    }

    object System.Collections.IEnumerator.Current
    {
      get { return Current; }
    }

    public bool MoveNext()
    {
      if (!Inner.MoveNext()) return false;
      ElementQueue.Enqueue(Inner.Current);
      return true;
    }

    public void Reset()
    {
      Inner.Reset();
      ElementQueue.Clear();
    }

    private IEnumerator<T> InnerRestore()
    {
      while (ElementQueue.Count != 0) yield return ElementQueue.Dequeue();
      while (Inner.MoveNext()) yield return Inner.Current;
    }
    public IEnumerator<T> Restore()
    {
      if (ElementQueue.Count == 0) return Inner;
      var res = InnerRestore();
      res.MoveNext();
      return res;
    }

    private Queue<T> ElementQueue = new Queue<T>();
    public IEnumerator<T> Inner { get; set; }
  }
}
