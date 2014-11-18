using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Modules.Entities
{
  public interface IEntityContext : IDataSourceProvider, IDataSinkProvider, IDisposable
  {
    /// <summary>
    /// describes this entity context
    /// </summary>
    EntityContextDescription ContextDescription { get; }
    
    /// <summary>
    /// reads all entities of type T from storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IQueryable<T>> ReadAsync<T>() where T : class;
    /// <summary>
    /// returns all entities which are already cached locally
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IQueryable<T>> ReadLocalAsync<T>() where T : class;
    /// <summary>
    /// returns all entities which are cached locally and also queries the storage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IQueryable<T>> ReadLocalAndRemoteAsync<T>() where T : class;
    /// <summary>
    /// returns the specified entity by id
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync<TEntity, TKey>(TKey key) where TEntity : class;
    /// <summary>
    /// deletes the specified entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task DeleteAsync<T>(T entity) where T : class;
    /// <summary>
    /// stores changes of the specified entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync<T>(T entity) where T : class;
    /// <summary>
    /// creates the entity in this context. does not persist to database. call savechanges
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task CreateAsync<T>(T entity) where T : class;
    /// <summary>
    /// saves all entities which still have pending changes
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
  }



}
