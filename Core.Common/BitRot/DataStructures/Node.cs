using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Core.BitRot.Nodes
{
  /**
   * <summary> Node base class defining some operations
   * 					 only the 6 Abstract node access mehtods 
   * 					 need to be implemented by base type.
   * 					 
   * 					 This Node ensures consistency in  the graph </summary>
   *
   * <remarks> Tobi, 3/15/2012.</remarks>
   */
  public abstract class Node<TNodeType> where TNodeType : Node<TNodeType>
  {
    #region abstract members
    //  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected abstract INodeStorage<TNodeType> Storage { get; }
    protected abstract void DoExpand();
    #endregion

    /**
     * <summary> Gets or sets a value indicating whether this object is expanded.
     * 					 a nodes needs to be expanded before accessing its members  
     * 					 allowing custom loading etc</summary>
     *
     * <value> true if this object is expanded, false if not.</value>
     */
    public bool IsExpanded
    {
      get;
      private set;
    }

    private void DeleteSuccessor(TNodeType successor)
    {
      if (CancelDeleteSuccessor(successor))
      {
        return;
      }
      Expand();
      Storage.DeleteSuccessor(This, successor);
      OnSuccessorDeleted(successor);
      OnNeighborDeleted(successor);
      OnNeighborChanged(successor);
    }

    protected virtual bool CancelDeleteSuccessor(TNodeType successor)
    {
      return false;
    }
    protected virtual void DeletePredecessor(TNodeType predecessor)
    {
      if (CancelDeletePredecessor(predecessor))
      {
        return;
      }
      Expand();
      Storage.DeletePredecessor(This, predecessor);
      OnPredecessorDeleted(predecessor);
      OnNeighborDeleted(predecessor);
      OnNeighborChanged(predecessor);
    }

    protected virtual bool CancelDeletePredecessor(TNodeType predecessor)
    {
      return false;
    }

    private void StoreSuccessor(TNodeType successor)
    {
      if (CancelStoreSuccessor(successor))
      {
        return;
      }
      Expand();
      OnBeforeStoreSuccessor(successor);
      Storage.StoreSuccessor(This, successor);
      OnSuccessorStored(successor);
      OnNeighborStored(successor);
      OnNeighborChanged(successor);
    }

    protected virtual bool CancelStoreSuccessor(TNodeType successor)
    {
      return false;
    }
    private void StorePredecessor(TNodeType predecessor)
    {
      if (CancelStorePredecessor(predecessor))
      {
        return;
      }
      Expand();
      OnBeforeStorePredecessor(predecessor);
      Storage.StorePredecessor(This, predecessor);
      OnPredecessorStored(predecessor);
      OnNeighborStored(predecessor);
      OnNeighborChanged(predecessor);
    }

    protected virtual void OnBeforeStoreSuccessor(TNodeType successor) { }
    protected virtual void OnBeforeStorePredecessor(TNodeType predecessor) { }

    protected virtual bool CancelStorePredecessor(TNodeType predecessor)
    {
      return false;
    }



    public IEnumerable<TNodeType> Predecessors
    {
      get { Expand(); return Storage.GetPredecessors(This); }
      /* set
       {
         Expand();
         foreach (var node in value)
         {
           AddPredecessor(node);
         }
       }*/
    }


    public IEnumerable<TNodeType> Successors
    {
      get { Expand(); return Storage.GetSuccessors(This); }
      /*  set
        {
          Expand();
          foreach (var node in value)
          {
            AddSuccessor(node);
          }
        }*/
    }

    #region extension points
    protected virtual void OnNeighborChanged(TNodeType node) { }
    protected virtual void OnNeighborDeleted(TNodeType node) { }
    protected virtual void OnNeighborStored(TNodeType node) { }
    protected virtual void OnBeforeExpand() { }
    protected virtual void OnExpanded() { }
    protected virtual void OnSuccessorDeleted(TNodeType successor) { }
    protected virtual void OnPredecessorDeleted(TNodeType predecessor) { }
    protected virtual void OnSuccessorStored(TNodeType successor) { }
    protected virtual void OnPredecessorStored(TNodeType predecessor) { }
    #endregion

    /**
     * <summary> Expands this node returns directly if node is already expanded .</summary>
     *
     * <remarks> Tobi, 15.03.2012.</remarks>
     */
    public void Expand()
    {
      if (IsExpanded) return;
      OnBeforeExpand();
      DoExpand();
      IsExpanded = true;
      OnExpanded();
    }

    /**
     * <summary> Gets a partial order. for the node in the graph</summary>
     *
     * <remarks> Tobi, 15.03.2012.</remarks>
     *
     * <param name="result">       The result.</param>
     * <param name="predecessors"> (optional) the predecessors.</param>
     */
    public IEnumerable<TNodeType> GetPartialOrder(
      Func<TNodeType, IEnumerable<TNodeType>> predecessors,
      Func<TNodeType, IEnumerable<TNodeType>> successors)
    {
      throw new NotImplementedException();
    }

    public void BreadthFirstSearch(Action<TNodeType> compute, Func<TNodeType, IEnumerable<TNodeType>> children, Func<bool> stop = null)
    {
      HashSet<TNodeType> nodesTouched = new HashSet<TNodeType>();
      Queue<TNodeType> nodeQueue = new Queue<TNodeType>();
      nodeQueue.Enqueue(This);

      while (nodeQueue.Count != 0)
      {
        if (stop != null)
        {
          if (stop()) return;
        }
        var currentNode = nodeQueue.Dequeue();
        nodesTouched.Add(currentNode);
        compute(currentNode);
        foreach (var child in children(currentNode))
        {
          if (nodesTouched.Contains(child)) continue;
          nodeQueue.Enqueue(child);
        }
      }
    }
    public void DepthFirstSearch(Action<TNodeType> compute, Func<TNodeType, IEnumerable<TNodeType>> children, Func<bool> stop = null)
    {
      throw new NotImplementedException();
    }

    public bool IsDirectSuccessorOf(TNodeType other)
    {
      return other.Successors.Contains(This);
    }
    public bool IsDirectPredecessorOf(TNodeType other)
    {
      return other.Predecessors.Contains(This);
    }


    /**
     * <summary> Gets the direct neighbors of this node.</summary>
     *
     * <value> The neighbors.</value>
     */
    public IEnumerable<TNodeType> Neighbors
    {
      get
      {
        return Predecessors.Union(Successors);
      }
    }

    /**
     * <summary> Gets this cast to TNodeType. (for convenience)</summary>
     *
     * <value> this.</value>
     */
    public virtual TNodeType This
    {
      get
      {
        return this as TNodeType;
      }
    }

    /**
     * <summary> Adds a successor. </summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="successor"> The successor.</param>
     */
    public virtual void AddSuccessor(TNodeType successor)
    {
      StoreSuccessor(successor);
      successor.StorePredecessor(This);
    }

    /**
     * <summary> Adds a predecessor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="predecessor"> The predecessor.</param>
     */
    public virtual void AddPredecessor(TNodeType predecessor)
    {
      StorePredecessor(predecessor);
      predecessor.StoreSuccessor(This);
    }

    /**
     * <summary> Adds a neighbor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="neighbor"> The neighbor.</param>
     */
    public virtual void AddNeighbor(TNodeType neighbor)
    {
      AddSuccessor(neighbor);
      AddPredecessor(neighbor);
    }

    /**
     * <summary> Removes the neighbor described by neighbor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="neighbor"> The neighbor.</param>
     */
    public virtual void RemoveNeighbor(TNodeType neighbor)
    {
      RemoveSuccessor(neighbor);
      RemovePredecessor(neighbor);
    }

    /**
     * <summary> Removes the neighbors described by neighbors.</summary>
     *
     * <remarks> Tobi, 18.03.2012.</remarks>
     *
     * <param name="neighbors"> The neighbors.</param>
     */
    public virtual void RemoveNeighbors(IEnumerable<TNodeType> neighbors)
    {
      foreach (var neighbor in neighbors)
      {
        RemoveNeighbor(neighbor);
      }
    }

    /**
     * <summary> Removes the successor described by successor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="successor"> The successor.</param>
     */
    public virtual void RemoveSuccessor(TNodeType successor)
    {
      Contract.Assume(successor != null);
      successor.DeletePredecessor(This);
      DeleteSuccessor(successor);
    }

    /**
     * <summary> Removes the predecessor described by predecessor.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="predecessor"> The predecessor.</param>
     */
    public virtual void RemovePredecessor(TNodeType predecessor)
    {
      Contract.Assume(predecessor != null);
      predecessor.DeleteSuccessor(This);
      DeletePredecessor(predecessor);
    }

    /**
     * <summary> Removes all successors.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    public virtual void RemoveAllSuccessors()
    {
      var list = Successors.ToArray();
      foreach (var successor in list)
      {
        RemoveSuccessor(successor);
      }
    }

    /**
     * <summary> Removes all predecessors.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    public virtual void RemoveAllPredecessors()
    {
      var list = Predecessors.ToArray();
      foreach (var predecessor in list)
      {
        RemovePredecessor(predecessor);
      }
    }

    /**
     * <summary> Removes all neighbors.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     */
    public virtual void RemoveAllNeighbors()
    {
      //to array always needs to be called when source collection is changed
      RemoveNeighbors(Neighbors.ToArray());
    }

    /**
     * <summary> Gets the connected nodes.</summary>
     *
     * <value> The connected nodes.</value>
     */
    public IEnumerable<TNodeType> ConnectedNodes
    {
      get
      {
        ICollection<TNodeType> result = CollectionFactory.CreateCollection<TNodeType>();
        GetAllNodes(result);
        return result;
      }
    }

    /**
     * <summary> Gets all predecessors.</summary>
     *
     * <value> all predecessors.</value>
     */
    public IEnumerable<TNodeType> AllPredecessors
    {
      get
      {
        ICollection<TNodeType> result = CollectionFactory.CreateCollection<TNodeType>();
        GetAllPredecessors(result);
        return result;
      }
    }

    /**
     * <summary> Gets all predecessors.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="result"> The result.</param>
     */
    private void GetAllPredecessors(ICollection<TNodeType> result)
    {
      foreach (var predecessor in Predecessors)
      {
        if (!result.Contains(predecessor))
        {
          result.Add(predecessor);
          predecessor.GetAllPredecessors(result);
        }
      }
    }

    /**
     * <summary> Gets all successors.</summary>
     *
     * <value> all successors.</value>
     */
    public IEnumerable<TNodeType> AllSuccessors
    {
      get
      {
        ICollection<TNodeType> result = CollectionFactory.CreateCollection<TNodeType>();
        GetAllSuccesssors(result);
        return result;
      }
    }

    /**
     * <summary> Gets all successsors.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="result"> The result.</param>
     */
    private void GetAllSuccesssors(ICollection<TNodeType> result)
    {
      foreach (var successor in Successors)
      {
        if (!result.Contains(successor))
        {
          result.Add(successor);
          successor.GetAllSuccesssors(result);
        }
      }
    }

    /**
     * <summary> Gets all nodes.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <param name="result"> The result.</param>
     */
    private void GetAllNodes(ICollection<TNodeType> result)
    {
      if (!result.Contains(this))
      {
        result.Add(This);
      }
      else
      {
        return;
      }

      foreach (var neighbor in Neighbors)
      {
        neighbor.GetAllNodes(result);
      }
    }

    /**
     * <summary> Reduces this Node.</summary>
     *
     * <remarks> Tobi, 3/15/2012.</remarks>
     *
     * <exception cref="NotImplementedException"> Thrown when the requested operation is unimplemented.</exception>
     *
     * <typeparam name="T"> Generic type parameter.</typeparam>
     * <param name="reductionFunction"> The reduction function.</param>
     *
     * <returns> .</returns>
     */
    public T Reduce<T>(Func<TNodeType, T, T> reductionFunction)
    {
      throw new NotImplementedException();
    }

  }
}
