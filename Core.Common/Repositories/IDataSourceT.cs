using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public interface IDataSource<out T> : IDataSource
  {
    new IQueryable<T> Read();
  }
  public interface IAsyncDataSource<T> : IAsyncDataSource, IDataSource<T>
  {
    new Task<IQueryable<T>> ReadAsync();
  }

}
