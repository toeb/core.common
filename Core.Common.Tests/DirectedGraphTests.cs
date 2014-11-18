using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Graph.Nodes;
using Core.Extensions;
using Core.Collections;
using Core.Graph.Directed;
using Core.Graph;

using System.Diagnostics;
namespace Core.Graph.Directed.Test
{

  class MyNode : INode<MyNode, MyEdge>
  {

    public IEnumerable<MyEdge> Edges
    {
      get;
      set;
    }
  }
  class MyEdge : IEdge<MyNode, MyEdge>
  {

    public MyNode Head
    {
      get;
      set;
    }

    public MyNode Tail
    {
      get;
      set;
    }
  }

  [DebuggerDisplay("n {i}")]
  class NBN : NodeBase<NBN, EBE>
  {
    public int i;
    public NBN()
    {

    }
    public NBN(int i) { this.i = i; }

  }
  [DebuggerDisplay("e {i}")]
  class EBE : EdgeBase<NBN, EBE>
  {
    public int i;
    public EBE()
    {

    }
    public EBE(int i) { this.i = i; }

  }

  [TestClass]
  public class CoreGraphDirectedTest
  {
    [TestMethod]
    public void TestINodeIEdge()
    {
      var n1 = new MyNode();
      var n2 = new MyNode();
      var n3 = new MyNode();
      var n4 = new MyNode();

      var e1 = new MyEdge() { Tail = n1, Head = n2 };
      var e2 = new MyEdge() { Tail = n2, Head = n3 };
      var e3 = new MyEdge() { Tail = n3, Head = n1 };
      var e4 = new MyEdge() { Tail = n4, Head = n2 };

      n1.Edges = new[] { e1, e3 };
      n2.Edges = new[] { e2, e4 };
      n3.Edges = new[] { e2, e3 };
      n4.Edges = new[] { e4 };


      var order = n1.BfsOrder(n => n.Edges.Select(e => e.Head)).ToArray();
      Assert.IsTrue(order.SequenceEqual(new[] { n1, n2, n3 }));
    }
    [TestMethod]
    public void TEstNodeBaseEdgeBase()
    {
      var n1 = new NBN();
      var n2 = new NBN();
      var n3 = new NBN();
      var n4 = new NBN();

      var e1 = new EBE() { Tail = n1, Head = n2 };
      var e2 = new EBE() { Tail = n2, Head = n3 };
      var e3 = new EBE() { Tail = n3, Head = n1 };
      var e4 = new EBE() { Tail = n4, Head = n2 };



      var order = n1.BfsOrder(n => n.Edges.Select(e => e.Head)).ToArray();
      Assert.IsTrue(order.SequenceEqual(new[] { n1, n2, n3 }));
      order = n1.BfsOrder(n => n.Edges.SelectMany(e => new[] { e.Head, e.Tail })).ToArray();
      Assert.IsTrue(order.ContainsAll(n1, n2, n3, n4));
    }



    [TestMethod]
    [Timeout(1500)]
    public void ReallyReallyLargeGraph()
    {
      var watch = new Stopwatch();
      watch.Start();
      int nV = 10000;
      int nE = 100000;
      Random rand = new System.Random();
      var nodes = new NBN[nV];
      var edges = new EBE[nE];
      for (int i = 0; i < nV; i++)
      {
        nodes[i] = new NBN();
      }
      for (int i = 0; i < nE; i++)
      {
        edges[i] = new EBE()
        {
          Tail = nodes[rand.Next(0, nV)],
          Head = nodes[rand.Next(0, nV)]

        };

      }

      var order = nodes[0].BfsOrder(n => n.Edges.SelectMany(e => new[] { e.Head, e.Tail })).ToArray();
      watch.Stop();
      Assert.IsTrue(watch.ElapsedMilliseconds < 15000);
    }

    [TestMethod]
    public void BfsEdgeNode()
    {

      var n1 = new NBN(1);
      var n2 = new NBN(2);
      var n3 = new NBN(3);
      var n4 = new NBN(4);

      var e1 = new EBE(1) { Tail = n1, Head = n2 };
      var e5 = new EBE(5) { Tail = n1, Head = n4 };
      var e2 = new EBE(2) { Tail = n2, Head = n3 };
      var e3 = new EBE(3) { Tail = n3, Head = n1 };
      var e4 = new EBE(4) { Tail = n4, Head = n2 };

      var result =DirectedGraphExtensions.BfsForwardOrder(n1).ToArray();

    }

    [TestMethod]
    public void Successors()
    {


      var n1 = new NBN(1);
      var n2 = new NBN(2);
      var n3 = new NBN(3);
      var n4 = new NBN(4);

      var e1 = new EBE(1) { Tail = n1, Head = n2 };
      var e5 = new EBE(5) { Tail = n1, Head = n4 };
      var e2 = new EBE(2) { Tail = n2, Head = n3 };
      var e3 = new EBE(3) { Tail = n3, Head = n1 };
      var e4 = new EBE(4) { Tail = n4, Head = n2 };

      var successors =  n1.Successors().ToArray();
      Assert.IsTrue(successors.ContainsAll(n2, n4));
    }
    [TestMethod]
    public void Predecessors()
    {


      var n1 = new NBN(1);
      var n2 = new NBN(2);
      var n3 = new NBN(3);
      var n4 = new NBN(4);

      var e1 = new EBE(1) { Tail = n1, Head = n2 };
      var e5 = new EBE(5) { Tail = n1, Head = n4 };
      var e2 = new EBE(2) { Tail = n2, Head = n3 };
      var e3 = new EBE(3) { Tail = n3, Head = n1 };
      var e4 = new EBE(4) { Tail = n4, Head = n2 };

      var predecessors = n1.Predecessors().ToArray();
      Assert.IsTrue(predecessors.ContainsAll(n3));
    }

  }
  /*
  public class NodeBuilder<TNode, TEdge, TGraph>
    where TGraph : IGraph<TNode, TEdge, TGraph>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {

  }
  public class EdgeBuilder<TNode, TEdge, TGraph>
    where TGraph : IGraph<TNode, TEdge, TGraph>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    public EdgeBuilder(TEdge edge)
    {

    }

  }
  public class GraphBuilder<TNode, TEdge, TGraph>
    where TGraph : IGraph<TNode, TEdge, TGraph>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    public GraphBuilder(IGraph<TNode, TEdge, TGraph> graph)
    {
      this.Graph = graph;
    }
    public void Dispose()
    {

    }
    public GraphBuilder<TNode, TEdge, TGraph> Node()
    {
      return this;
    }
    public GraphBuilder<TNode, TEdge, TGraph> Edge()
    {
      return this;
    }
    public IGraph<TNode, TEdge, TGraph> Graph { get; set; }
  }

  public class DefaultGraph<TNode, TEdge> : IGraph<TNode, TEdge, DefaultGraph<TNode, TEdge>>
    where TNode : INode<TNode, TEdge>
    where TEdge : IEdge<TNode, TEdge>
  {
    private ISet<TNode> nodes;
    private ISet<TEdge> edges;



    public IQueryable<TNode> Nodes
    {
      get { return nodes.AsQueryable(); }
    }

    public IQueryable<TEdge> Edges
    {
      get { return edges.AsQueryable(); }
    }
  }
  public static class Graph
  {
    public static GraphBuilder<TNode, TEdge, DefaultGraph<TNode, TEdge>> Build<TNode, TEdge>()
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return new DefaultGraph<TNode, TEdge>().Build();
    }
    public static GraphBuilder<TNode, TEdge, TGraph> Build<TNode, TEdge, TGraph>(this IGraph<TNode, TEdge, TGraph> graph)
      where TGraph : IGraph<TNode, TEdge, TGraph>
      where TNode : INode<TNode, TEdge>
      where TEdge : IEdge<TNode, TEdge>
    {
      return new GraphBuilder<TNode, TEdge, TGraph>(graph);
    }
  }
  public class MyNode : INode<MyNode, MyEdge>
  {

  }
  public class MyEdge : IEdge<MyNode, MyEdge>
  {
    public MyNode Head
    {
      get { throw new NotImplementedException(); }
    }

    public MyNode Tail
    {
      get { throw new NotImplementedException(); }
    }
  }
  public class MyGraph : IGraph<MyNode, MyEdge, MyGraph>
  {

  }*/
}
