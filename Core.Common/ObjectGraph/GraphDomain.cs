using Core.ManagedObjects;
using System;
using System.Collections.Generic;

namespace Core.ObjectGraph
{
  public class GraphDomain : AbstractGraphDomain
  {
    public GraphDomain()
    {
      AutoExpand = true;
      GraphDomainTypes = new HashSet<Type>();
    }
    public ISet<Type> GraphDomainTypes
    {
      get;
      private set;
    }
    private IDictionary<object, IGraphObject> dictionary = new Dictionary<object, IGraphObject>();
    public bool AutoExpand
    {
      get;
      set;

    }
    private Queue<IGraphObject> unexpanded = new Queue<IGraphObject>();
    protected override void OnGraphObjectCreated(IGraphObject graphObject)
    {
      unexpanded.Enqueue(graphObject);
      if (unexpanded.Count != 1) return;
      while (true)
      {
        if (unexpanded.Count == 0) return;
        var current = unexpanded.Peek();
        current.Expand();
        unexpanded.Dequeue();
      }
    }
    public override IGraphObject Get(object value)
    {
      if (value != null && dictionary.ContainsKey(value)) return dictionary[value];
      return null;
    }
    protected override void Set(object value, IGraphObject graphObject)
    {
      if (value != null) dictionary[value] = graphObject;
    }

    protected internal override bool IsGraphObjectType(Type type)
    {
      return GraphDomainTypes.Contains(type);
    }

    protected internal override bool IsGraphObjectProperty(IGraphObject @object, IPropertyInfo info, Lazy<object> initialValue)
    {
      return IsGraphObjectType(info.ValueType);
    }

    protected internal override bool IsGraphObject(object @object)
    {
      if (@object == null) return false;
      return IsGraphObjectType(@object.GetType());
    }

    protected internal override IGraphProperty ConstructGraphProperty(IGraphObject @object, IPropertyInfo info, Lazy<object> initialValue)
    {
      var graphObjectValue = Require(initialValue.Value);
      return new GraphProperty(info.Name, graphObjectValue, (GraphObject)@object, this);
    }
    protected internal override IGraphObject ConstructGraphObject(Lazy<object> initialValue)
    {
      return new GraphObject(initialValue, this);

    }


    protected internal override IPropertyValueConnection ConstructPropertyValueConnection(IGraphProperty property)
    {
      return new PropertyValueConnection();
    }


    protected internal override IGraphObjectPropertyConnection ConstructGraphObjectPropertyConnection(IGraphObject graphObject, IGraphProperty property)
    {
      return new GraphObjectPropertyConnection();
    }
  }
}
