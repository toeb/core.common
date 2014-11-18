using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{


  public interface IDataSink
  {
    void Create(object item);
    void Update(object item);
    void Delete(object item);
  }
  public interface IAsyncDataSink
  {
    Task CreateAsync(object item);
    Task UpdateAsync(object item);
    Task DeleteAsync(object item);
  }
}
