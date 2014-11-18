using System.Linq;

namespace Core.Store.DirectedGraph
{

  public interface INodeStore<TNodeKey,TNodeValue>{
    Node<TNodeKey, TNodeValue> StoreNode(Node<TNodeKey,TNodeValue> node);
    void DeleteNode(TNodeKey key);
    Node<TNodeKey, TNodeValue> LoadNode(TNodeKey key);
    IQueryable<Node<TNodeKey, TNodeValue>> GetNodes();
  }
  public interface IEdgeStore<TNodeKey, TEdgeKey, TEdgeValue>
  {
    Edge<TNodeKey,TEdgeKey,TEdgeValue> StoreEdge(Edge<TNodeKey,TEdgeKey,TEdgeValue> edge);
    void DeleteEdge(TEdgeKey key);
    Edge<TNodeKey, TEdgeKey, TEdgeValue> LoadEdge(TEdgeKey key);
    IQueryable<Edge<TNodeKey, TEdgeKey, TEdgeValue>> GetEdges();
  }
  public interface IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>:
    INodeStore<TNodeKey,TNodeValue>,
    IEdgeStore<TNodeKey,TEdgeKey,TEdgeValue>
  {
  }
}
