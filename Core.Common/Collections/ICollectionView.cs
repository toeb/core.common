using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public interface ICollectionView<T> : ICollection<T>
  {
    new bool Add(T item);
    new bool Remove(T item);
  }
}
