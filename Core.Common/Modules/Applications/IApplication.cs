using System;
using System.Collections.Generic;

namespace Core.Modules.Applications
{
  /// <summary>
  /// IApplication is the base interface for a module application
  /// It contains methods for managing, activating and deactivating  Modules
  /// </summary>
  public interface IApplication
  {
    /// <summary>
    /// returns a information object on the application
    /// </summary>
    IApplicationInformation ApplicationInfo { get; }
    /// <summary>
    /// returns a list of known modules
    /// these modules can be activated/deactivated
    /// </summary>
    IEnumerable<IModuleInstance> Modules { get; }
    /// <summary>
    /// actives a specific module (it must come from the Modules enumerable)
    /// </summary>
    /// <param name="instance"></param>
    void ActivateModule(IModuleInstance instance);
    /// <summary>
    /// deactivates a specifiec module (from Modules enumerable)
    /// </summary>
    /// <param name="instance"></param>
    void DeactivateModule(IModuleInstance instance);
    /// <summary>
    /// adds a type as a module
    /// throws if Type cannot be used as a module
    /// </summary>
    /// <param name="moduleType"></param>
    void AddModule(Type moduleType);
    /// <summary>
    /// removes a module type
    /// </summary>
    /// <param name="moduleType"></param>
    void RemoveModule(Type moduleType);
    /// <summary>
    /// returns true if the type is already registered as a module
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    bool HasModule(Type moduleType);
    /// <summary>
    /// gets the module instance for a specific type
    /// </summary>
    /// <param name="moduleType"></param>
    /// <returns></returns>
    IModuleInstance GetModule(Type moduleType);
  }


}
