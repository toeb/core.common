using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Resources;
using Core.FileSystem;
using Core.Extensions;
using System;
using System.IO;

namespace Core.Modules.Installation.AssemblyResources
{
  [Installer]
  public class AssemblyResourcesInstaller : IInstaller
  {
    public static readonly string ResourceDirectory = "AssemblyResources";
    IAssemblyResourceService ResourceService { get; set; }
    IFileSystemService FileSystemService { get; set; }

    public AssemblyResourcesInstaller(IAssemblyResourceService resourceService, IFileSystemService fileSystemService)
    {
      ResourceService = resourceService;
      FileSystemService = fileSystemService;
    }

    public IEnumerable<string> CanInstall(InstallationInstance installer)
    {
      return installer.InstallationInformation.InstallationRequirements.Where(req => req == "AssemblyResources");
    }

    public bool CheckInstall(InstallationInstance instance)
    {
      return false;
    }

    public void Install(InstallationInstance instance)
    {
      //
      var installableType = instance.Installable.GetType();
      var assembly = installableType.Assembly;
      var resources = ResourceService.GetResources(assembly);

      var fileSystem = instance.InstallationFileSystem.ScopeTo(ResourceDirectory);

      var result = new AssemblyResourceInstallation()
      {
        Installation = instance,
        FileSystem = fileSystem
      };


      // set ProjectFileSystem if it exists
      var info = assembly.GetProjectInfo();
      try
      {
        var dir = info.ProjectDir;
        var projectFs = new RelativeFileSystem(dir);
        result.ProjectFileSystem = projectFs;

      }
      catch (Exception e)
      {

      }
      
      resources.InstallResources(fileSystem, ".", true);

      result.Resources = resources.Resources;

      // share the installed resources
      FileSystemService.SharedFileSystem.AddFileSystem(result.GetDefaultFileSystem());
      
      var type = instance.Installable.GetType();
      instance.Installable.SetAttributedPropertyValues<ImportInstalledResourcesAttribute>(result, false);



    }

    public void Uninstall(InstallationInstance instance)
    {
      instance.InstallationFileSystem.DeleteDirectory(ResourceDirectory, true);



    }

  }
}
