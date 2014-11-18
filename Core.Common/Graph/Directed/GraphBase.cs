using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{

  public class GraphBase<TNode, TEdge, TGraph> : AbstractGraph<TNode, TEdge, TGraph>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
    where TGraph : GraphBase<TNode, TEdge, TGraph>
  {
    ICollection<TNode> nodes = new List<TNode>();
    ICollection<TEdge> edges = new List<TEdge>();
    protected internal override IEnumerable<TNode> GetNodes()
    {
      return nodes;
    }

    protected internal override void AddNodes(IEnumerable<TNode> nodes)
    {
      foreach (var node in nodes) this.nodes.Add(node);
      NodesAdded(nodes);
    }


    protected internal override void RemoveNodes(IEnumerable<TNode> nodes)
    {
      foreach (var node in nodes) this.nodes.Remove(node);
      NodesRemoved(nodes);
    }


    protected internal override void AddEdges(IEnumerable<TEdge> edges)
    {
      foreach (var edge in edges) this.edges.Add(edge);
      EdgesAdded(edges);

    }


    protected internal override void RemoveEdges(IEnumerable<TEdge> edges)
    {
      foreach (var edge in edges) this.edges.Remove(edge);
      EdgesRemove(edges);
    }


    protected internal override IEnumerable<TEdge> GetEdges()
    {
      return edges;
    }

    #region Extensionpoints

    protected void NodesAdded(IEnumerable<TNode> nodes) { }
    protected void NodesRemoved(IEnumerable<TNode> nodes) { }
    protected void EdgesAdded(IEnumerable<TEdge> edges) { }
    protected virtual void EdgesRemove(IEnumerable<TEdge> edges) { }
    #endregion
  }
}
