using Core.Extensions;
using Core.Graph.Directed;
using Core.ManagedObjects;
using Core.Values;
using System;
using System.Collections.Generic;

namespace Core.ObjectGraph
{
  public abstract class AbstractGraphDomain : GraphBase<IConnectable, IConnection, AbstractGraphDomain>, IGraphDomain
  {
    public AbstractGraphDomain()
    {
    }

    public abstract IGraphObject Get(object value);
    protected abstract void Set(object value, IGraphObject graphObject);
    protected internal abstract IPropertyValueConnection ConstructPropertyValueConnection(IGraphProperty property);
    protected internal abstract IGraphObjectPropertyConnection ConstructGraphObjectPropertyConnection(IGraphObject abstractGraphObject, IGraphProperty property);
    protected internal abstract IGraphObject ConstructGraphObject(Lazy<object> initialValue);
    protected internal abstract IGraphProperty ConstructGraphProperty(IGraphObject @object, IPropertyInfo info, Lazy<object> initialValue);
    protected internal abstract bool IsGraphObjectType(Type type);
    protected internal abstract bool IsGraphObjectProperty(IGraphObject @object, IPropertyInfo info, Lazy<object> initialValue);
    protected internal abstract bool IsGraphObject(object info);

    protected virtual void OnGraphObjectCreated(IGraphObject graphObject) { }

    protected void Register(IGraphObject graphObject, object obj)
    {
      AddNodes(graphObject.MakeEnumerable());
      Set(obj, graphObject);
    }


    public IGraphObject Create(object value)
    {
      // create a dictionary to store types
      var graphObject = ConstructGraphObject( new Lazy<object>(() => value ));
      Register(graphObject, value);
      graphObject.Consume(value);
      OnGraphObjectCreated(graphObject);
      return graphObject;
    }


    public IGraphObject Require(object value)
    {
      var result = Get(value);
      if (result != null) return result;
      result = Create(value);
      return result;
    }

    protected internal void AddConnections(IEnumerable<IConnection> connections)
    {
      this.AddEdges(connections);
    }

    protected internal void RemoveConnections(IEnumerable<IConnection> connections)
    {
      this.RemoveEdges(connections);
    }




  }
}
