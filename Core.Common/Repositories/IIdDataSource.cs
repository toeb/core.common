using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public interface IIdDataSource<TId, T> : IDataSource<T>
  {
    T GetById(TId id);
    IQueryable<TId> ReadIds(Expression<Func<T,bool>> predicate);
  }
  public interface IAsyncIdDataSource<TId, T> : IAsyncDataSource<T>
  {
    Task<T> GetByIdAsync(TId id);
    Task<IQueryable<TId>> ReadIdsAsync(Expression<Func<T, bool>> predicate);
  }
}
