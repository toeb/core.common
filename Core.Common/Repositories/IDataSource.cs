using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public interface IDataSource
  {
    IQueryable Read();
  }

  public interface IAsyncDataSource : IDataSource
  {
    Task<IQueryable> ReadAsync();
  }
}
