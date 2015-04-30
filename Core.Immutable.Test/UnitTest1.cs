using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
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


  public class ImmutablePropertyStore : Core.AbstractPropertyStore
  {
    internal ModelNode Node { get; set; }

    protected override void SetProperty(string key, object value, Type type)
    {

    }

    protected override object GetProperty(string key, Type type)
    {
      return null;
    }

    protected override bool HasProperty(string key, Type type)
    {

      return false;
    }
  }

  public class Person : ImmutablePropertyStore
  {
    public string LastName { get { return Get<string>(); } set { Set(value); } }
    public string FirstName { get { return Get<string>(); } set { Set(value); } }
    public IEnumerable<Person> Children { get { return Get<IEnumerable<Person>>(); } set { Set(value); } }
    public Person Parent { get { return Get<Person>(); } set { Set(value); } }
  }

  public class ModelNode : IEquatable<ModelNode>
  {
    private object data;
    public object Data
    {
      get { return data; }
    }
    public ModelNode(object data)
    {
      if (data == null)
      {
        this.data = data;

      }
      else if (data.GetType() == typeof(string) || data.GetType().IsPrimitive)
      {
        this.data = data;

      }
      else
      {
        throw new Exception("node may only store primitive data");
      }
    }
    public bool Equals(ModelNode other)
    {
      return object.ReferenceEquals(other, this);
    }
  }

  public class ModelEdge : Edge<ModelNode>
  {
    public ModelEdge(ModelNode tail, ModelNode head)
      : base(tail, head)
    {

    }
  }
  public abstract class NodeView
  {
    public ModelNode Node { get; protected set; }
    public abstract bool TrySetValue(object value);
    public abstract bool TryGetValue(out object value);
    public object Value
    {
      get
      {
        object value;
        if (!TryGetValue(out value)) throw new Exception("could not get value");
        return value;
      }
      set
      {
        if (!TrySetValue(value)) throw new Exception("could not set value");
      }
    }
  }
  public class PrimitiveNodeView : NodeView
  {
    private Model model;
    public PrimitiveNodeView(Model model)
    {
      this.model = model;
    }

    public override bool TrySetValue(object value)
    {
      if (Node != null) model.Graph = model.Graph.WithoutNode(Node);
      Node = new ModelNode(value);
      model.Graph = model.Graph.WithNode(Node);
      return true;
    }
    public override bool TryGetValue(out object value)
    {
      value = null;
      if (Node == null) return false;
      if (!model.Graph.Nodes.Contains(Node)) return false;
      value = Node.Data;
      return true;
    }

  }
  public class ListNodeView : NodeView
  {
    private Model model;
    public ListNodeView(Model model)
    {
      this.model = model;

    }
    public override bool TrySetValue(object value)
    {

      if (value == null)
      {
        if(Node == null) model.Graph = model.Graph.WithoutNode(Node);
        return true;
      }

      var type = value.GetType();
      if (!type.IsEnumerable()) return false;

      var enumerable = value as IEnumerable;
      
      foreach (var item in enumerable)
      {
        var view = model.Value(item);
        var Node = view.Node;
        model.Graph = model.Graph.WithNode(Node);
      }

      return false;
    }

    public override bool TryGetValue(out object value)
    {
      value = null;
      if(Node == null){
        return true;
      }
      List<object> list;
      foreach (var successor in model.Graph.GetSuccessors(Node))
      {

      }
      throw new NotImplementedException();
    }
  }


  public class Model
  {
    private IImmutableGraph<ModelNode, ModelEdge> graph;
    public IImmutableGraph<ModelNode, ModelEdge> Graph
    {
      get
      {
        return graph;
      }
      set
      {
        graph = value;
      }
    }
    private Model()
    {
      this.graph = ImmutableGraph.Create<ModelNode, ModelEdge>(CreateEdge);
    }

    private ModelEdge CreateEdge(ModelNode tail, ModelNode head)
    {
      return new ModelEdge(tail, head);
    }
    public static Model Create() { return new Model(); }


    internal PrimitiveNodeView Primitive(object value)
    {
      var view = new PrimitiveNodeView(this);
      view.TrySetValue(value);
      return view;
    }


    internal ListNodeView List(IEnumerable items)
    {
      throw new NotImplementedException();
    }
    internal ListNodeView List(params object[] items)
    {
      return List(items.AsEnumerable());
    }


    internal NodeView Value(object item)
    {
      if (item == null || item.GetType().IsPrimitive || item.GetType() == typeof(string)) return Primitive(item);
      var type = item.GetType();
      if (type.IsEnumerable()) return List(item as IEnumerable);
      throw new NotImplementedException();
      // implements ModelObject?
    }
  }


  [TestClass]
  public class ImmutableModelTest
  {
    [TestMethod]
    public void CreateImmutableModel()
    {
      var model = Model.Create();
      Assert.IsNotNull(model);
    }
    [TestMethod]
    public void CreatePrimitiveStringValue()
    {
      var model = Model.Create();
      var node = model.Primitive("hello");
      Assert.IsNotNull(node);
      Assert.AreEqual("hello", node.Value);
    }
    [TestMethod]
    public void CreatePrimitiveIntValue()
    {
      var model = Model.Create();
      var node = model.Primitive(123);
      Assert.IsNotNull(node);
      Assert.AreEqual(123, node.Value);
    }
    [TestMethod]
    public void CreatePrimitiveNullValue()
    {
      var model = Model.Create();
      var node = model.Primitive(null);
      Assert.IsNotNull(node);
      Assert.IsNull(node.Value);
    }
    [TestMethod]
    public void CreateListValue()
    {
      var model = Model.Create();
      var node = model.List();
      Assert.IsNotNull(node);

    }
  }
}
