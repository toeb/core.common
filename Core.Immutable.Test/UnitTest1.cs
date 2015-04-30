using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Core.Immutable.Test
{
  public static class ImmutableHelpers
  {
    public static bool IsImmutable(object @object, Type type)
    {
      return type.IsPrimitive
        || type.ImplementsInterface(typeof(IImmutableDictionary<string, object>))
        || type.ImplementsInterface(typeof(IImmutableList<object>));

    }
  }
  public class RevisionNode
  {
    private object @object;
    private Revision revision;
    private RevisionNode previous;
    public RevisionNode(object @object, Revision revision, RevisionNode previous)
    {
      this.@object = @object;
      this.revision = revision;
      this.previous = previous;
    }

    public Revision Revision { get { return revision; } }
    public object Object { get { return @object; } }
    public RevisionNode Previous { get { return previous; } }

  }
  public class Revision
  {
    private List<RevisionNode> nodes;
    
    private object state;
    private DateTime timestamp;
    private Revision parent;

    public Revision(Revision parent, DateTime timestamp, object state)
    {
      ImmutableHelpers.IsImmutable(state, state == null ? null : state.GetType());
      this.state = state;
      this.timestamp = timestamp;
      this.parent = parent;
    }
    object State { get { return state; } }
    public DateTime Timestamp { get { return timestamp; } }
    public Revision Parent { get { return parent; } }

  }
  public class ImmutablePropertyStore : Core.AbstractPropertyStore
  {
    IImmutableDictionary<string, object> store;

    protected override void SetProperty(string key, object value, Type type)
    {
      if (!ImmutableHelpers.IsImmutable(value, type)) throw new ArgumentException("value MUST be immutable");

    }

    protected override object GetProperty(string key, Type type)
    {
      object result = null;
      var success = store.TryGetValue(key, out result);
      return result;
    }

    protected override bool HasProperty(string key, Type type)
    {
      string result;
      var success = store.TryGetKey(key, out result);
      return success;
    }
  }

  public class Person : ImmutablePropertyStore
  {
    public string LastName { get { return Get<string>(); } set { Set(value); } }
    public string FirstName { get { return Get<string>(); } set { Set(value); } }
    public IEnumerable<Person> Children { get { return Get<IEnumerable<Person>>(); } set { Set(value); } }
    public Person Parent { get { return Get<Person>(); } set { Set(value); } }
  }

  public interface IImmutableGraph<NodeType>
  {
    int NodeCount { get; }
    int EdgeCount { get; }
    IImmutableSet<NodeType> Nodes { get; }
    IImmutableSet<Edge<NodeType>> Edges { get; }



  }
  public static class ImmutableGraph
  {
    internal static IImmutableGraph<NodeType> Create<NodeType>(params NodeType[] nodes)
    {
      return new ImmutableGraphImplementation<NodeType>(ImmutableHashSet.Create(nodes), ImmutableHashSet.Create<Edge<NodeType>>());
    }


    public static IImmutableGraph<NodeType> WithNode<NodeType>(this IImmutableGraph<NodeType> self, NodeType node)
    {
      return new ImmutableGraphImplementation<NodeType>(self.Nodes.Add(node), self.Edges);
    }
    public static IImmutableGraph<NodeType> WithoutNode<NodeType>(this IImmutableGraph<NodeType> self, NodeType node)
    {

      var edges = self.GetIncedentEdges(node);
      foreach (var edge in edges) self = self.WithoutEdge(edge);
      return new ImmutableGraphImplementation<NodeType>(self.Nodes.Remove(node), self.Edges);
    }

    public static IEnumerable<Edge<NodeType>> GetIncedentEdges<NodeType>(this IImmutableGraph<NodeType> self, NodeType node)
    {
      return self.Edges.Where(edge => edge.Tail.Equals(node) || edge.Head.Equals(node));
    }
    public static IEnumerable<Edge<NodeType>> GetOutgoingEdges<NodeType>(this IImmutableGraph<NodeType> self, NodeType node)
    {
      return self.Edges.Where(edge => edge.Tail.Equals(node));
    }
    public static IEnumerable<Edge<NodeType>> GetIncommingEdges<NodeType>(this IImmutableGraph<NodeType> self, NodeType node)
    {
      return self.Edges.Where(edge => edge.Head.Equals(node));
    }
    public static IImmutableGraph<NodeType> WithEdge<NodeType>(this IImmutableGraph<NodeType> self, NodeType tail, NodeType head)
    {
      return self.WithEdge(new Edge<NodeType>(tail, head));
    }
    public static IImmutableGraph<NodeType> WithEdge<NodeType>(this IImmutableGraph<NodeType> self, Edge<NodeType> edge)
    {
      if (!self.Nodes.Contains(edge.Tail)) self = self.WithNode(edge.Tail);
      if (!self.Nodes.Contains(edge.Head)) self = self.WithNode(edge.Head);
      return new ImmutableGraphImplementation<NodeType>(self.Nodes, self.Edges.Add(edge));
      
    }
    public static IImmutableGraph<NodeType> WithoutEdge<NodeType>(this IImmutableGraph<NodeType> self, NodeType tail, NodeType head)
    {
      return self.WithoutEdge(new Edge<NodeType>(tail, head));
    }
    public static IImmutableGraph<NodeType> WithoutEdge<NodeType>(this IImmutableGraph<NodeType> self, Edge<NodeType> edge)
    {
      return new ImmutableGraphImplementation<NodeType>(self.Nodes, self.Edges.Remove(edge));
    }
  }
  public class Edge<NodeType>
  {
    private NodeType head;
    private NodeType tail;
    public Edge(NodeType tail, NodeType head) {
      this.tail = tail;
      this.head = head;
    }
    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (obj.GetType() != typeof(Edge<NodeType>)) return false;

      return this == ((Edge<NodeType>)obj);

    }
    public static bool operator ==(Edge<NodeType> lhs, Edge<NodeType> rhs)
    {
      return lhs.head.Equals(rhs.head) && lhs.tail.Equals(rhs.tail);
    }
    public static bool operator !=(Edge<NodeType> lhs, Edge<NodeType> rhs)
    {
      return !(lhs == rhs);
    }
    public override int GetHashCode()
    {
      return Head.GetHashCode() ^ Tail.GetHashCode();
    }
    public NodeType Head { get { return head; } }
    public NodeType Tail { get { return tail; } }

  }
  class ImmutableGraphImplementation<NodeType> :IImmutableGraph<NodeType>
  {

    private IImmutableSet<NodeType> nodes;    
    private IImmutableSet<Edge<NodeType>> edges;

    public ImmutableGraphImplementation(IImmutableSet<NodeType> nodes, IImmutableSet<Edge<NodeType>> edges)
    {
      this.nodes = nodes;
      this.edges= edges;
    }



    public int NodeCount
    {
      get { return nodes.Count; }
    }

    public int EdgeCount
    {
      get { return edges.Count; }
    }


    public IImmutableSet<NodeType> Nodes
    {
      get { return nodes; }
    }

    public IImmutableSet<Edge<NodeType>> Edges
    {
      get { return edges; }
    }


  }

  [TestClass]
  public class ImmutableGraphTests
  {
    [TestMethod]
    public void CreateEmpty()
    {
      var graph = ImmutableGraph.Create<int>();
      Assert.IsNotNull(graph);
      Assert.AreEqual(0, graph.NodeCount);
      Assert.AreEqual(0, graph.EdgeCount);
    }
    [TestMethod]
    public void CreateFromParams()
    {
      var graph = ImmutableGraph.Create(1, 2, 3);
      Assert.IsNotNull(graph);
      Assert.AreEqual(3, graph.NodeCount);
      Assert.AreEqual(0, graph.EdgeCount);
      Assert.IsTrue(graph.Nodes.Contains(1));
      Assert.IsTrue(graph.Nodes.Contains(2));
      Assert.IsTrue(graph.Nodes.Contains(3));
    }

    [TestMethod]
    public void AddNode()
    {
      var graph = ImmutableGraph.Create<int>().WithNode(2);
      Assert.IsNotNull(graph);
      Assert.AreEqual(0, graph.EdgeCount);
      Assert.AreEqual(1, graph.NodeCount);
      Assert.IsTrue(graph.Nodes.Contains(2));
    }

    [TestMethod]
    public void AddEdge()
    {
      var graph = ImmutableGraph.Create(1, 2).WithEdge(1, 2);
      Assert.IsNotNull(graph);
      Assert.AreEqual(1, graph.EdgeCount);
      Assert.AreEqual(2, graph.NodeCount);
      Assert.IsTrue(graph.Edges.Contains(new Edge<int>(1,2)));

    }
    [TestMethod]
    public void AddEdgeWithUnknownNodes()
    {
      var graph = ImmutableGraph.Create(1, 2).WithEdge(3, 4);
      Assert.AreEqual(4, graph.NodeCount);
      Assert.AreEqual(1, graph.EdgeCount);
      Assert.IsTrue(graph.Nodes.Contains(3));
      Assert.IsTrue(graph.Nodes.Contains(4));
    }
    [TestMethod]
    public void RemoveUnconnectedNode()
    {
      var graph = ImmutableGraph.Create<int>(1, 2, 3);

      graph = graph.WithoutNode(2);

      Assert.AreEqual(2, graph.NodeCount);
      Assert.AreEqual(0, graph.EdgeCount);
      Assert.IsFalse(graph.Nodes.Contains(2));
    }
    [TestMethod]
    public void RemoveEdge()
    {
      var graph = ImmutableGraph
        .Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3)
        .WithoutEdge(1, 2);
      Assert.AreEqual(1, graph.EdgeCount);
      Assert.IsFalse(graph.Edges.Contains(new Edge<int>(1, 2)));
      Assert.IsTrue(graph.Edges.Contains(new Edge<int>(2,3)));
      
    }
    [TestMethod]
    public void GetIncommingEdges()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3);

      var edges = graph.GetIncommingEdges(2);
      Assert.AreEqual(1, edges.Count());
      Assert.AreEqual(new Edge<int>(1, 2), edges.First());
    }
    [TestMethod]
    public void GetOutgoingEdges()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3);

      var edges = graph.GetOutgoingEdges(2);
      Assert.AreEqual(1, edges.Count());
      Assert.AreEqual(new Edge<int>(2, 3), edges.First());

    }
    [TestMethod]
    public void GetIncedentEdges()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3);

      var edges = graph.GetIncedentEdges(2);
      Assert.AreEqual(2, edges.Count());
      Assert.IsTrue(edges.Contains(new Edge<int>(2, 3)));
      Assert.IsTrue(edges.Contains(new Edge<int>(1, 2)));

    }
    [TestMethod]
    public void RemoveConnectedNode()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3)
        .WithoutNode(2);
      Assert.AreEqual(2, graph.NodeCount);
      Assert.AreEqual(0, graph.EdgeCount);
      
    }

  }
}
