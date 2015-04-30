using System;
using System.Collections.Immutable;

namespace Core.Immutable
{
  class ImmutableGraphImplementation<NodeType, EdgeType> 
    : IImmutableGraph<NodeType, EdgeType> 
    where NodeType : IEquatable<NodeType>
    where EdgeType : IImmutableEdge<NodeType>
  {
  
    private IImmutableSet<NodeType> nodes;
    private IImmutableSet<EdgeType> edges;
    private CreateEdgeDelegate<NodeType,EdgeType> createEdge;

    public ImmutableGraphImplementation(
      IImmutableSet<NodeType> nodes, IImmutableSet<EdgeType> edges, CreateEdgeDelegate<NodeType,EdgeType> createEdge)
    {
      this.nodes = nodes;
      this.edges = edges;
      this.createEdge = createEdge;
    }
  
  
  
    public int NodeCount
    {
      get { return nodes.Count; }
    }
  
    public int EdgeCount
    {
      get { return edges.Count; }
    }
  
  
    public IImmutableSet<NodeType> Nodes
    {
      get { return nodes; }
    }
  
    public IImmutableSet<EdgeType> Edges
    {
      get { return edges; }
    }





    public CreateEdgeDelegate<NodeType, EdgeType> CreateEdge
    {
      get { return createEdge; }
    }
  }
}
