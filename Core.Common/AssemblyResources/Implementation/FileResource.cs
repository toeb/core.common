using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Core.Resources

{
  [DebuggerDisplay("Resource {FileName}")]
  public class FileResource : ManagedResource, IFileResource
  {
    public FileResource(string key, Stream stream, string localPathBase, string relativePath, Assembly assembly)
      : base(key, assembly)
    {
      this.stream = stream;
      FileName = Path.GetFileName(relativePath);

      if (!string.IsNullOrEmpty(localPathBase))
      {
        LocalPath = Path.Combine(localPathBase, relativePath);
       LocalPath =   Path.GetFullPath(LocalPath);
        if (!File.Exists(LocalPath))
        {
          LocalPath = null;
        }
      }

      RelativePath = relativePath;
      Extension = Path.GetExtension(FileName);

    }
    private Stream stream;
    public string FileName { get; set; }
    public string LocalPath { get; set; }
    public Stream OpenStream() { return stream; }

    public string Extension
    {
      get;
      set;
    }

    public string RelativePath
    {
      get;
      set;
    }
  }
}
