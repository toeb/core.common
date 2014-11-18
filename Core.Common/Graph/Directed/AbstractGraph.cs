using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{

  public abstract class AbstractGraph<TNode, TEdge, TGraph>
    : NotifyPropertyChangedBase, IGraph<TNode, TEdge, TGraph>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
    where TGraph : AbstractGraph<TNode, TEdge, TGraph>
  {
    public AbstractGraph() { }

    public IEnumerable<TNode> Nodes
    {
      get { return GetNodes(); }
      set
      {
        ProcessNodes(value);
      }
    }


    public IEnumerable<TEdge> Edges
    {
      get { return GetEdges(); }
      set
      {
        ProcessEdges(value);
      }
    }

    protected virtual void ProcessEdges(IEnumerable<TEdge> newEdges)
    {
      var currentEdges = GetEdges();
      var addEdges = newEdges.Except(currentEdges);
      var removeEdges = currentEdges.Except(newEdges);
      AddEdges(addEdges);
      RemoveEdges(removeEdges);
      RaisePropertyChanged(EdgesName);
    }

    protected virtual void ProcessNodes(IEnumerable<TNode> newNodes)
    {
      var currentNodes = GetNodes();
      var addNodes = newNodes.Except(currentNodes);
      var removeNodes = currentNodes.Except(newNodes);
      AddNodes(addNodes);
      RemoveNodes(removeNodes);
      RaisePropertyChanged(NodesName);
    }

    protected internal abstract IEnumerable<TNode> GetNodes();
    protected internal abstract void AddNodes(IEnumerable<TNode> nodes);
    protected internal abstract void RemoveNodes(IEnumerable<TNode> nodes);

    protected internal abstract void AddEdges(IEnumerable<TEdge> edges);
    protected internal abstract void RemoveEdges(IEnumerable<TEdge> edges);
    protected internal abstract IEnumerable<TEdge> GetEdges();

    private static readonly string EdgesName = "Edges";
    private static readonly string NodesName = "Nodes";
  }

}
