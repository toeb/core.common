using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{
  public static class IEnumeratorExtensions
  {
    public static IEnumerator<T> Restore<T>(this IEnumerator<T> saved)
    {
      var restorable = saved as ISaveRestoreEnumerator<T>;
      if (restorable == null) throw new InvalidOperationException("cannot restore unsaved enumerator");
      return restorable.Restore();
    }
    public static ISaveRestoreEnumerator<T> Save<T>(this IEnumerator<T> original)
    {
      return new LookAheadEnumerator<T>(original);
    }
  }
}
