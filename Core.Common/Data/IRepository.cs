using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.Data
{
  public interface IRepository<T>
  {
    void Create(T entity);
    void Delete(Func<T, bool> predicate);
    void Update(Func<T, bool> predicate, T entity);
    IEnumerable<T> Get();
  }
}
