using System;
using System.ComponentModel.Composition;

namespace Core.Modules
{

  /// <summary>
  /// this is an interface for activating and deactivating a module
  /// modudles need not implement this / see module attribute
  /// </summary>
  public interface IModule
  {
    [ActivationCallback]
    void Activate();
    [DeactivationCallback]
    void Deactivate();
  }




}
