using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identification
{
  public interface IIdentityAccessor<T, TId>
  {
    TId GetIdentity(T item);
    void SetIdentity(T item, TId id);
  }
}
