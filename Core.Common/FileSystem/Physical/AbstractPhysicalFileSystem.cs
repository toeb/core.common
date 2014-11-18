using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{

  /*
  public abstract class AbstractPhysicalFileSystem : IFileSystem
  {
    

    protected abstract void Execute(Action action);

    /// <summary>
    /// transform the external path to internal representation
    /// </summary>
    /// <param name="virtualPath"></param>
    /// <returns></returns>
    protected abstract string ToInternalPath(string externalPath);
    /// <summary>
    /// transform the internal representation to external representation
    /// </summary>
    /// <param name="absolutePath"></param>
    /// <returns></returns>
    protected abstract string ToExternalPath(string internalPath);

    public abstract string NormalizePath(string path);



    public Stream OpenFile(string path, FileAccess access)
    {
      var absolutePath = ToInternalPath(path);
      Stream stream = null;
      Action action = () => stream = File.Open(absolutePath, FileMode.Open, access);
      Execute(action);
      return stream;
    }

    public Stream CreateFile(string path)
    {
      var absolutePath = ToInternalPath(path);
      Stream stream = null;
      Action action = () => stream = File.Create(absolutePath);
      Execute(action);
      return stream;
    }

    public void CreateDirectory(string path)
    {
      var absolutePath = ToInternalPath(path);
      Action action = () => Directory.CreateDirectory(absolutePath);
      Execute(action);

    }

    public void Delete(string path)
    {
      var absolutePath = ToInternalPath(path);
      Action action = () => File.Delete(absolutePath);
      Execute(action);
    }

    public IEnumerable<string> GetEntries(string path)
    {
      IEnumerable<string> result = null;
      var absolutePath = ToInternalPath(path);
      Action action = () => result = Directory.GetFileSystemEntries(absolutePath);
      Execute(action);
      result = result.Select(p => ToExternalPath(p));
      return result;
    }

    public bool Exists(string path)
    {
      var absolutePath = ToInternalPath(path);
      bool result = false;
      Action action = () => result = File.Exists(absolutePath) || Directory.Exists(absolutePath);
      Execute(action);
      return result;
    }

    public bool IsDirectory(string path)
    {
      var absolutePath = ToInternalPath(path);
      bool result = false;
      Action action = () => result = Directory.Exists(absolutePath);
      Execute(action);
      return result;

    }

    public Stream OpenRead(string path)
    {
      return OpenFile(path, FileAccess.Read);
    }



    public void DeleteDirectory(string path, bool recurse)
    {
      path = ToInternalPath(path);
      Action action = () => Directory.Delete(path, recurse);
      Execute(action);

    }


    public string ToAbsolutePath(string path)
    {
      return ToInternalPath(path);
    }





    public string GetCacheKey(string path)
    {
      path = ToAbsolutePath(path);
      var time = File.GetLastWriteTime(path);
      var combined = path + time.Ticks;
      path = path.Replace('/', '_');
      path = path.Replace('\\', '_');
      var hash = path;//Cryptography.HashASCIIString(combined);
      return hash;
    }


    public bool IsFile(string path)
    {
      return Exists(path) && !IsDirectory(path);
    }


    public void TouchFile(string path)
    {
      path = ToAbsolutePath(path);
      FileStream myFileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
      myFileStream.Close();
      myFileStream.Dispose();
      File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
    }
  }*/
}
