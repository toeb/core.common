using Core.Modules;
using Core.Modules.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Applications
{
  /// <summary>
  /// base class for modules, imports standard stuff like
  /// * applicaiton container
  /// * module container
  /// * module instance
  /// *
  /// </summary>
  [Module]
  public abstract class ModuleBase
  {
    protected ModuleBase(bool install)
    {

    }

    [ApplicationContainer]
    protected CompositionContainer ApplicationContainer { get; set; }
    [ModuleContainer]
    protected CompositionContainer ModuleContainer { get; set; }

    [ModuleInstance]
    protected IModuleInstance ModuleInstance { get; set; }
    [ActivationCallback]
    protected virtual void Activate() { }
    [DeactivationCallback]
    protected virtual void Deactivate() { }



  }
}
