using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
namespace Core.Collections
{
  public interface Container
  {

  }
  public interface IContainer<T>
  {
    IEnumerable<T> Elements { get; }
    void Add(T item);
    void Remove(T item);
  }
  public class DelegateContainer<T> : IContainer<T>
  {
    private Action<T> remove;
    private Action<T> add;
    private Func<IEnumerable<T>> elements;
    public DelegateContainer(Func<IEnumerable<T>> elements, Action<T> add, Action<T> remove)
    {
      this.add = add;
      this.remove = remove;
      this.elements = elements;
    }

    public IEnumerable<T> Elements
    {
      get
      {
        return elements();
      }
    }

    public void Add(T item)
    {
      add(item);
    }

    public void Remove(T item)
    {
      remove(item);
    }
  }


  public class EnumerableWrapper : IEnumerable
  {
    IEnumerable enumerable;
    protected IEnumerable InnerEnumerable { get { return enumerable; } }
    public EnumerableWrapper(IEnumerable enumerable)
    {
      this.enumerable = enumerable;
    }
    public virtual IEnumerator GetEnumerator()
    {
      return enumerable.GetEnumerator();
    }
  }
  public class EnumerableWrapper<T> : EnumerableWrapper, IEnumerable<T>
  {
    public EnumerableWrapper(IEnumerable<T> enumerable)
      : base(enumerable)
    {

    }
    public new virtual IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>)InnerEnumerable).GetEnumerator();
    }
  }
  public class CollectionWrapper<T> : EnumerableWrapper<T>, ICollection<T>
  {
    protected ICollection<T> InnerCollection
    {
      get
      {
        return InnerEnumerable as ICollection<T>;
      }
    }
    public CollectionWrapper(ICollection<T> collection)
      : base(collection)
    {
    }
    public void Add(T item)
    {
      OnBeforeAdd(item);
      InnerCollection.Add(item);
      OnAfterAdd(item);
    }

    public bool Remove(T item)
    {
      OnBeforeRemove(item);
      var result = InnerCollection.Remove(item);
      OnAfterRemove(item);
      return result;
    }
    public void Clear()
    {
      OnBeforeClear();
      InnerCollection.Clear();
      OnAfterClear();
    }
    protected virtual void OnAfterAdd(T item) { }
    protected virtual void OnBeforeAdd(T item) { }
    protected virtual void OnAfterClear() { }
    protected virtual void OnBeforeClear() { }
    protected virtual void OnBeforeRemove(T item) { }
    protected virtual void OnAfterRemove(T item) { }


    public bool Contains(T item)
    {
      return InnerCollection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      InnerCollection.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return InnerCollection.Count; }
    }

    public bool IsReadOnly
    {
      get { return InnerCollection.IsReadOnly; }
    }


  }





  public class SetWrapper<T> : ISet<T>
  {
    private Action<T> add;
    private Action<T> remove;
    private Func<IEnumerable<T>> elements;

    public SetWrapper(Action<T> add, Action<T> remove, Func<IEnumerable<T>> elements)
    {
      this.add = add;
      this.remove = remove;
      this.elements = elements;
    }

    public bool CheckIfSet()
    {
      return elements().Count() == elements().Distinct().Count();
    }

    public virtual bool Add(T item)
    {
      if (elements().Contains(item)) return false;
      add(item);
      return true;
    }


    public void ExceptWith(IEnumerable<T> other)
    {
      foreach (var element in other)
      {
        Remove(element);
      }
    }

    public void IntersectWith(IEnumerable<T> other)
    {
      foreach (var element in other)
      {
        if (!Contains(element)) Add(element);
      }
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      return elements().IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      return elements().IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
      return elements().IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
      return elements().IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other)
    {
      return other.Intersect(elements()).Any();
    }

    public bool SetEquals(IEnumerable<T> other)
    {
      
      return other.Except(elements()).Any() && elements().Except(other).Any();
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      var toRemove = elements().Intersect(other);
      var toAdd = other.Except(elements());

      foreach (var element in toAdd)
      {
        add(element);
      }
      foreach (var element in toRemove)
      {
        remove(element);
      }
    }

    public void UnionWith(IEnumerable<T> other)
    {
      foreach (var element in other.Except(elements()).ToArray())
      {
        add(element);
      }
    }

    void ICollection<T>.Add(T item)
    {
      Add(item);
    }

    public void Clear()
    {
      foreach (var element in elements().ToArray())
      {
        remove(element);
      }
    }

    public bool Contains(T item)
    {
      return elements().Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      elements().ToList().CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return elements().Count(); }
    }

    public bool IsReadOnly
    {
      get {
        return false;
      }
    }

    public bool Remove(T item)
    {
      if (!elements().Contains(item)) return false;
      remove(item);
      return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return elements().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
  public class OrderedSet<T> : ISet<T>
  {
    private readonly IDictionary<T, LinkedListNode<T>> dictionary;
    private readonly LinkedList<T> list;

    public OrderedSet()
    {
      dictionary = new Dictionary<T, LinkedListNode<T>>();
      list = new LinkedList<T>();
    }

    public bool Add(T item)
    {
      if (dictionary.ContainsKey(item)) return false;
      var node = list.AddLast(item);
      dictionary.Add(item, node);
      return true;
    }

    void ICollection<T>.Add(T item)
    {
      Add(item);
    }

    public void Clear()
    {
      list.Clear();
      dictionary.Clear();
    }

    public bool Remove(T item)
    {
      LinkedListNode<T> node;
      bool found = dictionary.TryGetValue(item, out node);
      if (!found) return false;
      dictionary.Remove(item);
      list.Remove(node);
      return true;
    }

    public int Count
    {
      get { return dictionary.Count; }
    }

    public IEnumerator<T> GetEnumerator()
    {
      return list.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public bool Contains(T item)
    {
      return dictionary.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      list.CopyTo(array, arrayIndex);
    }


    public virtual bool IsReadOnly
    {
      get { return dictionary.IsReadOnly; }
    }

    public void UnionWith(IEnumerable<T> other)
    {

      throw new NotImplementedException();
    }

    public void IntersectWith(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public void ExceptWith(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool IsSubsetOf(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool IsSupersetOf(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool Overlaps(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

    public bool SetEquals(IEnumerable<T> other)
    {
      throw new NotImplementedException();
    }

  }
}
