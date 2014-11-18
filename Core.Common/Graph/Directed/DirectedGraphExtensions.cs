using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{
  /// <summary>
  /// represents a node which is visited coming from the specified edge
  /// 
  /// @todo rename to NodeVector?
  /// </summary>
  /// <typeparam name="TNode"></typeparam>
  /// <typeparam name="TEdge"></typeparam>
  [DebuggerDisplay("en {Edge}->{Node}")]
  public struct EdgeNode<TNode, TEdge>
  {
    public EdgeNode(TNode node, TEdge edge) { this.Edge = edge; this.Node = node; }
    /// <summary>
    /// the edge over which node is visited
    /// </summary>
    public TEdge Edge;
    /// <summary>
    /// the visited node
    /// </summary>
    public TNode Node;
    public override bool Equals(object obj)
    {
      if (!(obj is EdgeNode<TNode, TEdge>)) return false;
      if (object.ReferenceEquals(this, obj)) return true;
      var other = (EdgeNode<TNode, TEdge>)obj;
      return Node.Equals(other.Node);
    }
    public override int GetHashCode()
    {
        return Node.GetHashCode();
    }
  }
  public static class DirectedGraphExtensions
  {
    /// <summary>
    /// returns all directly suceeding nodes
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> Successors<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return node.GetOutgoingEdges().Select(edge=>edge.Head);
    }
    /// <summary>
    /// returns all directly preceeding nodes
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> Predecessors<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return node.GetIncomingEdges().Select(edge => edge.Tail);
    }
    /// <summary>
    /// returns all neighboring nodes (duplicates and the node itself may be contained)
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> Neighbor<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return node.Edges.Select(edge => edge.Head .Equals( node )? edge.Tail : edge.Head);
    }


    /// <summary>
    /// a helper method which casts the spcified INode to its TNode type
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static TNode Normalise<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return (TNode)node;
    }
    /// <summary>
    /// returns all edges of node where it is the tail
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TEdge> GetOutgoingEdges<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return node.Normalise().Edges.Where(edge => edge.Tail.Equals(node));
    }
    /// <summary>
    /// returns all edges of the node where it is the head
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TEdge> GetIncomingEdges<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return node.Normalise().Edges.Where(edge => edge.Head.Equals(node));
    }
    /// <summary>
    /// returns all heads from an enumerable of edges
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edges"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> Heads<TNode, TEdge>(this IEnumerable<IEdge<TNode,TEdge>> edges)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return edges.Select(edge => edge.Head);
    }
    /// <summary>
    /// returns all tails of an enumerable of edges
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edges"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> Tails<TNode, TEdge>(this IEnumerable<TEdge> edges)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return edges.Select(edge => edge.Tail);
    }
    /// <summary>
    /// returns all edges which correspond to the specified direction
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static IEnumerable<TEdge> GetEdges<TNode, TEdge>(this INode<TNode, TEdge> node, EdgeDirection direction)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      switch (direction)
      {
        case EdgeDirection.Succeeding: return node.GetOutgoingEdges();
        case EdgeDirection.Preceeding: return node.GetIncomingEdges();
        default: return node.Edges;
      }
    }
    /// <summary>
    /// creates an edge node for the head
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edge"></param>
    /// <returns></returns>
    public static EdgeNode<TNode, TEdge> HeadEdgeNode<TNode, TEdge>(this IEdge<TNode, TEdge> edge)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return new EdgeNode<TNode, TEdge>(edge.Head, (TEdge)edge);
    }
    /// <summary>
    /// returns an edgenode to the tail node
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edge"></param>
    /// <returns></returns>
    public static EdgeNode<TNode, TEdge> TailEdgeNode<TNode, TEdge>(this IEdge<TNode, TEdge> edge)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return new EdgeNode<TNode, TEdge>(edge.Tail,(TEdge) edge);
    }
    /// <summary>
    /// returns the root edgenode which has no edge specified
    /// this is useful for starting points of algorithm
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static EdgeNode<TNode, TEdge> Root<TNode, TEdge>(this  INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return new EdgeNode<TNode, TEdge>((TNode)node, default(TEdge));
    }
    /// <summary>
    /// returns the neighbor dfs order
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> DfsNeighborNodeOrder<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return GraphExtensions.DfsOrder((TNode)node, n => n.Edges.SelectMany(e => Combine(e.Head, e.Tail)));
    }
    /// <summary>
    /// returns the bfs neigbhor order
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> BfsNeighborNodeOrder<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return GraphExtensions.BfsOrder((TNode)node, n => n.Edges.SelectMany(e => Combine(e.Head, e.Tail)));
    }
    /// <summary>
    /// returns the forward bfs order of edge nodes
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<EdgeNode<TNode, TEdge>> BfsForwardOrder<TNode, TEdge>(this INode<TNode, TEdge> node)
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return GraphExtensions.BfsOrder(
        node.Root(), en => en.Node.Edges.Except(en.Edge.ToEnumerable()).Select(e => e.HeadEdgeNode()));
    }

    private static IEnumerable<T> ToEnumerable<T>(this T t) { yield return t; }
    private static IEnumerable<T> Combine<T>(T a, T b)
    {
      yield return a;
      yield return b;
    }
  }
}
