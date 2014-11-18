using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
  public abstract class AbstractRepository<T> : IRepository<T>
  {

    public abstract IQueryable<T> Read();
    public abstract void Create(T item);
    public abstract void Update(T item);
    public abstract void Delete(T item);


    IQueryable IDataSource.Read()
    {
      return Read() as IQueryable<object>;
    }


    public virtual void Create(object item)
    {
      Create((T)item);
    }

    public virtual void Update(object item)
    {
      Update((T)item);
    }

    public virtual void Delete(object item)
    {
      Delete((T)item);
    }
  }
  public abstract class AbstractAsyncRepository<T> : AbstractRepository<T>, IAsyncRepository<T>
  {


    public abstract Task<IQueryable<T>> ReadAsync();
    public abstract Task CreateAsync(T item);
    public abstract Task UpdateAsync(T item);
    public abstract Task DeleteAsync(T item);


    async Task<IQueryable> IAsyncDataSource.ReadAsync()
    {
      var queryable = await ReadAsync();
      return queryable;
    }
    Task IAsyncDataSink.CreateAsync(object item) { return CreateAsync((T)item); }
    Task IAsyncDataSink.UpdateAsync(object item) { return UpdateAsync((T)item); }
    Task IAsyncDataSink.DeleteAsync(object item) { return DeleteAsync((T)item); }

    public override IQueryable<T> Read()
    {
      return ReadAsync().Await();
    }

    public override void Create(T item)
    {
      CreateAsync(item).Await();
    }

    public override void Update(T item)
    {
      UpdateAsync(item).Await();
    }

    public override void Delete(T item)
    {
      DeleteAsync(item).Await();
    }


  }
}
