using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Immutable.Test
{

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
      Assert.IsTrue(graph.Edges.Contains(new Edge<int>(1, 2)));

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
      Assert.IsTrue(graph.Edges.Contains(new Edge<int>(2, 3)));

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
    [TestMethod]
    public void GetSuccessors()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3)
        .WithEdge(3, 2);

      var result = graph.GetSuccessors(2);
      Assert.IsTrue(result.Single() == 3);
    }
    [TestMethod]
    public void GetPredecessors()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3)
        .WithEdge(3, 2);
      var result =graph.GetPredecessors(2);
      Assert.AreEqual(2, result.Count());
      Assert.IsTrue(result.Contains(3));
      Assert.IsTrue(result.Contains(1));
    }

    [TestMethod]
    public void GetNeighbors()
    {
      var graph = ImmutableGraph.Create(1, 2, 3)
        .WithEdge(1, 2)
        .WithEdge(2, 3)
        .WithEdge(3,2);
      var result = graph.GetNeighbors(2);
      Assert.AreEqual(2, result.Count());
      Assert.IsTrue(result.Contains(1));
      Assert.IsTrue(result.Contains(3));
    }
    
  }



}
