using System;

namespace Core.Store.DirectedGraph
{
  public enum NodePosition
  {
    Successor,
    Predecessor
  }
  public static class IDirectedGraphStoreGuid
  {
    private static Guid MakeId() { return Guid.NewGuid(); }
    public static Guid StoreNode<TNodeValue, TEdgeValue>(this IDirectedGraphStore<Guid, TNodeValue, Guid, TEdgeValue> self, TNodeValue value = default(TNodeValue))
    {
      var id = MakeId();
      self.StoreConnectedNode(id, value);
      return id;
    }
    public static Guid StoreEdge<TEdgeValue, TNodeKey, TNodeValue>(this IDirectedGraphStore<TNodeKey, TNodeValue, Guid, TEdgeValue> self, TNodeKey tail, TNodeKey head, TEdgeValue value = default(TEdgeValue))
    {
      Guid id = MakeId();
      self.StoreEdge(id, tail, head, value);
      return id;
    }
    /// <summary>
    /// stores a node connected to existing node via an edge and returns it
    /// </summary>
    /// <typeparam name="TNodeValue"></typeparam>
    /// <typeparam name="TEdgeValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="existingNode"></param>
    /// <param name="nodeValue"></param>
    /// <param name="edgeValue"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Edge<Guid, Guid, TEdgeValue> StoreConnectedNode<TNodeValue, TEdgeValue>(
      this IDirectedGraphStore<Guid, TNodeValue, Guid, TEdgeValue> self,
      Guid existingNode,
      TNodeValue nodeValue = default(TNodeValue),
      TEdgeValue edgeValue = default(TEdgeValue),
      NodePosition direction = NodePosition.Successor
      )
    {
      return self.StoreNode(existingNode, Guid.NewGuid(), Guid.NewGuid(), nodeValue, edgeValue, direction);
    }
  }
}
