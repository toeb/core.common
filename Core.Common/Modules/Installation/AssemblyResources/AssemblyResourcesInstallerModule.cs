using System.ComponentModel.Composition;
using Core.Resources;
using Core.Modules;
using Core.FileSystem;
namespace Core.Modules.Installation.AssemblyResources
{
  /// <summary>
  /// adds the assembly resource installer to the installation service
  /// requires 
  /// * IInstallationService
  /// * IAssemblyResourceService
  /// * IFileSystemService
  /// </summary>
  [Module]
  public class AssemblyResourcesInstallerModule
  {
    [Import]
    IInstallationService InstallationService { get; set; }

    [Import]
    IAssemblyResourceService ResourceService { get; set; }

    [Import]
    IFileSystemService FileSystemService { get; set; }


    public AssemblyResourcesInstaller AssemblyResourcesInstaller { get; set; }
    [ActivationCallback]
    private void Activate()
    {
      
      AssemblyResourcesInstaller = new AssemblyResourcesInstaller(ResourceService,FileSystemService);
      InstallationService.AddInstaller(AssemblyResourcesInstaller);
    }
  }
}