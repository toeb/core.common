
using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.DirectedGraph.Tests 
{

  [TestClass]
  public class CoreStoreDirectedGraphTest : TestBase
  {    
    private IDirectedGraphStore<Guid, object, Guid, object> uut;
    [TestInitialize]
    public void InitTest()
    {
      uut = null;
      uut = GetUut<DirectedGraphStore<Guid, object, Guid, object>>();
    }

    [TestMethod]
    public void Creation()
    {
      Assert.IsNotNull(uut);
      Assert.IsFalse(uut.GetEdges().Any());
      Assert.IsFalse(uut.GetNodes().Any());
    }

    [TestMethod]
    public void StoreNode()
    {
      var id = Guid.NewGuid();
      uut.StoreNode(id, "hello node");
      var a = uut.LoadNode(id);
      var result = uut.GetNodes().First();
      Assert.AreEqual("hello node", result.Value);
    }
    [TestMethod]
    public void DeleteUnconnectedNode()
    {
      var id = Guid.NewGuid();
      uut.StoreNode(id, "hello");
      uut.DeleteNode(id);
      Assert.IsFalse(uut.GetNodes().Any());
    }

    [TestMethod]
    public void CreateEdge()
    {
      var id1 = Guid.NewGuid();
      var id2 = Guid.NewGuid();
      uut.StoreNode(id1, "n1");
      uut.StoreNode(id2, "n2");
      uut.StoreEdge(Guid.NewGuid(), id1, id2, "relation");

      var edges = uut.GetEdges();
      var edge = uut.GetEdges().Single();
      Assert.AreEqual(id1, edge.Tail);
      Assert.AreEqual(id2, edge.Head);
      Assert.AreEqual("relation", edge.Value);
    }



    [TestMethod]
    public void DeleteEdge()
    {

      var id1 = Guid.NewGuid();
      var id2 = Guid.NewGuid();
      var e1 = Guid.NewGuid();
      uut.StoreNode(id1, "n1");
      uut.StoreNode(id2, "n2");
      uut.StoreEdge(e1, id1, id2, "relation");

      uut.DeleteEdge(e1);
      Assert.IsFalse(uut.GetEdges().Any());
    }

    [TestMethod]
    public void DeleteConnectedNode()
    {

      var id1 = Guid.NewGuid();
      var id2 = Guid.NewGuid();
      uut.StoreNode(id1, "n1");
      uut.StoreNode(id2, "n2");
      uut.StoreEdge(Guid.NewGuid(), id1, id2, "relation");
      uut.DeleteNode(id1);
      Assert.IsFalse(uut.GetEdges().Any());
      Assert.AreEqual(id2, uut.GetNodes().Single().Id);
    }




  }
}
