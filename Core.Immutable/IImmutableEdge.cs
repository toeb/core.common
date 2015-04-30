using System;

namespace Core.Immutable
{
  public interface IImmutableEdge<NodeType> : IEquatable<IImmutableEdge<NodeType>>
  {
    NodeType Head { get; }
    NodeType Tail { get; }
  }
}
