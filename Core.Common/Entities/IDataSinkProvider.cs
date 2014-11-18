using Core.Repositories;

namespace Core.Modules.Entities
{
  public interface IDataSinkProvider
  {
    IDataSink GetDataSink(DataSinkDescription description);
  }
}
