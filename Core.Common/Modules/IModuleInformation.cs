using System;
namespace Core.Modules
{

  public interface IModuleInformation : IIdentifiable<Guid>
  {
    /// <summary>
    /// the type of the modules
    /// </summary>
    Type Type { get; }
    /// <summary>
    /// a name which is unique to the module
    /// </summary>
    string ModuleName { get; }
    /// <summary>
    /// returns the version of the plugin
    /// </summary>
    Version Version { get; }
    bool AutoActivate { get; }

    int InitializationPriority { get; }
  }
}
