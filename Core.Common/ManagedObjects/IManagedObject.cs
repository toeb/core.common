using Core.Values;
using System;
using System.Collections.Generic;

namespace Core.ManagedObjects
{


  public class ManagedPropertyNotFoundException :Exception{
    private string name;

    public ManagedPropertyNotFoundException(string name)
    {
      // TODO: Complete member initialization
      this.name = name;
    }
  }
  public interface IManagedObject : IConnectableObject
  {
    /// <summary>
    /// returns the value of the underlying property
    /// (may also be an enumerable type) 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    object this[string name]
    {
      get;
      set;
    }

    /// <summary>
    /// needs returns true Properties contains a proeprty identified by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool HasProperty(string name);

    /// <summary>
    /// needs to return an enumerable of properties associated with this object
    /// </summary>
    IEnumerable<IManagedProperty> Properties { get; }
    
    /// <summary>
    /// needs to return the property identified by name if property does nto exist it should throw an PropertyNotFoundException
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IManagedProperty GetProperty(string name);

    /// <summary>
    /// should return value of proeprty identified by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    object GetValue( string name);
    /// <summary>
    /// should set value of property identified by name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void SetValue(string name, object value);


  }


  
}
