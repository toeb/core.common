using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{


  public interface IIdRepository<TId, T> : IRepository<T>, IIdDataSource<TId, T>, IIdDataSink<TId, T>
  {
  }
}
