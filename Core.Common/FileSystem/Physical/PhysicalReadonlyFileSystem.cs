using System.IO;

namespace Core.FileSystem
{


  public class PhysicalReadonlyFileSystem : DelegateReadonlyFileSystem, IReadonlyFileSystem
  {
    public PhysicalReadonlyFileSystem()
    {
      AutoNormalize = true;
      DelegateNormalizePath = Path.GetFullPath;
      DelegateOpenRead = File.OpenRead;
      DelegateGetEntries = Directory.EnumerateFileSystemEntries;
      DelegateIsFile = File.Exists;
      DelegateIsDirectory = Directory.Exists;
      DelegateExists = DefaultExists;
      DelegateGetLastAccessTime = File.GetLastAccessTime;
      DelegateGetLastWriteTime = File.GetLastWriteTime;
      DelegateGetCreationTime = File.GetCreationTime;
      DelegateGetCacheKey = DefaultGetCacheKey;
      DelegateGetFiles = Directory.GetFiles;
      DelegateGetDirectories = Directory.GetDirectories;
    }
  }
}
