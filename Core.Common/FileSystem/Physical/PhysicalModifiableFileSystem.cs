using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{

  public class PhysicalModifiableFileSystem : DelegateFileSystem, IFileSystem
  {
    private DelegateReadonlyFileSystem readonlyFileSystem;
    public PhysicalModifiableFileSystem()
    {
      readonlyFileSystem = new PhysicalReadonlyFileSystem();
      SetImplementation(readonlyFileSystem);
      DelegateCreateDirectory = path => Directory.CreateDirectory(path);
      DelegateCreateFile = File.Create;
      DelegateDeleteDirectory = Directory.Delete;
      DelegateDeleteFile = File.Delete;
      DelegateOpenFile = PhsyicalOpenFile;
      DelegateTouchFile = PhsyicalTouchFile;
    }

    private void PhsyicalTouchFile(string path)
    {
      FileStream myFileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
      myFileStream.Close();
      myFileStream.Dispose();
      File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
    }

    private Stream PhsyicalOpenFile(string path, FileAccess access)
    {
      if (access == FileAccess.Read) return File.Open(path, FileMode.Open, FileAccess.Read);
      return File.Open(path, FileMode.Create, access);
    }

  }
}
