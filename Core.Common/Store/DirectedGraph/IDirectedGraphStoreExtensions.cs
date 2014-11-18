using System;
using System.Linq;

namespace Core.Store.DirectedGraph
{
  public static class IDirectedGraphStoreExtensions
  {
    /// <summary>
    /// returns all outgoing edges for the specified node
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static IQueryable<Edge<TNodeKey, TEdgeKey, TEdgeValue>> GetOutgoingEdges<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IEdgeStore<TNodeKey, TEdgeKey, TEdgeValue> self, TNodeKey node)
    {
      return self.GetEdges().Where(edge => edge.Equals(node));
    }
    /// <summary>
    /// returns all incoming edges for node
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static IQueryable<Edge<TNodeKey, TEdgeKey, TEdgeValue>> GetIncomingEdges<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IEdgeStore<TNodeKey, TEdgeKey, TEdgeValue> self, TNodeKey node)
    {
      return self.GetEdges().Where(edge => edge.Equals(node));
    }


    /// <summary>
    /// Stores a new node connected to existingNode by an edge identified by edgeid
    /// direction may be reversed causing this method to create a preceeding node instead of a  succeeding one
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="existingNode"></param>
    /// <param name="newNode"></param>
    /// <param name="edgeId"></param>
    /// <param name="nodeValue"></param>
    /// <param name="edgeValue"></param>
    /// <param name="direction"></param>
    /// <returns>the newly created edge</returns>
    public static Edge<TNodeKey, TEdgeKey, TEdgeValue> 
      StoreNode<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(
      this IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> self,
      TNodeKey existingNode,
      TNodeKey newNode,
      TEdgeKey edgeId,
      TNodeValue nodeValue = default(TNodeValue),
      TEdgeValue edgeValue = default(TEdgeValue),
      NodePosition direction = NodePosition.Successor
      )
    {
      var tail = existingNode;
      var head = newNode;
      if (direction == NodePosition.Predecessor)
      {
        var temp = tail;
        tail = head;
        head = temp;
      }
      self.StoreNode(newNode, nodeValue);
      var edge = new Edge<TNodeKey, TEdgeKey, TEdgeValue>(edgeId, tail, head, edgeValue);
      self.StoreEdge(edge);
      return edge;
    }
    /// <summary>
    /// creates a node for the specified node store
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Node<TNodeKey, TNodeValue> MakeNode<TNodeKey, TNodeValue>(this INodeStore<TNodeKey, TNodeValue> self, TNodeKey key, TNodeValue value = default(TNodeValue))
    {
      var node = new Node<TNodeKey, TNodeValue>(key, value);
      return node;
    }
    /// <summary>
    /// create an edge for the specified edgestore
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <param name="tail"></param>
    /// <param name="head"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Edge<TNodeKey, TEdgeKey, TEdgeValue> MakeEdge<TNodeKey, TEdgeKey, TEdgeValue>(this IEdgeStore<TNodeKey, TEdgeKey, TEdgeValue> self, TEdgeKey id, TNodeKey tail, TNodeKey head, TEdgeValue value = default(TEdgeValue))
    {
      var edge = new Edge<TNodeKey, TEdgeKey, TEdgeValue>(id, head, tail, value);
      return edge;
    }
    /// <summary>
    /// stores an edge in the specified edgestore
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="edgeId"></param>
    /// <param name="tail"></param>
    /// <param name="head"></param>
    /// <param name="edgeValue"></param>
    /// <returns></returns>
    public static Edge<TNodeKey, TEdgeKey, TEdgeValue> StoreEdge<TNodeKey, TEdgeKey, TEdgeValue>(this IEdgeStore<TNodeKey, TEdgeKey, TEdgeValue> self, TEdgeKey edgeId, TNodeKey tail, TNodeKey head, TEdgeValue edgeValue)
    {
      var edge = self.MakeEdge(edgeId, tail, head, edgeValue);
      self.StoreEdge(edge);
      return edge;
    }
    /// <summary>
    /// stores a node in the graph store
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Node<TNodeKey, TNodeValue> StoreNode<TNodeKey, TNodeValue>(this INodeStore<TNodeKey, TNodeValue> self, TNodeKey id, TNodeValue value)
    {
      var node = self.MakeNode(id, value);
      self.StoreNode(node);
      return node;
    }
    /// <summary>
    /// returns the node identified by Id or null if it is not found
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Node<TNodeKey, TNodeValue> GetById<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> self, TNodeKey id) where TNodeKey : IEquatable<TNodeKey>
    {
      return self.GetNodes().SingleOrDefault(it => it.Id.Equals(id));
    }

    /// <summary>
    /// returns the number of edges that point towards the node
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static int IncidentToCount<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> self, TNodeKey node)
       where TNodeKey : IEquatable<TNodeKey>
    {
      return self.GetEdges().Count(t => t.Head.Equals(node));
    }

    /// <summary>
    /// returns the number of edges that originate form the node
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static int IncidentFromCount<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> self, TNodeKey node)
       where TNodeKey : IEquatable<TNodeKey>
    {
      return self.GetEdges().Count(t => t.Tail.Equals(node));
    }
    /// <summary>
    /// retruns the order of the node ( number of copnnected edges)
    /// </summary>
    /// <typeparam name="TNodeKey"></typeparam>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeKey"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static int OrderCount<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>(this IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> self, TNodeKey node)
       where TNodeKey : IEquatable<TNodeKey>
    {
      return self.IncidentFromCount(node) + self.IncidentToCount(node);
    }
  }
}
