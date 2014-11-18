using Core.Graph;
using Core.ManagedObjects;
using Core.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Core.ObjectGraph
{



  [DebuggerDisplay("go:{Value}")]
  public abstract class AbstractGraphObject : AssignableManagedObject, IGraphObject
  {
    protected override void OnEdgesAdded(IEnumerable<AbstractConnection> edges)
    {
      Domain.AddConnections(edges.Where(connection => connection.Head is IGraphProperty));
    }
    protected override void OnEdgesRemoved(IEnumerable<AbstractConnection> edges)
    {
      Domain.RemoveConnections(edges.Where(connection => connection.Head is IGraphObject));
    }

    protected sealed override IManagedProperty ConstructProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      Expand();
      // check if it should be part of the object graph
      if (IsGraphObjectProperty(info, initialValue))
      {
        return ConstructGraphProperty(info, initialValue);
      }
      // if its a not a graphproperty return a default memory property
      return ConstructNonGraphProperty(info, initialValue);
    }


    protected sealed override IObjectPropertyConnection ConstructPropertyConnection(IManagedProperty property)
    {
      if (property is IGraphProperty)
      {


      }
      return base.ConstructPropertyConnection(property);
    }

    protected virtual IGraphObjectPropertyConnection ConstructGraphObjectPropertyConnection(IGraphProperty property) { return Domain.ConstructGraphObjectPropertyConnection(this, property); }
    protected virtual IObjectPropertyConnection ConstructNonGraphObjectPropertyConnection(IManagedProperty property) { return base.ConstructPropertyConnection(property); }


    public AbstractGraphObject(Lazy<object> initialValue, AbstractGraphDomain graphDomain)
      : base(new ManagedObjectInfo(true, true, typeof(object), false))
    {
      var exp = new DelegateExpandable(ExpandGraphObject, CollapseGraphObject);
      exp.PropertyChanged += (sender, args) => this.RaisePropertyChanged(args);
      this.expandable = exp;
      this.Domain = graphDomain;
    }

    protected virtual IManagedProperty ConstructNonGraphProperty(IPropertyInfo info, Lazy<object> initialValue) { return base.ConstructProperty(info, initialValue); }
    protected virtual IGraphProperty ConstructGraphProperty(IPropertyInfo info, Lazy<object> initialValue) { return Domain.ConstructGraphProperty(this, info, initialValue); }
    protected virtual bool IsGraphObjectProperty(IPropertyInfo info, Lazy<object> initialValue) { return Domain.IsGraphObjectProperty(this, info, initialValue); }

    protected abstract void AddValueProperties(object value);
    protected abstract void RemoveValueProperties(object value);


    protected override void OnBeforeValueConsumed(object newValue)
    {
      Collapse();
    }
    protected override void OnValueConsumed(object value)
    {

    }

    protected internal AbstractGraphDomain Domain { get; private set; }
    IGraphDomain IGraphDomainNode.Domain { get { return Domain; } }

    public bool ExpandGraphObject()
    {
      AddValueProperties(ProduceValue());
      return true;
    }

    public bool CollapseGraphObject()
    {
      RemoveValueProperties(ProduceValue());
      return true;
    }

    #region expandable delegation
    private IExpandable expandable;
    public ExpandableState State
    {
      get { return expandable.State; }
    }

    public bool IsTransitioning
    {
      get { return expandable.IsTransitioning; }
    }
    public bool IsExpanded
    {
      get { return expandable.IsExpanded; }
    }
    public bool IsCollapsed
    {
      get { return expandable.IsCollapsed; }
    }

    public ExpandableState Expand()
    {
      return expandable.Expand();
    }

    public ExpandableState Collapse()
    {
      return expandable.Collapse();
    }
    #endregion
  }
}
