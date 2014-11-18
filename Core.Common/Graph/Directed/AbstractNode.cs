using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{


  public abstract class AbstractNode<TNode, TEdge> :
    NotifyPropertyChangedBase,
    INode<TNode, TEdge>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {

    protected TNode Self
    {
      get
      {
        return (TNode)(object)this;
      }
    }
    

    protected internal abstract IEnumerable<TEdge> GetEdges();
    protected internal abstract void AddEdges(IEnumerable<TEdge> edges);
    protected internal abstract void RemoveEdges(IEnumerable<TEdge> edges);
    /// <summary>
    /// must call addedges and removeedges if edges changed it should call RaiseEdgesChanged()
    /// </summary>
    /// <param name="input">all edges the node should be connected to</param>
    /// <returns></returns>
    protected virtual bool ProcessIncommingEdges(IEnumerable<TEdge> input)
    {
      var addEdges = input.Except(GetEdges());
      var removeEdges = GetEdges().Except(input);
      if (!(addEdges.Any() || removeEdges.Any())) return false;
      AddEdges(addEdges);
      RemoveEdges(removeEdges);
      RaiseEdgesChanged();
      return true;
    }

    public IEnumerable<TEdge> Edges
    {
      get
      {
        return GetEdges();
      }
      set
      {
        if (!ProcessIncommingEdges(value)) return;

      }
    }
    protected void RaiseEdgesChanged()
    {
      EdgesChanged();
      RaisePropertyChanged(EdgesName);
    }
    private static readonly string EdgesName = "Edges";




    protected virtual void EdgesChanged() { }

  }
}