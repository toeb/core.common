using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Nodes
{


  public abstract class GraphBase<T, TId> : Graph<T>
    where T : INode<T>, IIdentifiable<TId>
    where TId : IComparable, IComparable<TId>, IEquatable<TId>
  {
    /// <summary>
    /// the datastore for this graph
    /// </summary>
    public abstract IDictionary<TId, T> Dictionary { get; }

    protected sealed override void AddNode(T item)
    {
      Dictionary[item.Id] = item;
    }

    protected sealed override void RemoveNode(T item)
    {
      Dictionary.Remove(item.Id);
    }

    public sealed override bool Contains(T item)
    {
      return Dictionary.ContainsKey(item.Id);
    }

    public sealed override IEnumerator<T> GetEnumerator()
    {
      return Dictionary.Values.GetEnumerator();
    }
  }
}
