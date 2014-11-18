using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  /// <summary>
  ///  Base class for managed object 
  /// </summary>
  public class ManagedObjectBase : AbstractManagedObject
  {
    /// <summary>
    /// constructor needs a managedobjectinfo object
    /// </summary>
    /// <param name="info"></param>
    public ManagedObjectBase(ManagedObjectInfo info) : base(info) { }

    /// <summary>
    /// ManagedObjectBase uses a default dictionary to store properties
    /// </summary>
    private IDictionary<string, IManagedProperty> properties = new Dictionary<string, IManagedProperty>();
    private IDictionary<string, IObjectPropertyConnection> connections = new Dictionary<string, IObjectPropertyConnection>();

    /// <summary>
    /// caches the connection in a dictionary
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    protected override IObjectPropertyConnection RequireObjectPropertyConnection(IManagedProperty property)
    {
      string name = property.PropertyInfo.Name;
      if (!connections.ContainsKey(name))
      {
        var result = ConstructPropertyConnection(property);
        connections[name] = result;
        return result;
      }
      return connections[name];
    }
    /// <summary>
    /// subclasses may implement this to return a more specialized ManagedProperty
    /// the default implementats returns MemoryManagedProperty
    /// </summary>
    /// <param name="info"></param>
    /// <param name="initialValue"></param>
    /// <returns></returns>
    protected virtual IManagedProperty ConstructProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      return ManagedProperty.Memory(info, initialValue.Value);
    }

    /// <summary>
    /// subclasses need to call this method when they require a Managed proeprty to exist
    /// </summary>
    /// <param name="info"></param>
    /// <param name="initialValue"></param>
    /// <returns></returns>
    protected IManagedProperty RequireProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      if (HasProperty(info.Name)) return GetProperty(info.Name);
      var property = ConstructProperty(info, initialValue);
      properties[info.Name] = property;
      NotifyPropertyAdded(property);
      return property;
    }

    /// <summary>
    ///  subclasses need to call this when they want to remove a proeprty
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    protected bool RemoveProperty(string name)
    {
      if (!HasProperty(name)) return false;
      var property = GetProperty(name);
      return RemoveProperty(property);
    }

    protected bool RemoveProperty(IManagedProperty property)
    {
      if (!properties.Remove(property.PropertyInfo.Name)) return false;
      NotifyPropertyRemoved(property);
      return true;
    }



    #region AbstractManagedProperty specialization
    /// <summary>
    /// faster implementation for HasProperty O(1)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public override bool HasProperty(string name)
    {
      return properties.ContainsKey(name);
    }
    /// <summary>
    /// O(1) implementation 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public override IManagedProperty GetProperty(string name)
    {
      try
      {//if (!HasProperty(name)) throw new ManagedPropertyNotFoundException(name);
        return properties[name];
      }
      catch
      {
        throw new ManagedPropertyNotFoundException(name);
      }
    }

    public override IEnumerable<IManagedProperty> Properties
    {
      get { return properties.Values; }
    }
    #endregion
  }
}
