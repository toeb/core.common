using Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Nodes
{
  public interface IGraph<T> : IFeedbackContainer<T> where T : INode<T>
  {

  }
  /// <summary>
  /// a graph consist of Nodes
  /// </summary>
  public abstract class Graph<T> : IGraph<T> where T : INode<T>
  {


    protected virtual void OnNodeAdded(T item) { }
    protected virtual void OnNodeRemoved(T item) { }
    protected virtual void OnConnect(T item) { }
    protected virtual void OnDisconnect() { }

    protected abstract void AddNode(T item);
    protected abstract void RemoveNode(T item);
    public abstract bool Contains(T item);
    public abstract IEnumerator<T> GetEnumerator();

    public bool Add(T item)
    {
      if (Contains(item)) return false;
      int addedNodes = 0;
      AddNode(item);
      foreach (var node in item.BfsNeighbors())
      {
        if (Add(node)) addedNodes++;
      }
      return true;
    }

    public bool Remove(T item)
    {
      if (!Contains(item)) return false;
      item.Disconnect();
      RemoveNode(item);
      return true;
    }



    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}
