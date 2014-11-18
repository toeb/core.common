using System.Collections.Generic;
using Core.FileSystem;
using Core.Resources;

namespace Core.Modules.Installation.AssemblyResources
{
  public class AssemblyResourceInstallation : IAssemblyResourceInstallation
  {
    public InstallationInstance Installation { get; set; }
    public IRelativeFileSystem FileSystem { get; set; }
    public IRelativeFileSystem ProjectFileSystem { get; set; }
    public IEnumerable<IManagedResource> Resources { get; set; }
  }
}