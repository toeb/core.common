using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{

  public class NodeBase<TNode, TEdge> : AbstractNode<TNode, TEdge>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    private ISet<TEdge> edges = new HashSet<TEdge>();

    protected internal override void AddEdges(IEnumerable<TEdge> edges)
    {
      this.edges.UnionWith(edges);
      OnEdgesAdded(edges);
    }
    protected internal override void RemoveEdges(IEnumerable<TEdge> edges)
    {
      this.edges.ExceptWith(edges);
      OnEdgesRemoved(edges);
    }
    protected internal override IEnumerable<TEdge> GetEdges()
    {
      return edges;
    }

    protected virtual void OnEdgesAdded(IEnumerable<TEdge> edges) { }
    protected virtual void OnEdgesRemoved(IEnumerable<TEdge> edges) { }
  }
}