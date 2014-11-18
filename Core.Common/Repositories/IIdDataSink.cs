using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public interface IIdDataSink<TId, T> : IDataSink<T>
  {
    void Delete(TId id);
    void Update(TId id, T value);
  }
  public interface IAsyncIdDataSink<TId, T> : IAsyncDataSink<T>, IIdDataSink<TId, T>
  {
    Task Delete(TId id);
    Task Update(TId id, T value);
  }
}
