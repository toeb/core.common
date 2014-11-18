using Core.Collections;
using Core.Graph.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Graph;
namespace Core.Test
{
  [DebuggerDisplay("{Value}")]
  class MyNode : Node<MyNode>
  {
    public MyNode() { }
    public MyNode(object value)
    {
      Value = value;
    }
    public object Value { get; set; }
  }
  [TestClass]
  public class NodeTest
  {
    [TestMethod]
    public void TestA()
    {
      MyNode a = new MyNode();
      MyNode b = new MyNode();
      a.Successors.Add(b);
      Assert.IsTrue(b.Predecessors.Contains(a));
    }
    [TestMethod]
    public void Dfs()
    {
      var a = new MyNode("a");
      var b = new MyNode("b");
      var c = new MyNode("c");
      var d = new MyNode("d");
      var e = new MyNode("e");
      var f = new MyNode("f");
      var g = new MyNode("g");

      a.Successors.Add(b);
      a.Successors.Add(c);

      b.Successors.Add(d);
      b.Successors.Add(e);

      c.Successors.Add(f);
      c.Successors.Add(g);



      var dfs = a.DfsOrder();


      Assert.IsTrue(dfs.IsInPartialOrder(a, b));
      Assert.IsTrue(dfs.IsInPartialOrder(a, c));
      Assert.IsTrue(dfs.IsInPartialOrder(b, d));
      Assert.IsTrue(dfs.IsInPartialOrder(b, e));
      Assert.IsTrue(dfs.IsInPartialOrder(c, f));
      Assert.IsTrue(dfs.IsInPartialOrder(c, g));

    }

    [TestMethod]
    public void DfsWithCycle()
    {

      var a = new MyNode("a");
      var b = new MyNode("b");
      a.Successors.Add(b);
      b.Successors.Add(a);
      // should not throw
      var dfs = a.DfsOrder();
    }
    [TestMethod]
    public void BfsWithCycle()
    {

      var a = new MyNode("a");
      var b = new MyNode("b");
      a.Successors.Add(b);
      b.Successors.Add(a);
      // should not throw
      var dfs = a.BfsOrder();
    }

    [TestMethod]
    public void Bfs()
    {
      var a = new MyNode("a");
      var b = new MyNode("b");
      var c = new MyNode("c");
      var d = new MyNode("d");
      var e = new MyNode("e");
      var f = new MyNode("f");
      var g = new MyNode("g");

      a.Successors.Add(b);
      a.Successors.Add(c);

      b.Successors.Add(d);
      b.Successors.Add(e);

      c.Successors.Add(f);
      c.Successors.Add(g);



      var dfs = a.BfsOrder();


      Assert.IsTrue(dfs.IsInPartialOrder(a, b));
      Assert.IsTrue(dfs.IsInPartialOrder(a, c));
      Assert.IsTrue(dfs.IsInPartialOrder(b, d));
      Assert.IsTrue(dfs.IsInPartialOrder(b, e));
      Assert.IsTrue(dfs.IsInPartialOrder(c, f));
      Assert.IsTrue(dfs.IsInPartialOrder(c, g));

    }

    [TestMethod]
    public void PartialOrder()
    {
      var a = new MyNode("a");
      var b = new MyNode("b");
      var c = new MyNode("c");
      var d = new MyNode("d");
      var e = new MyNode("e");
      var f = new MyNode("f");
      var g = new MyNode("g");

      a.Successors.Add(b);
      a.Successors.Add(c);

      b.Successors.Add(d);
      b.Successors.Add(e);

      c.Successors.Add(f);
      c.Successors.Add(g);

      var order = a.PartialOrder(n => n.Successors);
      Assert.IsTrue(order.IsInPartialOrder(c, a));
      Assert.IsTrue(order.IsInPartialOrder(b, a));
      Assert.IsTrue(order.IsInPartialOrder(d, b));
      Assert.IsTrue(order.IsInPartialOrder(e, b));
      Assert.IsTrue(order.IsInPartialOrder(f, c));
      Assert.IsTrue(order.IsInPartialOrder(g, c));

    }

    [TestMethod]
    public void PartialOrderShouldNotReturnNull()
    {
      var a = new MyNode("a");
      var b = new MyNode("b");
      var c = new MyNode("c");
      a.Successors.Add(b);
      a.Successors.Add(c);
      b.Successors.Add(c);

       var order = a.PartialOrder(n => n.Successors);
      Assert.IsNotNull(order);
    }

    [TestMethod]
    public void PartialOrderShouldReturnNullIfCycleExists()
    {

      var a = new MyNode("a");
      var b = new MyNode("b");
      a.Successors.Add(b);
      b.Successors.Add(a);

      var order = a.PartialOrder(n => n.Successors);
      Assert.IsNull(order);
    }
  }
}
