using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Core.FileSystem;
using System.IO;
using System.Text;

namespace Core.FileSystem
{
  [Export]
  [Export(typeof(IFileSystemService))]
  public class FileSystemService : IFileSystemService
  {
    [ImportingConstructor]
    public FileSystemService([Import("ApplicationRootDirectory", AllowDefault = true)]string root = null)
    {
      fileSystem = new PhysicalFileSystem();
      if (string.IsNullOrEmpty(root))
      {
        root = fileSystem.NormalizePath(".");
      }
      applicationFS = new RelativeFileSystem(fileSystem, root);
      SharedFileSystem = new CompositeFileSystem();
      SharedFileSystem.AddFileSystem(ApplicationFileSystem);
    }
    private PhysicalFileSystem fileSystem;
    private RelativeFileSystem applicationFS;

    public CompositeFileSystem SharedFileSystem
    {
      get;
      set;
    }

    public IFileSystem FileSystem
    {
      get { return fileSystem; }
    }

    public IRelativeFileSystem ApplicationFileSystem
    {
      get
      {
        return applicationFS;
      }
    }
  }
}
