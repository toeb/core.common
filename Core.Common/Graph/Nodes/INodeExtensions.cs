using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Collections;
using Core.Extensions;
namespace Core.Graph.Nodes
{
  public static class INodeExtensions
  {
    #region Dfs Order
    public static IEnumerable<T> DfsOrder<T>(this T root) where T : INode<T>
    {
      return GraphExtensions.DfsOrder(root, r => r.Successors);
    }

    public static IEnumerable<TContext> DfsOrderWithContext<T, TContext>(this T rootElement, Func<T, TContext, TContext> select) where T : INode<T>
    {
      return GraphExtensions.DfsOrderWithContext<T, TContext>(rootElement, c => c.Successors, select);
    }
    public static IEnumerable<TraversalContext<T>> DfsOrderWithContext<T>(this T rootElement) where T : INode<T>
    {
      return DfsOrderWithContext<T, TraversalContext<T>>(rootElement, TraversalContext<T>.Create);
    }
    public static IEnumerable<T> DfsNeigbhors<T>(this T root) where T : INode<T>
    {
      return GraphExtensions.DfsOrder(root, r => r.Successors.Concat(r.Predecessors));
    }

    #endregion
    #region Bfs Order
    public static IEnumerable<T> BfsOrder<T>(this T root) where T : INode<T>
    {
      return GraphExtensions.BfsOrder(root, r => r.Successors);
    }
    public static IEnumerable<T> BfsNeighbors<T>(this T root) where T : INode<T>
    {
      return GraphExtensions.BfsOrder(root, r => r.Successors.Concat(r.Predecessors));
    }
    #endregion
    public static void Disconnect<T>(this T node) where T : INode<T>
    {
      node.Successors.Clear();
      node.Predecessors.Clear();
    }
  }
}
