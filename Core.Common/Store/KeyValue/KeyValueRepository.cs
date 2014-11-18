using Core.Identification;
using Core.Identifiers;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  public class KeyValueRepository<TId, T> : AbstractRepository<T>,IRepository<T>, IIdRepository<TId, T>
  {
    public KeyValueRepository(IKeyValueStore<TId, T> store, IIdentifierProvider<TId> idProvider, IIdentityAccessor<T, TId> idAccess, IEqualityComparer<TId> equalityComparer)
    {
      this.store = store;
      this.idProvider = idProvider;
      this.idAccess = idAccess;
      this.equalityComparer = equalityComparer;
    }
    IKeyValueStore<TId, T> store;
    IIdentifierProvider<TId> idProvider;
    IIdentityAccessor<T, TId> idAccess;
    IEqualityComparer<TId> equalityComparer;
    public override IQueryable<T> Read()
    {
      return store.Values;
    }

    public override void Create(T item)
    {
      var defaultId = idProvider.DefaultId;
      var itemId = idAccess.GetIdentity(item);
      if (equalityComparer.Equals(defaultId, itemId))
      {
        itemId = idProvider.CreateIdentifier();
        idAccess.SetIdentity(item, itemId);
      }
      if (store.ContainsKey(itemId)) throw new ArgumentException("item's key already exists");

      store.Store(itemId, item);
      //return item;
    }

    protected virtual void OnItemCreated(T item) { }
    protected virtual void OnItemUpdated(T oldItem, T newItem) { }
    protected virtual void OnItemDeleted(T item) { }

    public override void Update(T item)
    {
      var defaultId = idProvider.DefaultId;
      var itemId = idAccess.GetIdentity(item);
      if (equalityComparer.Equals(defaultId, itemId)) throw new ArgumentException("item's id is not set");
      if (!store.ContainsKey(itemId)) throw new ArgumentException("item's does not exist");
      var storeItem = store.Load(itemId);
      store.Store(itemId, item);
      //return item;
    }



    public override void Delete(T item)
    {
      var defaultId = idProvider.DefaultId;
      var itemId = idAccess.GetIdentity(item);
      if (equalityComparer.Equals(defaultId, itemId)) throw new ArgumentException("item's id is not set");
      if (!store.ContainsKey(itemId)) throw new ArgumentException("item's does not exist");
      store.Delete(itemId);
      OnItemDeleted(item);
    }

    public T GetById(TId id)
    {
      if (!store.ContainsKey(id)) return default(T);
      return store.Load(id);
    }

    public void Delete(TId id)
    {
      var item = GetById(id);
      Delete(item);
    }

    public void Update(TId id, T value)
    {
      Update(value);
    }

    IQueryable IDataSource.Read()
    {
      return Read();
    }


    public IQueryable<TId> ReadIds(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
    {
      return Read().Where(predicate).Select(it => this.idAccess.GetIdentity(it));
    }
  }

}
