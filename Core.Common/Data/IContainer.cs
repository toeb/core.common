using System.Collections;

namespace Core.Common.Data
{
  public interface IContainer : IEnumerable
  {
    bool Add(object item);
    bool Remove(object item);
    bool Contains(object item);
  }
}
