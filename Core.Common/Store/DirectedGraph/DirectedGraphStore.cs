using Core.Store.KeyValue;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace Core.Store.DirectedGraph
{

  [Export(typeof(DirectedGraphStore<,,,>))]
  [Export(typeof(IDirectedGraphStore<,,,>))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class DirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue> :
    IDirectedGraphStore<TNodeKey, TNodeValue, TEdgeKey, TEdgeValue>
  {

    [ImportingConstructor]
    public DirectedGraphStore([Import] CompositionContainer container)
    {
      // Nodes = container.GetExportedValue<JsonMemoryKeyValueStore<TNodeKey, Node<TNodeKey, TNodeValue>>>();
      // Edges = container.GetExportedValue<JsonMemoryKeyValueStore<TEdgeKey, Edge<TNodeKey, TEdgeKey, TEdgeValue>>>();

    }

    [Import]
    IKeyValueStore<TNodeKey, TNodeValue> Nodes { get; set; }
    [Import]
    IKeyValueStore<TEdgeKey, TNodeKey> Heads { get; set; }
    [Import]
    IKeyValueStore<TEdgeKey, TNodeKey> Tails { get; set; }
    [Import]
    IKeyValueStore<TEdgeKey, TEdgeValue> EdgeValues { get; set; }





    public Node<TNodeKey,TNodeValue> StoreNode(Node<TNodeKey,TNodeValue> node)
    {
      Nodes.Store(node.Id, node.Value);
      return node;
    }

    public void DeleteNode(TNodeKey key)
    {
      var edgesToDelete = GetEdges().Where(e => e.Head.Equals(key) || e.Tail.Equals(key)).ToArray();
      foreach (var id in edgesToDelete.Select(e => e.Id))
      {
        DeleteEdge(id);
      }
      Nodes.Delete(key);
    }

    public Node<TNodeKey, TNodeValue> LoadNode(TNodeKey id)
    {
      return this.MakeNode(id, Nodes.Load(id));
    }

    public IQueryable<Node<TNodeKey, TNodeValue>> GetNodes()
    {
      return Nodes.Keys.Select(id => this.MakeNode(id, Nodes.Load(id)));
    }
   /* private Edge<TNodeKey, TEdgeKey, TEdgeValue> MakeEdge(TEdgeKey id, TNodeKey tail, TNodeKey head, TEdgeValue value)
    {
      return new Edge<TNodeKey, TEdgeKey, TEdgeValue>(id, tail, head, value);
    }
    private Node<TNodeKey, TNodeValue> MakeNode(TNodeKey id, TNodeValue value)
    {
      return new Node<TNodeKey, TNodeValue>(id, value);
    }*/
    public IQueryable<Edge<TNodeKey, TEdgeKey, TEdgeValue>> GetEdges()
    {
      return EdgeValues.Keys.Select(id => LoadEdge(id));
    }

    public Edge<TNodeKey,TEdgeKey,TEdgeValue> StoreEdge(Edge<TNodeKey,TEdgeKey,TEdgeValue> edge)
    {
      EdgeValues.Store(edge.Id, edge.Value);
      Heads.Store(edge.Id, edge.Head);
      Tails.Store(edge.Id, edge.Tail);
      return edge;
    }

    public void DeleteEdge(TEdgeKey id)
    {
      EdgeValues.Delete(id);
      Heads.Delete(id);
      Tails.Delete(id);
    }


    public Edge<TNodeKey, TEdgeKey, TEdgeValue> LoadEdge(TEdgeKey key)
    {
      return this.MakeEdge(key, Tails.Load(key), Heads.Load(key), EdgeValues.Load(key));
    }



  }


}
