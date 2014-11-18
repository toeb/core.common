using Core.FileSystem;
using Core.Resources;
using System;
using System.IO;
using System.Linq;

namespace Core.Resources
{
  public static class IResourceExtensions
  {
    /// <summary>
    /// installs the file resource to path using the specified fileSystem
    /// if linkLocal is true and file contains a vaild local path
    /// the file will be hardlinked
    /// </summary>
    /// <param name="file"></param>
    /// <param name="fileSystem"></param>
    /// <param name="path"></param>
    /// <param name="linkLocal"></param>
    public static void InstallFileResource(this IFileResource file, IFileSystem fileSystem, string path, bool linkLocal)
    {
      if (linkLocal&&file.LocalPath != null && File.Exists(file.LocalPath))
      {
        var p2 = fileSystem.NormalizePath(path);
        path = fileSystem.ToAbsolutePath(p2);
        path = path.Replace("\\\\", "\\");
        // create a link to local path instead of a copy
        var res = Ntfs.CreateHardLink(path, file.LocalPath, IntPtr.Zero);
      }
      else
      {
        // create the file from the resoure
        using (var newFile = fileSystem.CreateFile(path))
        using (var resourceFile = file.OpenStream())
        {
          resourceFile.CopyTo(newFile);
        }
      }
    }

    public static void InstallResources(this IAssemblyResources resources, IFileSystem fileSystem, string path,bool linkLocalFiles)
    {
      foreach (var file in resources.Resources.OfType<IFileResource>())
      {
        var filePath = fileSystem.NormalizePath(Path.Combine(path, file.RelativePath));
        // ensure directory is created
        var dir = Path.GetDirectoryName(filePath);
        fileSystem.CreateDirectory(dir);
        file.InstallFileResource(fileSystem, filePath, linkLocalFiles);
      }
    }
  }
}
