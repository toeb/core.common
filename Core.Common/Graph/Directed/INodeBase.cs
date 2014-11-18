using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{
  
  public interface INodeBase<out TNode, out TEdge>
  {
    /// <summary>
    /// returns all edges connected to the node
    /// </summary>
    IEnumerable<TEdge> Edges
    {
      get;
    }
  }



  
}
