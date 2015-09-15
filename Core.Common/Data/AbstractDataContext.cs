using Core.Common.Reflect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Crypto;

namespace Core.Common.Data
{



  public abstract class MultiSet
  {
    private IDictionary<Type, ICollection> sets = new Dictionary<Type, ICollection>();

    protected abstract ICollection<T> MakeSet<T>() where T : class;

    private static MethodInfo createCollectionMethod;



    protected virtual void OnCollectionCreated(Type type, ICollection collection)
    {

    }
    protected virtual void OnCollectionRemoved(Type type, ICollection collection)
    {

    }
    protected virtual void OnItemAdded(Type type, ICollection collection, object item)
    {

    }
    protected virtual void OnItemRemoved(Type type, ICollection collection, object item)
    {

    }





    protected ICollection CreateSet(Type itemType)
    {
      if (createCollectionMethod == null)
        createCollectionMethod = typeof(AbstractDataContext).GetMethod("MakeSet", BindingFlags.NonPublic | BindingFlags.Instance);

      Trace.WriteLine("Creating Collection for " + itemType.Name);
      var method = createCollectionMethod.MakeGenericMethod(itemType);
      var collection = method.Invoke(this, new object[0]) as ICollection;
      return collection;
    }

    protected ICollection Set(Type type)
    {
      ICollection set;
      if (sets.TryGetValue(type, out set)) return set;
      set = CreateSet(type);
      sets[type] = set;
      return set;
    }



    public ICollection<T> Set<T>() where T : class
    {
      var collection = Set(typeof(T));
      return collection as ICollection<T>;
    }

    protected IEnumerable<Type> Types { get { return sets.Keys; } }
    protected IEnumerable<KeyValuePair<Type, ICollection>> GetSets() { return sets; }

    protected void AddToSet(object item)
    {
      var type = item.GetType();
      var collection = Set(type) as IContainer;
      collection.Add(item);
    }
    protected void RemoveFromSet(object item)
    {
      var type = item.GetType();
      var collection = Set(type);
      var removeMethod = collection.GetType().GetMethod("Remove");
      removeMethod.Invoke(collection, new[] { item });
    }
  }


  public abstract class AbstractDataContext : MultiSet, IDataContext
  {
    private IDictionary<object, IEntry> entities = new Dictionary<object, IEntry>();






    /// <summary>
    /// needs to return the hash value for the specified entity
    /// the hash needs to change only if entity's persistent values change
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract object GetHash(object entity);
    protected abstract object GenerateId(object item);
    protected abstract IEntry CreateEntry(object item);
    /// <summary>
    /// needs to return the set of referenced entities including entity itself
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract IEnumerable<object> GetReferencedEntities(object entity);

    public bool ChangeState(IEntry entry, EntityState oldState, EntityState newState, Action<EntityState> setState)
    {
      if (oldState == newState) return false;
      var oldHash = entry.Hash;
      var newHash = GetHash(entry.Value);
      setState(newState);


      if (newState == EntityState.Deleted || newState == EntityState.Detached)
      {
        RemoveFromSet(entry.Value);
      }
      else
      {
        AddToSet(entry.Value);
      }

      var references = GetReferencedEntities(entry.Value);

      foreach (var reference in references)
      {
        var e = Entry(reference);
        if (e.State == EntityState.Detached)
        {
          e.State = EntityState.Attached;
        }
      }
      if (DataChanged != null) DataChanged(this, new DataContextChangedEventArgs(entry));
      return true;
    }


    public event DataChangedEventHandler DataChanged;

    public IEnumerable<IEntry> Entries { get { return entities.Values; } }

    public IEntry GetEntry(object entity)
    {
      if (entity == null) return null;
      lock (entities)
      {
        IEntry entry;
        if (!entities.TryGetValue(entity, out entry)) return null;
        return entry;
      }
    }
    public IEntry Entry(object entity)
    {
      if (entity == null) return null;
      var graph = GetReferencedEntities(entity);
      lock (entities)
      {
        foreach (var node in graph)
        {
          if (node == null) continue;
          if (entities.ContainsKey(node)) continue;
          var entry = CreateEntry(node) as SimpleEntry;
          entities[node] = entry;
        }

        return entities[entity];

      }
    }


    public Task SaveAsync()
    {
      return DoSaveAsync();
    }

    protected abstract Task DoSaveAsync();
    protected abstract Task DoRefreshAsync();



    public Task RefreshAsync()
    {
      return DoRefreshAsync();
    }




    protected IEntry GetEntryById(object id)
    {
      return Entries.SingleOrDefault(e => e.Id.Equals(id));
    }
    protected object GetById(object id)
    {
      var entry = GetEntryById(id);
      if (entry == null) return null;
      return entry.Value;
    }


  }
}
