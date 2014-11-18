using System;

namespace Core.Modules.Applications
{
  public interface IModifiableModuleInstance : IModuleInstance
  {
    bool IsActive { get; set; }
    object Module { get; set; }

  }
}
