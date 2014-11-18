using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  public interface IExtensibleManagedObject : IManagedObject
  {

    /// <summary>
    /// returns the property specified by info.Name
    /// or creates it and returns it
    /// </summary>
    /// <param name="info"></param>
    /// <param name="initialValue"></param>
    /// <returns></returns>
    IManagedProperty RequireProperty(IPropertyInfo info, Lazy<object> initialValue);
    /// <summary>
    /// removes the property specified returns false if property does not exist in this object or cannot be removed
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool RemoveProperty(IManagedProperty property);
  }
}
