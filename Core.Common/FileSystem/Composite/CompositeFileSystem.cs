using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.FileSystem
{
  public class CompositeFileSystem : IReadonlyFileSystem
  {
    public IList<IFileSystem> FileSystems { get; set; }
    public CompositeFileSystem()
    {
      FileSystems = new List<IFileSystem>();
    }
    public void AddFileSystem(IFileSystem fs)
    {
      FileSystems.Add(fs);
    }

    public Stream OpenFile(string path, FileAccess access)
    {
      var fs = GetFileSystemFromFilePath(path);
      if (fs == null) throw new FileNotFoundException();
      return fs.OpenFile(path, access);
    }


    public System.Collections.Generic.IEnumerable<string> GetEntries(string path)
    {
      var empty = new string[0];
      return FileSystems.SelectMany(fs =>
      {
        if (fs.Exists(path)) return fs.GetEntries(path);
        return empty;
      });
    }

    public bool Exists(string path)
    {
      var fs = GetFileSystemFromPath(path);
      return fs != null;
    }

    public bool IsDirectory(string path)
    {
      var fs = GetFileSystemFromDirectoryPath(path);
      return fs != null;

    }

    public Stream OpenRead(string path)
    {
      return OpenFile(path, FileAccess.Read);
    }

    public string NormalizePath(string path)
    {
      throw new NotSupportedException();
    }

    public string ToAbsolutePath(string path)
    {
      throw new NotSupportedException();
    }

    public bool IsFile(string path)
    {
      var fs = GetFileSystemFromFilePath(path);
      return fs != null;
    }


    public IFileSystem GetFileSystemFromPath(string path)
    {
      foreach (var fs in this.FileSystems)
      {
        if (fs.Exists(path)) return fs;
      }
      return null;
    }
    public IFileSystem GetFileSystemFromFilePath(string path)
    {
      foreach (var fs in this.FileSystems)
      {
        if (fs.IsFile(path)) return fs;
      }
      return null;
    }
   public  IFileSystem GetFileSystemFromDirectoryPath(string path)
    {
      foreach (var fs in this.FileSystems)
      {
        if (fs.IsDirectory(path)) return fs;
      }
      return null;
    }

    public string GetCacheKey(string path)
    {
      var fs = GetFileSystemFromFilePath(path);
      if (fs == null) return null;
      return fs.GetCacheKey(path);
    }


    public DateTime GetLastAccessTime(string path)
    {
      var fs = GetFileSystemFromFilePath(path);
      return fs.GetLastAccessTime(path);
    }

    public DateTime GetCreationTime(string path)
    {
      var fs = GetFileSystemFromFilePath(path);
      return fs.GetCreationTime(path);
    }

    public DateTime GetLastWriteTime(string path)
    {
      var fs = GetFileSystemFromFilePath(path);
      return fs.GetLastWriteTime(path);
    }


    public IEnumerable<string> GetFiles(string path)
    {
      if (!IsDirectory(path)) yield break ;
      
      foreach (var fs in FileSystems)
      {
        if (fs.IsDirectory(path))
        {
          foreach (var file in fs.GetFiles(path))
          {
            yield return file;
          }
        }
      }
    }

    public IEnumerable<string> GetDirectories(string path)
    {
      if (!IsDirectory(path)) yield break;

      foreach (var fs in FileSystems)
      {
        if (fs.IsDirectory(path))
        {
          foreach (var dir in fs.GetDirectories(path))
          {
            yield return dir;
          }
        }
      }
 
    }
  }
}