using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{


  public interface IFeedbackContainer<T> : IEnumerable<T>
  {
    bool Add(T item);
    bool Remove(T item);
    bool Contains(T item);
  }
}
