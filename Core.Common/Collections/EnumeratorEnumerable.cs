using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public class EnumeratorEnumerable<T> : IEnumerable<T>
  {
    private IEnumerator<T> enumerator;
    public EnumeratorEnumerable(IEnumerator<T> enumerator)
    {
      this.enumerator = enumerator;
    }
    public IEnumerator<T> GetEnumerator()
    {
      return enumerator;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return enumerator;
    }
  }

}
