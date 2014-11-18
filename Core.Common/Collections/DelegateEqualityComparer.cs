using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{
  class DelegateEqualityComparer<T> : IEqualityComparer<T>
  {
    public DelegateEqualityComparer(Func<T, T, bool> compare)
    {
      this.Compare = compare;
    }
    public bool Equals(T x, T y)
    {
      return Compare(x, y);
    }

    public int GetHashCode(T obj)
    {
      return obj.GetHashCode();
    }

    public Func<T, T, bool> Compare { get; set; }
  }
}
