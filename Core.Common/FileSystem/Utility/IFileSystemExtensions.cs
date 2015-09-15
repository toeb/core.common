using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Trying;
namespace Core.FileSystem
{

  public struct TextFile
  {
    public string Path { get; set; }
    public string Content { get; set; }
  }
  public static class IFileSystemExtensions
  {
    /// <summary>
    /// convenience method for quickly adding files to a filesystem
    /// creates all directories and files specified
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="files"></param>
    public static void AddTextFiles(this IFileSystem fs, IEnumerable<TextFile> files)
    {
      
      foreach (var file in files)
      {
        var dir = Path.GetDirectoryName(file.Path);

        fs.EnsureDirectoryExists(dir);
        fs.WriteTextFile(file.Path, file.Content);
      }
    }
    /// <summary>
    /// Scopes to the specified path and returns a relative filesystem based there
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static IRelativeFileSystem ScopeTo(this IFileSystem fs, string path)
    {
      var relFs = new RelativeFileSystem(fs, path);
      return relFs;
    }
    /// <summary>
    /// Utility method to write a text to a file
    /// </summary>
    /// <param name="path"></param>
    /// <param name="content"></param>
    public static void WriteTextFile(this IFileSystem fs, string path, string content, bool createDirs = false)
    {
      if (createDirs)
      {
        fs.EnsureFileExists(path);
      }
      using (var writer = new StreamWriter(fs.CreateOrOpen(path)))
      {
        writer.Write(content);
      }
    }

    
    /// <summary>
    /// Utility method to read a text file to its end
    /// </summary>
    /// <param name="self"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadTextFile(this IFileSystem self, string path)
    {
      using (var reader = new StreamReader(self.OpenRead(path)))
      {
        return reader.ReadToEnd();
      }

    }

    /// <summary>
    /// creates the file if it does not exist
    /// </summary>
    /// <param name="self"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Stream CreateOrOpen(this IFileSystem self, string path)
    {
      if (self.Exists(path)) return self.OpenFile(path, FileAccess.Write);
      return self.CreateFile(path);
    }
    /// <summary>
    /// returns all entries of the specified path, retrying up to n times
    /// </summary>
    /// <param name="self"></param>
    /// <param name="path"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static IEnumerable<string> TryGetEntries(this IFileSystem self, string path, int n = 15)
    {
      IEnumerable<string> result = null;
      Action action = () => result = self.GetEntries(path);
      action.TryRepeatException(n);
      return result;
    }

    public static void TryCreateDirectory(this IFileSystem self, string path, int retries = 15)
    {
      Action action = () => self.CreateDirectory(path);
      action.TryRepeatException(retries);
    }
    public static void TryDelete(this IFileSystem self, string path, int retries = 15)
    {
      Action action = () => self.Delete(path);
      action.TryRepeatException(retries);
    }
    public static Stream TryCreateFile(this IFileSystem self, string path, int retries = 15)
    {
      Stream result = null;
      Action action = () => result = self.CreateFile(path);
      action.TryRepeatException(retries);
      return result;
    }
    public static Stream TryOpenFile(this IFileSystem self, string path, FileAccess accesss, int retries = 15)
    {

      Stream result = null;
      Action action = () => result = self.OpenFile(path, accesss);
      action.TryRepeatException(retries);
      return result;
    }

    public static IEnumerable<string> RecursiveGetFiles(this IReadonlyFileSystem self, string path)
    {
      foreach (var entry in self.GetEntries(path))
      {
        if (self.IsDirectory(entry))
        {
          foreach (var file in self.RecursiveGetFiles(entry))
          {
            yield return file;
          }
        }
        else
        {
          yield return entry;
        }
      }
    }
    public static IEnumerable<string> RecursiveGetEntries(this IReadonlyFileSystem self, string path)
    {
      foreach (var entry in self.GetEntries(path))
      {
        if (self.IsDirectory(entry))
        {
          foreach (var file in self.RecursiveGetEntries(entry))
          {
            yield return file;
          }
        }
        yield return entry;
      }
    }

    public static IEnumerable<string> RecursiveGetDirectories(this IReadonlyFileSystem self, string path)
    {
      foreach (var entry in self.GetEntries(path))
      {
        if (self.IsDirectory(entry))
        {
          yield return entry;

          foreach (var dir in self.RecursiveGetDirectories(entry))
          {
            yield return dir;
          }
         
        }
      }
    }
    



    public static bool EnsureFileExists(this IFileSystem self, string path)
    {
      if (!self.Exists(path))
      {
        var p = Path.GetDirectoryName(path);
        self.EnsureDirectoryExists(p);
        self.CreateFile(path).Close();
        return true;
      }
      else if (self.IsDirectory(path))
      {
        throw new Exception("there is a directory where a file should be");
      }
      else
      {
        //ok
      }
      return false;
    }
    public static bool EnsureDirectoryExists(this IFileSystem self, string path)
    {
      if (!self.Exists(path))
      {
        self.CreateDirectory(path);
        return true;
      }
      else if (!self.IsDirectory(path))
      {
        throw new Exception("there is a file where the directory should be");
      }
      else
      {
        // ok
      }
      return false;
    }

    public static IRelativeFileSystem MakeRelative(this IFileSystem fileSystem, string path = ".")
    {
      return new RelativeFileSystem(fileSystem, path);
    }
  }
}
