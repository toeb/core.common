using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{


  public interface IGraphBase<TNode, TEdge, TGraph>
    where TNode : INodeBase<TNode, TEdge>
    where TEdge : IEdgeBase<TNode, TEdge>
  {

    IEnumerable<TNode> Nodes { get; set; }
    IEnumerable<TEdge> Edges { get; set; }
  }
}
