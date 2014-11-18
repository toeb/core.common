using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Core.BitRot.Nodes
{
  /**
   * <summary> A Node storage implementation which stores the node centrally.</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="TNodeType"> Type of the node type.</typeparam>
   */
  public class NodeStorageMemoryCentral<TNodeType> : INodeStorage<TNodeType> where TNodeType: Node<TNodeType>
  {
    private IDictionary<TNodeType, Tuple< ICollection<TNodeType>,ICollection<TNodeType>>> _neighbors;

    public NodeStorageMemoryCentral()
    {
      _neighbors = CollectionFactory.CreateDictionary<TNodeType, Tuple<ICollection<TNodeType>, ICollection<TNodeType>>>();
    }

    protected Tuple< ICollection<TNodeType>,ICollection<TNodeType>> Neighbors(TNodeType currentNode)
    {
      if (_currentNode == currentNode) return _currentTuple;
       Tuple< ICollection<TNodeType>,ICollection<TNodeType>>  tuple =null;
      if (!_neighbors.ContainsKey(currentNode))
      {
        var successors = CollectionFactory.CreateCollection<TNodeType>();
        var predecessors = CollectionFactory.CreateCollection<TNodeType>();
        tuple = new Tuple<ICollection<TNodeType>, ICollection<TNodeType>>(predecessors, successors);
        _neighbors[currentNode] = tuple;
      }
      _currentNode = currentNode;
      _currentTuple = tuple;
      return tuple;
    }

    TNodeType _currentNode = null;
    Tuple<ICollection<TNodeType>, ICollection<TNodeType>> _currentTuple = null;

    protected ICollection<TNodeType> Successors(TNodeType currentNode)
    {
      return Neighbors(currentNode).Item2;
    }
    protected ICollection<TNodeType> Predecessors(TNodeType currentNode)
    {
      return Neighbors(currentNode).Item1;
    }
    public void DeleteSuccessor(TNodeType currentNode, TNodeType successor)
    {
      Successors(currentNode).Remove(successor);
    }

    public void DeletePredecessor(TNodeType currentNode, TNodeType predecessor)
    {
      Predecessors(currentNode).Remove(predecessor);
    }

    public void StoreSuccessor(TNodeType currentNode, TNodeType successor)
    {
      var successors = Successors(currentNode);
      if (successors.Contains(successor)) return;
      successors.Add(successor);
    }

    public void StorePredecessor(TNodeType currentNode, TNodeType predecessor)
    {
      var predecessors = Predecessors(currentNode);
      if (predecessors.Contains(predecessor)) return;
      predecessors.Add(predecessor);
    }

    public IEnumerable<TNodeType> GetPredecessors(TNodeType currentNode)
    {
      return Predecessors(currentNode);
    }

    public IEnumerable<TNodeType> GetSuccessors(TNodeType currentNode)
    {
      return Successors(currentNode);
    }
  }
}
