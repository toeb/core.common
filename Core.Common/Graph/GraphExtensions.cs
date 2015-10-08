using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Collections;
using Core.Extensions;

namespace Core.Graph
{

  public static class GraphExtensions
  {
    /// <summary>
    /// Implements the Tarjan 1976 depth first topological sort algorithm (recursive)
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nodes"></param>
    /// <param name="expand"></param>
    /// <returns></returns>
    public static IEnumerable<T> TopSort<T>(this IEnumerable<T> rootNodes, Func<T, IEnumerable<T>> expand)
    {
      ISet<T> visited = new HashSet<T>();
      ISet<T> visiting = new HashSet<T>();
      List<T> L = new List<T>();
      Func<T, bool> visit = null;
      visit = n =>
      {
        if (visited.Contains(n)) return true;
        if (!visiting.Add(n)) return false;
        if (!visited.Contains(n))
        {
          foreach (var m in expand(n))
          {
            if (!visit(m)) return false;
          }
          visited.Add(n);
          visiting.Remove(n);
          L.Add(n);
        }
        return true;
      };

      foreach (var n in rootNodes)
      {
        if (!visit(n)) return null;
      }
      return L;
    }

    public static T FirstCommonAncestor<T>(this T a, T b, Func<T, IEnumerable<T>> ancestors)
    {
      var bfsA = a.BfsOrder(ancestors);
      var bfsB = b.BfsOrder(ancestors);

      var visited = new Marker();
      foreach (var it in bfsA) visited[it] = true;
      foreach (var it in bfsB)
      {
        if (visited[it]) return it;
      }
      return default(T);

    }
    public static IEnumerable<T> TraverseOrder<T>(this IEnumerable<T> v0, Func<T, IEnumerable<T>> expand, Func<T> pop, Action<T> push, Func<bool> empty)
    {
      //used to mark if an element was visited
      var visited = new Marker();
      // add root element
      foreach(var v in v0)
      {
        push(v);

      }
      while (true)
      {
        // stop when container is empty
        if (empty()) yield break;
        // get the first element of the container
        var current = pop();
        // if element is visited return
        if (visited[current]) continue;
        // visit element
        visited[current] = true;
        // yield the element
        yield return current;
        // expand the element
        var successors = expand(current);
        // add all successors to the container
        foreach (var successor in successors)
        {
          push(successor);
        }
      }
    }
    /// <summary>
    /// Default Traversal algorithm
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    /// <param name="root">the root element from where traversal starts</param>
    /// <param name="expand">a function that is called when an element is touched for the first time, it needs to return the successors (in algorithmic context) of the input element</param>
    /// <param name="pop">removes an element from the container</param>
    /// <param name="push">adds an element from to the container</param>
    /// <param name="empty">checks if the container is empty</param>
    /// <returns></returns>
    public static IEnumerable<T> TraverseOrder<T>(this T root, Func<T, IEnumerable<T>> expand, Func<T> pop, Action<T> push, Func<bool> empty)
    {
      //used to mark if an element was visited
      var visited = new Marker();
      // add root element
      push(root);
      while (true)
      {
        // stop when container is empty
        if (empty()) yield break;
        // get the first element of the container
        var current = pop();
        // if element is visited return
        if (visited[current]) continue;
        // visit element
        visited[current] = true;
        // yield the element
        yield return current;
        // expand the element
        var successors = expand(current);
        // add all successors to the container
        foreach (var successor in successors)
        {
          push(successor);
        }
      }
    }


    public static void Traverse<T>(this T root, Func<T, IEnumerable<T>> expand, Func<T> pop, Action<T> push, Func<bool> empty)
    {
      foreach (var item in root.TraverseOrder(expand, pop, push, empty)) { /*do nothing*/}
    }
    public static IEnumerable<TContext> TraverseOrderWithContext<T, TContext>(
      this T root,
      Func<T, IEnumerable<T>> expand,
      Func<T> pop,
      Action<T> push,
      Func<bool> empty,
      Func<T, TContext, TContext> select,
      TContext initialContext = default(TContext)
      )
    {
      var visited = new Marker();
      var contexts = new Marker<TContext>();
      contexts[root] = select(root, initialContext);
      push(root);

      while (true)
      {
        if (empty()) yield break;
        var current = pop();
        if (visited[current]) continue;
        visited[current] = true;
        yield return contexts[current];
        var successors = expand(current);
        foreach (var successor in successors)
        {
          contexts[successor] = select(successor, contexts[current]);
          push(successor);
        }
      }
    }
    public static IEnumerable<T> DfsOrder<T>(this IEnumerable<T> v0, Func<T,IEnumerable<T>> expand)
    {
      var stack = new Stack<T>();
      return TraverseOrder<T>(v0, expand, stack.Pop, stack.Push, stack.None);
    }

    public static IEnumerable<T> DfsOrder<T>(this T root, Func<T, IEnumerable<T>> successors)
    {
      var stack = new Stack<T>();
      return TraverseOrder(root, successors, stack.Pop, stack.Push, stack.None);
    }
    public static IEnumerable<TContext> DfsOrderWithContext<T, TContext>(this T root, Func<T, IEnumerable<T>> successors, Func<T, TContext, TContext> select)
    {
      var stack = new Stack<T>();
      return TraverseOrderWithContext(root, successors, stack.Pop, stack.Push, stack.None, select);
    }
    public static IEnumerable<T> BfsOrder<T>(this T root, Func<T, IEnumerable<T>> successors)
    {
      var queue = new Queue<T>();
      return TraverseOrder(root, successors, queue.Dequeue, queue.Enqueue, queue.None);
    }

    public static IEnumerable<TContext> BfsOrderWithContext<T, TContext>(this T root, Func<T, IEnumerable<T>> successors, Func<T, TContext, TContext> select)
    {
      var queue = new Queue<T>();
      return TraverseOrderWithContext(root, successors, queue.Dequeue, queue.Enqueue, queue.None, select);
    }

    public static IEnumerable<TContext> BfsOrderWithContext<T, TContext>(this T root, TContext initialContext, Func<T, IEnumerable<T>> successors, Func<T, TContext, TContext> select)
    {
      var queue = new Queue<T>();
      return TraverseOrderWithContext(root, successors, queue.Dequeue, queue.Enqueue, queue.None, select, initialContext);
    }
    public static bool ContainsCycle<T>(this IEnumerable<T> nodes, Func<T, IEnumerable<T>> successors)
    {
      return nodes.PartialOrder(successors) == null;
    }
    public static bool ContainsCycle<T>(this T node, Func<T, IEnumerable<T>> successors)
    {
      return node.PartialOrder(successors) == null;
    }
    public static IEnumerable<T> PartialOrder<T>(this T node, Func<T, IEnumerable<T>> successors)
    {
      return new T[] { node }.PartialOrder(successors);
    }
    /// <summary>
    /// returns the nodes the partial order for passed nodes and successor function 
    /// (dependency order, a nodes is added when all successors were already added)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nodes"></param>
    /// <param name="successors"></param>
    /// <returns></returns>
    public static IEnumerable<T> PartialOrder<T>(this IEnumerable<T> nodes, Func<T, IEnumerable<T>> successors)
    {
      var allNodes = new HashSet<T>(nodes.SelectMany(node => node.BfsOrder(current => successors(current))));


      // resulting list
      var L = new List<T>();

      var temporaryMark = new Marker();
      var marked = new Marker();
      Func<T, bool> visit = null;
      visit = n =>
      {
        // cycle detected
        if (temporaryMark[n]) return false;
        if (!marked[n])
        {
          temporaryMark[n] = true;
          foreach (var m in successors(n))
          {
            if (!visit(m)) return false;
          }
          temporaryMark[n] = false;
          marked[n] = true;
          L.Add(n);
        }
        return true;

      };

      while (true)
      {
        //select unmarked node
        var current = allNodes.Where(n => !marked[n]);
        if (!current.Any()) return L;

        var noCycle = visit(current.First());
        if (!noCycle) return null;

      }



    }
  }
}