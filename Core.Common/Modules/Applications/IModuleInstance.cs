using System;

namespace Core.Modules.Applications
{
  /// <summary>
  /// represents a Module in an Application
  /// if can be activated or deactivated via the IApplication interface
  /// 
  /// </summary>
  public interface IModuleInstance : IIdentifiable<Type>, IIdentifiable<Guid>
  {
    /// <summary>
    /// reference to the Application to which this module instance belongs
    /// </summary>
    IApplication Application { get; }
    /// <summary>
    /// returns true if the module is active
    /// </summary>
    bool IsActive { get; }
    /// <summary>
    /// returns the actual instance of the module (this instance exists only once for any application)
    /// </summary>
    object Module { get; }
    /// <summary>
    /// contains information about the module
    /// </summary>
    IModuleInformation ModuleInfo { get; }
  }

}
