using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Core.BitRot.Nodes
{
  /**
   * <summary> A Storage Implementation which stores the nodes locally in every node</summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   *
   * <typeparam name="TNodeType"> Type of the node type.</typeparam>
   */
  public class NodeStorageMemoryLocal<TNodeType> : INodeStorage<TNodeType> where TNodeType : Node<TNodeType>
  {
    public NodeStorageMemoryLocal(TNodeType node)
    {
      _node = node;
      _predecessors = CollectionFactory.CreateCollection<TNodeType>();
      _successors = CollectionFactory.CreateCollection<TNodeType>();
    }
    private TNodeType _node;
    private ICollection<TNodeType> _predecessors;
    private ICollection<TNodeType> _successors;

    public void DeleteSuccessor(TNodeType currentNode, TNodeType successor)
    {
      Contract.Assume(currentNode == _node);
      _successors.Remove(successor);
    }

    public void DeletePredecessor(TNodeType currentNode, TNodeType predecessor)
    {
      Contract.Assume(currentNode == _node);
      _predecessors.Remove(predecessor);
    }

    public void StoreSuccessor(TNodeType currentNode, TNodeType successor)
    {
      Contract.Assume(currentNode == _node);
      if (_successors.Contains(successor)) return;
      _successors.Add(successor);
    }

    public void StorePredecessor(TNodeType currentNode, TNodeType predecessor)
    {
      Contract.Assume(currentNode == _node);
      if (_predecessors.Contains(predecessor)) return;
      _predecessors.Add(predecessor);
    }

    public IEnumerable<TNodeType> GetPredecessors(TNodeType currentNode)
    {
      Contract.Assume(currentNode == _node);
      return _predecessors;
    }

    public IEnumerable<TNodeType> GetSuccessors(TNodeType currentNode)
    {
      Contract.Assume(currentNode == _node);
      return _successors;
    }
  }
}
