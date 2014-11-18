
using System.Diagnostics;
namespace Core.Store.DirectedGraph
{

  [DebuggerDisplay("e{value} {tail}->{head}")]
  public struct Edge<TNodeKey, TEdgeKey, TEdgeValue> : IIdentifiable<TEdgeKey>
  {
    private TNodeKey head;
    private TNodeKey tail;
    private TEdgeValue value;
    private TEdgeKey id;
    public Edge(TEdgeKey id, TNodeKey tail, TNodeKey head, TEdgeValue value)
    {
      this.head = head;
      this.tail = tail;
      this.value = value;
      this.id = id;
    }
    public TNodeKey Head
    {
      get { return head; }
    }

    public TNodeKey Tail
    {
      get { return tail; }
    }

    public TEdgeValue Value
    {
      get { return value; }
    }


    public TEdgeKey Id
    {
      get { return this.id; }
    }
  }
}
