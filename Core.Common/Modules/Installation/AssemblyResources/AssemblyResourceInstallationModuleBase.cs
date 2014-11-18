using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Installation.AssemblyResources
{
  [Module]
  [Installable("AssemblyResources")]
  public abstract class ResourceInstallationModuleBase 
  {
    [Import]
    IInstallationService InstallationService { get; set; }

    [CanInstallCallback]
    protected virtual void CanInstall()
    {
      InstallationService.Install(this);
    }
  }
}
