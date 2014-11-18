
using System.Diagnostics;
namespace Core.Store.DirectedGraph
{
  /// <summary>
  /// A Value Type for a Node. A Node only constists of a value and an Identifer
  /// </summary>
  /// <typeparam name="TNodeKey"></typeparam>
  /// <typeparam name="TNodeValue"></typeparam>
  [DebuggerDisplay("n{id}:{value}")]
  public struct Node<TNodeKey, TNodeValue> : IIdentifiable<TNodeKey>
  {
    private TNodeKey id;
    private TNodeValue value;
    public Node(TNodeKey key, TNodeValue value)
    {
      this.id = key;
      this.value = value;
    }
    //public Node() { id = default(TNodeKey); value = default(TNodeValue); }
    public TNodeValue Value
    {
      get { return value; }
      //set {this.value = value; }
    }

    public TNodeKey Id
    {
      get { return id; }
      //set{this.id = value;}
    }
  }
}
