using Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Core.Modules.Entities
{
  public static class IEntityContextExtensions
  {

    public static async Task CreateAndSaveAsync<T>(this IEntityContext self, T entity) where T : class
    {
      try
      {
        await self.CreateAsync(entity);
        await self.SaveChangesAsync();
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public static async Task UpdateAndSaveAsync<T>(this IEntityContext self, T entity) where T : class
    {
      await self.UpdateAsync(entity);
      await self.SaveChangesAsync();
    }

    public static async Task DeleteAndSaveAsync<T>(this IEntityContext self, T entity) where T : class
    {
      await self.DeleteAsync(entity);
      await self.SaveChangesAsync();
    }

    public static IDataSource<T> GetDataSource<T>(this IDataSourceProvider self)
    {
      var description = new DataSourceDescription()
      {
        Contract = typeof(T).FullName,
        RequiredType = typeof(T),
        DataSourceType = typeof(IDataSource<T>)
      };
      return self.GetDataSource(description) as IDataSource<T>;
    }
    public static IIdDataSource<TId, T> GetIdDataSource<TId, T>(this IDataSourceProvider self)
    {
      var description = new DataSourceDescription()
      {
        Contract = typeof(T).FullName,
        RequiredType = typeof(T),
        DataSourceType = typeof(IIdDataSource<TId, T>)
      };
      return self.GetDataSource(description) as IIdDataSource<TId, T>;

    }

    public static IIdDataSink<TId, T> GetIdDataSink<TId, T>(this IDataSinkProvider self)
    {
      var description = new DataSinkDescription()
      {
        Contract = typeof(T).FullName,
        RequiredType = typeof(T),
        DataSinkType = typeof(IIdDataSink<TId, T>)
      };
      return self.GetDataSink(description) as IIdDataSink<TId, T>;
    }
    public static IDataSink<T> GetDataSink<T>(this IDataSinkProvider self)
    {
      var description = new DataSinkDescription()
      {
        Contract = typeof(T).FullName,
        RequiredType = typeof(T),
        DataSinkType = typeof(IDataSink<T>)
      };
      return self.GetDataSink(description) as IDataSink<T>;
    }
    public static IRepository<T> GetRepository<T>(this IEntityContext context)
    {
      var sink = GetDataSink<T>(context);
      var source = GetDataSource<T>(context);
      return new CompositeRepository<T>(sink, source);
    }
    public static IIdRepository<TId, T> GetIdRepository<TId, T>(this IEntityContext context)
    {
      var source = GetIdDataSource<TId, T>(context);
      var sink = GetIdDataSink<TId, T>(context);
      return new CompositeIdRepository<TId, T>(sink, source);
    }
  }

  class CompositeIdRepository<TId, T> : AbstractRepository<T>, IIdRepository<TId, T>
  {
    public CompositeIdRepository(IIdDataSink<TId, T> sink, IIdDataSource<TId, T> source)
    {
      if (sink == null) throw new ArgumentNullException("sink");
      if (source == null) throw new ArgumentNullException("source");
      this.Sink = sink;
      this.Source = source;


    }
    public override System.Linq.IQueryable<T> Read()
    {
      return Source.Read();
    }

    public override void Create(T item)
    {
      Sink.Create(item);
    }

    public override void Update(T item)
    {
      Sink.Update(item);
    }

    public override void Delete(T item)
    {
      Sink.Delete(item);
    }


    public T GetById(TId id)
    {
      return Source.GetById(id);
    }

    public System.Linq.IQueryable<TId> ReadIds(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
      return Source.ReadIds(predicate);
    }

    public void Delete(TId id)
    {
      Sink.Delete(id);
    }

    public void Update(TId id, T value)
    {
      Sink.Update(id, value);
    }

    public IIdDataSource<TId, T> Source { get; set; }
    public IIdDataSink<TId, T> Sink { get; set; }
  }

  class CompositeRepository<T> : AbstractRepository<T>
  {
    public CompositeRepository(IDataSink<T> sink, IDataSource<T> source)
    {
      if (sink == null) throw new ArgumentNullException("sink");
      if (source == null) throw new ArgumentNullException("source");

      Sink = sink;
      Source = source;

    }
    public override System.Linq.IQueryable<T> Read()
    {
      return Source.Read();
    }

    public override void Create(T item)
    {
      Sink.Create(item);
    }

    public override void Update(T item)
    {
      Sink.Update(item);
    }

    public override void Delete(T item)
    {
      Sink.Delete(item);
    }

    public IDataSource<T> Source { get; set; }

    public IDataSink<T> Sink { get; set; }
  }
}
