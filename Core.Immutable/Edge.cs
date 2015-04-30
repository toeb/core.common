using System;

namespace Core.Immutable
{
  public class Edge<NodeType> : IImmutableEdge<NodeType> where NodeType : IEquatable<NodeType>
  {
    private NodeType head;
    private NodeType tail;
    public Edge(NodeType tail, NodeType head)
    {
      this.tail = tail;
      this.head = head;
    }
    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      //if (!obj.GetType().ImplementsInterface<IImmutableEdge<NodeType>>()) return false;
      return this.Equals(obj as IImmutableEdge<NodeType>);
    }
    public static bool operator ==(Edge<NodeType> lhs, Edge<NodeType> rhs)
    {
      return lhs.head.Equals(rhs.head) && lhs.tail.Equals(rhs.tail);
    }
    public static bool operator !=(Edge<NodeType> lhs, Edge<NodeType> rhs)
    {
      return !(lhs == rhs);
    }
    public override int GetHashCode()
    {
      return Head.GetHashCode() ^ Tail.GetHashCode();
    }
    public NodeType Head { get { return head; } }
    public NodeType Tail { get { return tail; } }
  
  
    public bool Equals(IImmutableEdge<NodeType> other)
    {
      if (other == null) return false;
      return this.Head.Equals(other.Head) && this.Tail.Equals(other.Tail);
    }
  }
}
