using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
  public class DelegateFileSystem : DelegateReadonlyFileSystem, IFileSystem
  {
    protected void SetImplementation(IFileSystem fileSystem)
    {
      DelegateOpenFile = fileSystem.OpenFile;
      DelegateCreateFile = fileSystem.CreateFile;
      DelegateDeleteFile = fileSystem.Delete;
      DelegateDeleteDirectory = fileSystem.DeleteDirectory;
      DelegateCreateDirectory = fileSystem.CreateDirectory;
      DelegateTouchFile = fileSystem.TouchFile;
      base.SetImplementation(fileSystem);
    }

    public virtual Func<string, FileAccess, Stream> DelegateOpenFile { get; set; }
    public virtual Func<string, Stream> DelegateCreateFile { get; set; }
    public virtual Action<string> DelegateDeleteFile { get; set; }
    public virtual Action<string, bool> DelegateDeleteDirectory { get; set; }
    public virtual Action<string> DelegateCreateDirectory { get; set; }
    public virtual Action<string> DelegateTouchFile { get; set; }
    public virtual Stream OpenFile(string path, FileAccess access)
    {
      
      if (DelegateOpenFile == null)
      {
        if (access == FileAccess.Read)
        {
          return OpenRead(path);
        }
        throw new NotSupportedException();
      }
      return DelegateOpenFile(ToInputPath(path), access);
    }


    public virtual Stream CreateFile(string path)
    {
      if (DelegateCreateFile == null) throw new NotSupportedException();
      return DelegateCreateFile(ToInputPath(path));

    }

    public virtual void CreateDirectory(string path)
    {
      if (DelegateCreateDirectory == null) throw new NotSupportedException();
      DelegateCreateDirectory(ToInputPath(path));
    }

    public virtual void Delete(string path)
    {
      if (DelegateDeleteFile == null) throw new NotSupportedException();
      if (IsDirectory(path)) DeleteDirectory(path, false);
      else DelegateDeleteFile(ToInputPath(path));
    }

    public virtual void DeleteDirectory(string path, bool recurse)
    {
      if (DelegateDeleteDirectory == null) throw new NotSupportedException();
      DelegateDeleteDirectory(ToInputPath(path), recurse);
    }


    public void TouchFile(string path)
    {
      if (DelegateTouchFile == null) throw new NotSupportedException("TouchFile is not supported");
      DelegateTouchFile(ToInputPath(path));
    }
  }


}
