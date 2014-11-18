
using Core.Repositories;
using System;
namespace Core.Modules.Entities
{

  public class EntityContextDescription
  {
    public EntityContextDescription() { ContractName = "DefaultEntityContext"; }
    public static readonly EntityContextDescription Default = new EntityContextDescription();
    public string ContractName { get; set; }
    public Type RequiredType { get; set; }

  }
  public static class IEntityServiceExtensions 
  {
    public static IEntityContext GetContext(this IEntityService service)
    {
      return service.GetContext(EntityContextDescription.Default);
    }
    public static IEntityContext GetContext<T>(this IEntityService service)
    {
      var description = new EntityContextDescription();
      description.RequiredType = typeof(T);
      description.ContractName = description.RequiredType.FullName;
      return service.GetContext(description);
    }


  }
  public interface IEntityService
  {
    IEntityContext GetContext(EntityContextDescription description);
    void SetDataSource(DataSourceDescription description, IDataSource dataSource);
    void SetDataSink(DataSinkDescription description, IDataSink dataSink);
  }
}
