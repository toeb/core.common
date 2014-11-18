using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
namespace Core.Graph.Directed
{

  public class EdgeBase<TNode, TEdge> : AbstractEdge<TNode, TEdge>
    where TNode : AbstractNode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    protected override void ConnectionChanged()
    {
      if (Head == null && Tail != null)
      {
        Tail.RemoveEdges(Self.MakeEnumerable());
      }
      if (Tail == null && Head != null)
      {
        Head.RemoveEdges(Self.MakeEnumerable());
      }
      if (Tail != null && Head != null)
      {
        Head.AddEdges(Self.MakeEnumerable());
        Tail.AddEdges(Self.MakeEnumerable());
      }
    }


    private TNode head;
    private TNode tail;

    protected override void StoreHead(TNode head)
    {
      this.head = head;
    }

    protected override TNode RetrieveHead()
    {
      return head;
    }

    protected override void StoreTail(TNode tail)
    {
      this.tail = tail;
    }

    protected override TNode RetrieveTail()
    {
      return tail;
    }
  }
}
