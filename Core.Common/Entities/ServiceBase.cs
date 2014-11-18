using System.ComponentModel.Composition;
using Core.Modules.Entities;


namespace Core.Modules.Entities
{
  public class ServiceBase
  {
    [Import(AllowRecomposition = true)]
    public IEntityService EntityService
    {
      get;
      set;
    }

    [Import(AllowRecomposition = true)]
    public IContextService ContextService
    {
      get;
      set;
    }


    protected IEntityContext Entities
    {
      get
      {
        var context = ContextService.GetContext(ContextDescriptor.Request);
        var entityContext = context.Get<IEntityContext>();
        if (entityContext != null) return entityContext;
        entityContext = EntityService.GetContext();
        context.Set(entityContext);
        return entityContext;
      }
    }

  }
}