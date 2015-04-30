using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
namespace Core.Immutable
{
  public static class ImmutableGraph
  {
    public static IImmutableGraph<NodeType, EdgeType> WithoutEdges<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, Func<EdgeType,bool> predicate)
      where NodeType: IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.WithoutEdges(self.EdgesWhere(predicate));
    }
    public static IImmutableGraph<NodeType, EdgeType> WithoutEdges<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, IEnumerable<EdgeType> edges)
      where NodeType: IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      foreach (var edge in edges)
      {
        self = self.WithoutEdge(edge);
      }
      return self;
    }
    public static IImmutableGraph<NodeType, EdgeType> WithoutNodes<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, IEnumerable<NodeType> nodes)
      where NodeType: IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      foreach (var node in nodes)
      {
        self = self.WithoutNode(node);
      }
      return self;
    }
    
     public static IImmutableGraph<NodeType, EdgeType> WithoutNodes<NodeType, EdgeType>(this IImmutableGraph<NodeType, EdgeType> self, Func<NodeType,bool> predicate)
      where NodeType: IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.WithoutNodes(self.NodesWhere(predicate));
    }
    
    public static IImmutableGraph<NodeType, IImmutableEdge<NodeType>> Create<NodeType>(params NodeType[] nodes) 
      where NodeType : IEquatable<NodeType>
    {
      return new ImmutableGraphImplementation<NodeType,IImmutableEdge<NodeType>>(
        ImmutableHashSet.Create(nodes), 
        ImmutableHashSet.Create<IImmutableEdge<NodeType>>(),
        (tail,head)=>new Edge<NodeType>(tail,head)
        );
    }
    public static IImmutableGraph<NodeType, EdgeType> 
      Create<NodeType,EdgeType>(CreateEdgeDelegate<NodeType, EdgeType> createEdge, params NodeType[] nodes)
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {

      return new ImmutableGraphImplementation<NodeType, EdgeType>(
        ImmutableHashSet.Create(nodes),
        ImmutableHashSet.Create<EdgeType>(),
        createEdge
        );
    }
  
    public static IImmutableGraph<NodeType,EdgeType> WithNode<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return new ImmutableGraphImplementation<NodeType, EdgeType>(self.Nodes.Add(node), self.Edges,self.CreateEdge);
    }
    public static IImmutableGraph<NodeType,EdgeType> WithoutNode<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType node) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
  
      var edges = self.GetIncedentEdges(node);
      foreach (var edge in edges) self = self.WithoutEdge(edge);
      return new ImmutableGraphImplementation<NodeType,EdgeType>(self.Nodes.Remove(node), self.Edges,self.CreateEdge);
    }
  
    public static IImmutableGraph<NodeType,EdgeType> WithEdge<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType tail, NodeType head) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.WithEdge(self.CreateEdge(tail,head));
    }
    public static IImmutableGraph<NodeType,EdgeType> WithEdge<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, EdgeType edge) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      if (!self.Nodes.Contains(edge.Tail)) self = self.WithNode(edge.Tail);
      if (!self.Nodes.Contains(edge.Head)) self = self.WithNode(edge.Head);
      return new ImmutableGraphImplementation<NodeType,EdgeType>(self.Nodes, self.Edges.Add(edge),self.CreateEdge);
  
    }
    public static IImmutableGraph<NodeType,EdgeType> WithoutEdge<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, NodeType tail, NodeType head) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return self.WithoutEdge(self.CreateEdge(tail, head));
    }
    public static IImmutableGraph<NodeType,EdgeType> WithoutEdge<NodeType,EdgeType>(this IImmutableGraph<NodeType,EdgeType> self, EdgeType edge) 
      where NodeType : IEquatable<NodeType>
      where EdgeType : IImmutableEdge<NodeType>
    {
      return new ImmutableGraphImplementation<NodeType,EdgeType>(self.Nodes, self.Edges.Remove(edge),self.CreateEdge);
    }
  }
}
