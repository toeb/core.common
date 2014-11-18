
using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
namespace Core.ManagedObjects
{



  /// <summary>
  /// Abstract base for managed objects
  /// provieds default implementations for IManagedObject interface which can be overriden
  /// subclasses need only provide Properties property
  /// </summary>
  public abstract class AbstractManagedObject : AbstractConnectableObjectValue, IManagedObject, IManagedObjectValue
  {
    #region Constructors
    public AbstractManagedObject(ManagedObjectInfo info)
      : base(info)
    {
      if (info == null) throw new ArgumentNullException("info");
    }

    #endregion

    #region Must Implement Members
    /// <summary>
    /// ony abstract member of AbstractManagedObject this needs to always return a valid enumerable 
    /// containing all IManagedProperty s associated with this Managedobject
    /// </summary>
    public abstract IEnumerable<IManagedProperty> Properties { get; }

    /// <summary>
    /// override to create a custom connection between property and object
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected virtual IObjectPropertyConnection ConstructPropertyConnection(IManagedProperty property)
    {
      var connection = new AbstractObjectPropertyConnection() ;
      return connection;
    }
    /// <summary>
    /// Subclasses need to call this when a managed property was added to properties enumerable
    /// </summary>
    /// <param name="property"></param>
    protected void NotifyPropertyAdded(IManagedProperty property)
    {
      ConnectProperty(property);
      RaisePropertyChanged("Properties");
      OnPropertyAdded(property);
    }

    /// <summary>
    /// the function should store return the same edge for the same property
    /// needs to call ContructPropertyConnection if a new edge is needed
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    protected virtual IObjectPropertyConnection RequireObjectPropertyConnection(IManagedProperty property)
    {
      return DefaultRequireObjectPropertyConnection(property);
    }
    private IObjectPropertyConnection DefaultRequireObjectPropertyConnection(IManagedProperty property)
    {
      if (property is AbstractManagedProperty)
      {
        // get edge 
        var edge = Edges.OfType<AbstractObjectPropertyConnection>().SingleOrDefault(e => e.Head == property);
        if (edge != null) return edge;
      }
      return ConstructPropertyConnection(property);
    }

    /// <summary>
    /// should connect the property to this object 
    /// </summary>
    /// <param name="property"></param>
    protected virtual void ConnectProperty(IManagedProperty property)
    {
      var connection = RequireObjectPropertyConnection(property) as AbstractObjectPropertyConnection;
      connection.SetEdge(this, property as AbstractManagedProperty);
      OnPropertyConnected(property);
    }

    /// <summary>
    /// shoud disconnect the property specified
    /// </summary>
    /// <param name="property"></param>
    protected virtual void DisconnectProperty(IManagedProperty property)
    {
      var connection = RequireObjectPropertyConnection(property) as AbstractObjectPropertyConnection;
      connection.Head = null;
      OnPropertyDisconnected(property, connection);
    }

  
    /// <summary>
    /// Subclasses need to call this when a property was remveod from properties enumerable
    /// </summary>
    /// <param name="property"></param>
    protected void NotifyPropertyRemoved(IManagedProperty property)
    {
      DisconnectProperty(property);
      RaisePropertyChanged("Properties");
      OnPropertyRemoved(property);
    }

    #endregion

    #region Value
    /// <summary>
    /// subclasses which allow an isntance to be set should implement this and also pass the corresponding Objectinfo to the construcotr
    /// </summary>
    /// <param name="value"></param>
    protected override void ConsumeValue(object value)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// subclasses which allow an isntance to be set should implement this and also pass the corresponding Objectinfo to the construcotr
    /// </summary>
    /// <param name="value"></param>
    protected override object ProduceValue()
    {
      throw new NotImplementedException();
    }


    #endregion

    #region Extension Points
    protected virtual void OnPropertyAdded(IManagedProperty property) { }
    protected virtual void OnPropertyRemoved(IManagedProperty property) { }  
    protected virtual void OnPropertyConnected(IManagedProperty property) { }
    protected virtual void OnPropertyDisconnected(IManagedProperty property, AbstractObjectPropertyConnection conenction) { }


    #endregion

    #region Default Implementations (overridable for more performance)

    public virtual bool HasProperty(string name)
    {
      return Properties.Any(p => p.PropertyInfo.Name == name);
    }


    public virtual IManagedProperty GetProperty(string name)
    {
      return Properties.SingleOrDefault(p => p.PropertyInfo.Name == name);
    }

    public virtual object GetValue(string name)
    {
      var property = GetProperty(name);
      return property.Value;
    }
    public virtual void SetValue(string name, object value)
    {
      var property = GetProperty(name);
      property.Value = value;
    }

    #endregion

    #region Convencience Accessors
    /// <summary>
    /// convenience accesor for getValue and SetValue
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object this[string name]
    {
      get
      {
        return this.GetValue(name);
      }
      set
      {
        this.SetValue(name, value);
      }
    }

    /// <summary>
    /// convenience accessor to ManagedObjectInfo
    /// </summary>
    protected new ManagedObjectInfo Info
    {
      get { return ObjectInfo as ManagedObjectInfo; }
    }

    /// <summary>
    /// IManagedObject implementation
    /// </summary>
    public IManagedObjectInfo ObjectInfo
    {
      get { return (IManagedObjectInfo)ValueInfo; }
    }

    #endregion

  }

}
