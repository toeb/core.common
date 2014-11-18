using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{
  /// <summary>
  /// gives access to the file system functions needed for readonly access
  /// </summary>
  public interface IReadonlyFileSystem
  {
    /// <summary>
    /// returns the entries in the specified path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerable<string> GetEntries(string path);

    /// <summary>
    /// returns the files in current path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerable<string> GetFiles(string path);

    /// <summary>
    /// returns the directories of current path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerable<string> GetDirectories(string path);

    /// <summary>
    /// returns true if the path exists
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool Exists(string path);

    /// <summary>
    /// returns true if the path exists and is a directory and not a file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool IsDirectory(string path);

    /// <summary>
    /// returns true if path describes an existing file
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool IsFile(string path);

    /// <summary>
    /// Opens a file for reading only
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Stream OpenRead(string path);

    /// <summary>
    /// normalizes the path for this filesystem
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string NormalizePath(string path);


    /// <summary>
    /// deprecated returns the full physical path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string ToAbsolutePath(string path);


    /// <summary>
    /// should return a string which is unique and only changes when the underying file changes
    /// if the fs does not support this operation return null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    string GetCacheKey(string path);

    /// <summary>
    /// should return the last time a directory or file was accessed
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    DateTime GetLastAccessTime(string path);
    /// <summary>
    /// should return the time the file was created
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    DateTime GetCreationTime(string path);
    /// <summary>
    /// should return the time the file was last written to
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    DateTime GetLastWriteTime(string path);
  }


  public static class IReadonlyFileSystemExtensions
  {
    public static TextReader OpenText(this IReadonlyFileSystem self, string path)
    {
      return new StreamReader(self.OpenRead(path));
    }
    public static string ReadFileToEnd(this IReadonlyFileSystem self, string path)
    {
      using (var reader = self.OpenText(path))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
