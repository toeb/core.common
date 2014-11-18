using Core.Modules.Rules;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Modules.Entities
{
  class EntityContext : IEntityContext
  {
    private EntityService entityService;

    public EntityContext(EntityService entityService, EntityContextDescription description)
    {
      ContextDescription = description;
      this.entityService = entityService;
    }

    public IDataSource GetDataSource(DataSourceDescription description)
    {
      return entityService.RequireDataSource(description);
    }

    public void Dispose()
    {
      entityService.ReleaseContext(this);
    }


    public IDataSink GetDataSink(DataSinkDescription description)
    {
      return entityService.RequireDataSink(description);
    }

    public EntityContextDescription ContextDescription
    {
      get;
      private set;
    }



    public async Task<IQueryable<T>> ReadAsync<T>() where T : class
    {
      return this.GetDataSource<T>().Read();
    }

    public async Task<TEntity> GetByIdAsync<TEntity, TKey>(TKey key) where TEntity : class
    {
      return this.GetIdDataSource<TKey, TEntity>().GetById(key);
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
      this.GetDataSink<T>().Delete(entity);
    }

    public async Task UpdateAsync<T>(T entity) where T : class
    {
      this.GetDataSink<T>().Update(entity);
    }

    public async Task CreateAsync<T>(T entity) where T : class
    {
      this.GetDataSink<T>().Create(entity);
    }

    public async Task SaveChangesAsync()
    {

    }

    public Task<IQueryable<T>> ReadLocalAndRemoteAsync<T>() where T : class
    {
      return ReadAsync<T>();
    }


    public async Task<IQueryable<T>> ReadLocalAsync<T>() where T : class
    {
      return new T[0].AsQueryable();
    }
  }
  public static class Constants
  {
    public const string DataContextName = "DataContext";
  }
  public static class IRulesContextExtensions
  {
    public static IEntityContext GetDataContext(this IRuleContext self)
    {
      return self.Get<IEntityContext>(Constants.DataContextName);
    }
    public static IEntityContext GetEntityContext(this IRuleContext self)
    {
      return self.Get<IEntityContext>(Constants.DataContextName);
    }

  }

  public class RulesDataContext : IEntityContext
  {
    public const string BeforeRead = "BeforeRead";
    public const string AfterRead = "AfterRead";
    public const string BeforeCreate = "BeforeCreate";
    public const string AfterCreate = "AfterCreate";
    public const string BeforeDelete = "BeforeDelete";
    public const string AfterDelete = "AfterDelete";
    public const string BeforeSave = "BeforeSave";
    public const string AfterSave = "AfterSave";
    public const string BeforeUpdate = "BeforeUpdate";
    public const string AfterUpdate = "AfterUpdate";



    [Import]
    public IRulesService RulesService { get; set; }

    public IEntityContext InnerContext { get; set; }
    public RulesDataContext(IEntityContext innerContext)
    {
      InnerContext = innerContext;
    }
    IRuleContext CreateContext<T>(string action, string extensionpoint, object resource)
    {
      return CreateContext(action, extensionpoint, typeof(T), resource);

    }

    IRuleContext CreateContext(string action, string extensionpoint, Type resourceType, object resource)
    {
      var context = new RuleContext()
      {
        Action = action,
        ExtensionPoint = extensionpoint,
        Resource = resource
      };
      context.Data["ResourceType"] = resourceType;
      context.Data[Constants.DataContextName] = this;
      return context;
    }

    public async Task<IQueryable<T>> ReadAsync<T>() where T : class
    {
      await this.RulesService.ApplyRules(CreateContext<IQueryable<T>>("Read", "Entry", null));
      var result = await InnerContext.ReadAsync<T>();
      await this.RulesService.ApplyRules(CreateContext<IQueryable<T>>("Read", "Exit", result));
      return result;
    }
    public async Task<TEntity> GetByIdAsync<TEntity, TKey>(TKey key) where TEntity : class
    {
      await this.RulesService.ApplyRules(CreateContext<TEntity>("Read", "Entry", null));
      var result = await InnerContext.GetByIdAsync<TEntity, TKey>(key);
      await this.RulesService.ApplyRules(CreateContext<TEntity>("Read", "Exit", result));
      return result;
    }
    public async Task DeleteAsync<T>(T entity) where T : class
    {
      await this.RulesService.ApplyRules(CreateContext<T>("Delete", "Entry", entity));
      await InnerContext.DeleteAsync(entity);
      await this.RulesService.ApplyRules(CreateContext<T>("Delete", "Exit", entity));
    }
    public async Task UpdateAsync<T>(T entity) where T : class
    {
      await this.RulesService.ApplyRules(CreateContext<T>("Update", "Entry", entity));
      await InnerContext.UpdateAsync(entity);
      await this.RulesService.ApplyRules(CreateContext<T>("Update", "Exit", entity));
    }
    public async Task CreateAsync<T>(T entity) where T : class
    {
      await this.RulesService.ApplyRules(CreateContext<T>("Create", "Entry", entity));
      await InnerContext.CreateAsync(entity);
      await this.RulesService.ApplyRules(CreateContext<T>("Create", "Exit", entity));
    }
    public async Task SaveChangesAsync()
    {
      await this.RulesService.ApplyRules(CreateContext("SaveChanges", "Entry", null, null));
      await InnerContext.SaveChangesAsync();
      await this.RulesService.ApplyRules(CreateContext("SaveChanges", "Exit", null, null));
    }
    public void Dispose()
    {
      InnerContext.Dispose();
    }


    public EntityContextDescription ContextDescription
    {
      get { return InnerContext.ContextDescription; }
    }

    public IDataSource GetDataSource(DataSourceDescription description)
    {
      return InnerContext.GetDataSource(description);
    }

    public IDataSink GetDataSink(DataSinkDescription description)
    {
      return InnerContext.GetDataSink(description);
    }


    public Task<IQueryable<T>> ReadLocalAsync<T>() where T : class
    {
      return InnerContext.ReadLocalAsync<T>();
    }


    public Task<IQueryable<T>> ReadLocalAndRemoteAsync<T>() where T : class
    {
      return InnerContext.ReadLocalAndRemoteAsync<T>();
    }
  }

}
