using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{

  public interface IRepository<T> : IRepository, IDataSource<T>, IDataSink<T>
  {

  }
  public interface IAsyncRepository<T> : IRepository<T>, IAsyncDataSource<T>,IAsyncDataSink<T>
  {

  }
}
