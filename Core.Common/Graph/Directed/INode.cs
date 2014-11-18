using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{
  public interface INode<out TNode, out TEdge> : INodeBase<TNode, TEdge>
    where TNode : INodeBase<TNode, TEdge>
    where TEdge : IEdgeBase<TNode, TEdge>
  {

  }

  public enum EdgeDirection
  {
    Succeeding,
    Preceeding,
    Neighboring
  }


}
