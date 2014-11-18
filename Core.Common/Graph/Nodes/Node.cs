using Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Nodes
{

  public class Node<T> : INode<T> where T : INode<T>
  {
    private ObservableSet<T, OrderedSet<T>> predecessors;
    private ObservableSet<T, OrderedSet<T>> successors;


    public ISet<T> Successors { get { return successors; } }
    public ISet<T> Predecessors { get { return predecessors; } }
        

    public Node()
    {
      predecessors = new ObservableSet<T, OrderedSet<T>>();
      successors = new ObservableSet<T, OrderedSet<T>>();
      // watch the successors and predecessors sets
      predecessors.CollectionChanged += predecessorsChanged;
      successors.CollectionChanged += successorsChanged;
    }

    /// <summary>
    /// listener for collection changed events of successors
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void successorsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {

      if (e.NewItems != null)
      {
        foreach (var added in e.NewItems)
        {
          SuccessorAdded((T)added);
        }
      }
      if (e.OldItems != null)
      {
        foreach (var removed in e.OldItems)
        {
          SuccessorRemoved((T)removed);
        }
      }
    }

    private void predecessorsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        foreach (var added in e.NewItems)
        {
          object it = added;

          PredecessorAdded((T)added);
        }
      }
      if (e.OldItems != null)
      {
        foreach (var removed in e.OldItems)
        {
          PredecessorRemoved((T)removed);
        }
      }
    }

    /// <summary>
    /// returns a reference to this object cast t T
    /// </summary>
    public T Self
    {
      get
      {
        return (T)((object)this);
      }
    }

    /// <summary>
    /// ensures that the removed elements successors are consistent
    /// </summary>
    /// <param name="removed"></param>
    private void PredecessorRemoved(T removed)
    {
      removed.Successors.Remove(Self);
      OnPredecessorRemoved(removed);
    }

    private void PredecessorAdded(T added)
    {
      added.Successors.Add(Self);
      OnPredecessorAdded(added);
    }

    private void SuccessorRemoved(T removed)
    {
      removed.Predecessors.Remove(Self);
      OnSuccessorRemoved(removed);
    }

    private void SuccessorAdded(T added)
    {
      added.Predecessors.Add(Self);
      OnSuccessorAdded(added);
    }

    /// <summary>
    /// Extension point called when a successor was added (this is only called once per added successor)
    /// </summary>
    /// <param name="added"></param>
    protected virtual void OnSuccessorAdded(T added) { }
    /// <summary>
    /// Extension point called when a predecessor was added (this is only called once per added successor)
    /// </summary>
    /// <param name="added"></param>
    protected virtual void OnPredecessorAdded(T added) { }
    /// <summary>
    /// Extension point called when a successor was removed (this is only called once per removed successor)
    /// </summary>
    /// <param name="added"></param>
    protected virtual void OnSuccessorRemoved(T removed) { }
    /// <summary>
    /// Extension point called when a predecessor was removed (this is only called once per removed predecessor)
    /// </summary>
    /// <param name="added"></param>
    protected virtual void OnPredecessorRemoved(T removed) { }



    ~Node()
    {
      predecessors.CollectionChanged -= predecessorsChanged;
      successors.CollectionChanged -= successorsChanged;
    }
  }
}
