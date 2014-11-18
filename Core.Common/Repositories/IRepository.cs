using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{

  public interface IRepository : IDataSource, IDataSink
  {

  }

  public interface IAsyncRepository : IAsyncDataSource
  {

  }
}
