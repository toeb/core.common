using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{

  public class DelegateReadonlyFileSystem : IReadonlyFileSystem
  {

    /// <summary>
    /// Filters GetEntries by whether or not entry is a file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerable<string> DefaultGetFiles(string path)
    {
      return GetEntries(path).Where(entry => IsFile(entry));
    }
    /// <summary>
    /// Filters GetEntries by whether or not entry is a directory
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerable<string> DefaultGetDirectories(string path)
    {
      return GetEntries(path).Where(entry => IsDirectory(entry));
    }
    /// <summary>
    /// combines GetFiles and GetDirectories
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerable<string> DefaultGetEntries(string path)
    {
      return GetDirectories(path).Concat(GetFiles(path));
    }
    public bool DefaultExists(string path)
    {
      return IsFile(path) || IsDirectory(path);
    }
    public string DefaultGetCacheKey(string path)
    {
      if (DelegateGetLastWriteTime == null) return null;
      var time = GetLastWriteTime(path);
      var filename = Path.GetFileName(path);
      return time.Ticks + "_" + filename;
    }
    protected void SetImplementation(IReadonlyFileSystem fs)
    {
      DelegateGetEntries = fs.GetEntries;
      DelegateExists = fs.Exists;
      DelegateIsDirectory = fs.IsDirectory;
      DelegateOpenRead = fs.OpenRead;
      DelegateNormalizePath = fs.NormalizePath;
      DelegateToAbsolutePath = fs.ToAbsolutePath;
      DelegateIsFile = fs.IsFile;
      DelegateGetCacheKey = fs.GetCacheKey;
      DelegateGetFiles = fs.GetFiles;
      DelegateGetDirectories = fs.GetDirectories;
    }

    public bool AutoNormalize { get; set; }

    public virtual Func<string, IEnumerable<string>> DelegateGetEntries { get; set; }
    public virtual Func<string, IEnumerable<string>> DelegateGetFiles { get; set; }
    public virtual Func<string, IEnumerable<string>> DelegateGetDirectories { get; set; }
    public virtual Func<string, bool> DelegateExists { get; set; }
    public virtual Func<string, bool> DelegateIsDirectory { get; set; }
    public virtual Func<string, Stream> DelegateOpenRead { get; set; }
    public virtual Func<string, string> DelegateNormalizePath { get; set; }
    public virtual Func<string, string> DelegateToAbsolutePath { get; set; }
    public virtual Func<string, bool> DelegateIsFile { get; set; }
    public virtual Func<string, string> DelegateGetCacheKey { get; set; }
    public virtual Func<string, DateTime> DelegateGetLastAccessTime { get; set; }
    public virtual Func<string, DateTime> DelegateGetLastWriteTime { get; set; }
    public virtual Func<string, DateTime> DelegateGetCreationTime { get; set; }
    public virtual IEnumerable<string> GetEntries(string path)
    {
      if (DelegateGetEntries == null) throw new NotSupportedException("Filesystem does not support GetEntries");
      var input = ToInputPath(path);
      var result = DelegateGetEntries(input);
      var output = result.Select(entry => ToOutputPath(entry));
      return output;
    }
    protected virtual string TransformInputPath(string path) { return path; }
    protected virtual string TransformOutputPath(string path) { return path; }
    public virtual bool Exists(string path)
    {
      if (DelegateExists == null) throw new NotSupportedException("filesystem does not support Exists()");
      var input = ToInputPath(path);
      return DelegateExists(input);
    }

    public virtual bool IsDirectory(string path)
    {
      if (DelegateIsDirectory == null) throw new NotSupportedException("filesystem does not support IsDirectory");
      var input = ToInputPath(path);
      return DelegateIsDirectory(input);
    }

    public virtual System.IO.Stream OpenRead(string path)
    {
      if (DelegateOpenRead == null) throw new NotSupportedException("filesystem does not support openfile");
      var input = ToInputPath(path);
      return DelegateOpenRead(input);
    }

    public virtual string AutoNormalizePath(string path)
    {
      if (!AutoNormalize) return path;
      return NormalizePath(path);
    }

    public virtual string ToAbsolutePath(string path)
    {
      if (DelegateToAbsolutePath == null) throw new NotSupportedException("filesystem does not support to absolute paht");
      var input = ToInputPath(path);
      var result = DelegateToAbsolutePath(input);
      var output = ToOutputPath(result);
      return output;
    }


    public virtual string NormalizePath(string path)
    {
      if (DelegateNormalizePath == null) return path;
      var result = DelegateNormalizePath(path);
      return result;
    }


    public bool IsFile(string path)
    {
      if (DelegateIsFile == null) throw new NotSupportedException("IsFile is not supported");
      var input = ToInputPath(path);
      return DelegateIsFile(input);
    }


    public string GetCacheKey(string path)
    {
      if (DelegateGetCacheKey == null) throw new NotSupportedException("GetCacheKey is not supporeted");
      var input = ToInputPath(path);
      return DelegateGetCacheKey(input);
    }


    public DateTime GetLastAccessTime(string path)
    {
      if (DelegateGetLastAccessTime == null) throw new NotSupportedException("GetLastAccessTime is not supported");
      var input = ToInputPath(path);
      return DelegateGetLastAccessTime(input);
    }

    public DateTime GetCreationTime(string path)
    {
      if (DelegateGetCreationTime == null) throw new NotSupportedException("GetCreationTime is not supported");
      var input = ToInputPath(path);
      return DelegateGetCreationTime(input);
    }

    public DateTime GetLastWriteTime(string path)
    {
      if (DelegateGetLastWriteTime == null) throw new NotSupportedException("GetLastWriteTime is not supported");
      var input = ToInputPath(path);
      return DelegateGetLastWriteTime(input);
    }

    protected string ToInputPath(string path)
    {
      var normalized = AutoNormalizePath(path);
      var input = TransformInputPath(normalized);
      return input;
    }
    protected string ToOutputPath(string path)
    {
      return TransformOutputPath(path);
    }


    public IEnumerable<string> GetFiles(string path)
    {
      if (DelegateGetFiles == null) throw new NotSupportedException("GetFiles is not supported");
      var input = ToInputPath(path);
      return DelegateGetFiles(input).Select(file => ToOutputPath(file));
    }

    public IEnumerable<string> GetDirectories(string path)
    {
      if (DelegateGetDirectories == null) throw new NotSupportedException("GetDirectories is not supported");
      var input = ToInputPath(path);
      return DelegateGetDirectories(input).Select(dir => ToOutputPath(dir));
    }
  }
}
