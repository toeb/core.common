using Core.Repositories;

namespace Core.Modules.Entities
{
  public interface IDataSourceProvider{
    IDataSource GetDataSource(DataSourceDescription description);
  }
}
