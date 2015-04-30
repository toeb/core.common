using System;
using System.Collections.Immutable;

namespace Core.Immutable
{

  public delegate EdgeType CreateEdgeDelegate<NodeType, EdgeType>(NodeType tail, NodeType head)
    where NodeType : IEquatable<NodeType>
    where EdgeType : IImmutableEdge<NodeType>;

  public interface IImmutableGraph<NodeType, EdgeType>
    where NodeType : IEquatable<NodeType>
    where EdgeType : IImmutableEdge<NodeType>
  {

    int NodeCount { get; }
    int EdgeCount { get; }
    IImmutableSet<NodeType> Nodes { get; }
    IImmutableSet<EdgeType> Edges { get; }
    CreateEdgeDelegate<NodeType,EdgeType> CreateEdge { get; }
  }

}
