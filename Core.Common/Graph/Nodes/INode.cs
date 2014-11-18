using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Nodes
{


  

  /// <summary>
  /// Base interface for nodes - do not use it use INode instead
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface INodeBase<T>
  {
    ISet<T> Successors { get; }
    ISet<T> Predecessors { get; }
  }

  

  public interface INode<T> : INodeBase<T> where T : INodeBase<T>
  {
  }
}
