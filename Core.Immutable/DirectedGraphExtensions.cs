using System;
using System.Collections.Generic;
using System.Linq;
namespace Core.Immutable
{
  public static class DirectedGraphExtensions
  {
    public static IEnumerable<EdgeType> EdgesWhere<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, Func<EdgeType, bool> predicate)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.Edges.Where(predicate);
    }
    public static IEnumerable<NodeType> NodesWhere<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, Func<NodeType, bool> predicate)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.Nodes.Where(predicate);
    }

    public static IEnumerable<TResult> EdgesSelect<NodeType, EdgeType, TResult>(this IImmutableGraph<NodeType, EdgeType> self, Func<EdgeType, TResult> selector)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.Edges.Select(selector);
    }

    public static IEnumerable<TResult> NodesSelect<NodeType, EdgeType, TResult>(this IImmutableGraph<NodeType, EdgeType> self, Func<NodeType, TResult> selector)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.Nodes.Select(selector);
    }


    
    
    
    
    /// <summary>
    /// returns the set of neighbors of the specified graph
    /// </summary>
    /// <typeparam name="NodeType"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<NodeType> GetNeighbors<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.GetIncedentEdges(node)
        .Select(edge => edge.Head.Equals(node) ? edge.Tail : edge.Head)
        .Distinct();
    }
    /// <summary>
    /// Returns the list (duplicates possible) of all predecessors of the specified graph and node. 
    /// </summary>
    /// <typeparam name="NodeType"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IEnumerable<NodeType> GetPredecessors<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.GetIncommingEdges(node)
        .Select(edge => edge.Tail);
    }
    public static IEnumerable<NodeType> GetSuccessors<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.GetOutgoingEdges(node)
        .Select(edge => edge.Head);
    }
    public static IEnumerable<EdgeType> GetIncedentEdges<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node)
      where EdgeType : IImmutableEdge<NodeType>
      where NodeType : IEquatable<NodeType>
    {
      return self.Edges
        .Where(edge => edge.Tail.Equals(node) || edge.Head.Equals(node));
    }
    public static IEnumerable<EdgeType> GetOutgoingEdges<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node)
      where EdgeType : IImmutableEdge<NodeType>
      where NodeType : IEquatable<NodeType>
    {
      return self.Edges
        .Where(edge => edge.Tail.Equals(node));
    }
    public static IEnumerable<EdgeType> GetIncommingEdges<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node)
      where EdgeType : IImmutableEdge<NodeType>
      where NodeType : IEquatable<NodeType>
    {
      return self.Edges
        .Where(edge => edge.Head.Equals(node));
    }
  
  
  }
}
