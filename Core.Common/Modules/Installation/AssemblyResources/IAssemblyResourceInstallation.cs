using System.Collections.Generic;
using Core.FileSystem;
using Core.Resources;
using System.Configuration;
using System;

namespace Core.Modules.Installation.AssemblyResources
{
  /// <summary>
  /// interface for assembly resource installation
  /// </summary>
  public interface IAssemblyResourceInstallation
  {
    /// <summary>
    /// the filesystem where the the resources were installed (if any)
    /// </summary>
    IRelativeFileSystem FileSystem { get; }
    /// <summary>
    /// if not null this filesystem points to the original location of the resources in developement mode 
    /// @todo rename to DevelopementFileSystem
    /// </summary>
    IRelativeFileSystem ProjectFileSystem { get; }


    /// <summary>
    /// returns all managed resources 
    /// </summary>
    IEnumerable<IManagedResource> Resources { get; }
  }

  public static class IAssemblyResourceInstallationExtensions
  {




    /// <summary>
    /// returns the default filesystem depending on wether the projectfilesystem exists
    /// </summary>
    public static IRelativeFileSystem GetDefaultFileSystem(this IAssemblyResourceInstallation self, bool? developermode = null)
    {
      if (!developermode.HasValue)
      {
        developermode = false;
        try
        {
          var value = ConfigurationManager.AppSettings["installation:preferLocalFiles"];
          if (value == "true")
          {
            developermode = true;
          }
        }
        catch (Exception e)
        {

        }
      }
      
      if (self.ProjectFileSystem == null) return self.FileSystem;
      if (self.ProjectFileSystem.IsDirectory(".")) return self.ProjectFileSystem;
      return self.FileSystem;
    }
  }
}