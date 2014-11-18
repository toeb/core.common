using System.IO;

namespace Core.Resources
{
  public interface IFileResource : IManagedResource
  {
    string FileName { get; }
    string Extension { get; }
    /// <summary>
    /// the project relative path
    /// </summary>
    string RelativePath { get; }
    /// <summary>
    /// the path to the local file on the developement machine
    /// </summary>
    string LocalPath { get; }
    Stream OpenStream();
  }
}
