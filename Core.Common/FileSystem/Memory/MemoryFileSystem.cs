using Core.FileSystem.Memory;
using Core.Strings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem
{


  public class MemoryFileSystem : DelegateFileSystem
  {
    /// <summary>
    /// the filesystem is based on this dictionary 
    /// creating a entry for every folder and file
    /// </summary>
    public IDictionary<string, MemoryFileSystemEntry> entries = new Dictionary<string, MemoryFileSystemEntry>();


    public MemoryFileSystem(IEnumerable<TextFile> initialFiles)
      : this()
    {
      this.AddTextFiles(initialFiles);
    }
    public MemoryFileSystem()
    {
      AutoNormalize = true;
      DelegateCreateDirectory = MemoryCreateDirectory;
      DelegateCreateFile = MemoryCreateFile;
      DelegateDeleteDirectory = MemoryDeleteDirectory;
      DelegateDeleteFile = MemoryDeleteFile;
      DelegateExists = ExistsMemory;
      DelegateGetEntries = GetMemoryEntries;
      DelegateGetCacheKey = GetMemoryCacheKey;
      DelegateIsDirectory = IsDirectoryMemory;
      DelegateIsFile = MemoryIsFile;
      DelegateNormalizePath = MemoryNormalizePath;
      DelegateOpenFile = MemoryOpenFile;
      DelegateOpenRead = MemoryOpenFile;
      DelegateToAbsolutePath = MemoryToAbsolutePath;
      DelegateGetCreationTime = MemoryGetCreationTime;
      DelegateGetLastAccessTime = MemoryGetLastAccessTime;
      DelegateGetLastWriteTime = MemoryGetLastWriteTime;
      DelegateTouchFile = MemoryTouchFile;
      CreateDirectory("~/");

    }

    private void MemoryTouchFile(string obj)
    {
      var entry = GetEntry(obj);
      if (entry != null)
      {

        entry.Touch();
        return;


      }
      CreateFile(obj);

    }

    private DateTime MemoryGetLastWriteTime(string arg)
    {
      var entry = GetEntryOrThrow(arg);
      return entry.LastWriteTime;
    }

    private DateTime MemoryGetLastAccessTime(string arg)
    {
      var entry = GetEntryOrThrow(arg);
      return entry.LastAccessTime;
    }

    private DateTime MemoryGetCreationTime(string arg)
    {
      var entry = GetEntryOrThrow(arg);
      return entry.CreationTime;
    }

    private bool MemoryIsFile(string arg)
    {
      return Exists(arg) && !IsDirectory(arg);
    }

    private string MemoryToAbsolutePath(string arg)
    {
      return arg;
    }

    private Stream MemoryOpenFile(string arg)
    {
      return OpenFile(arg, FileAccess.Read);
    }

    private Stream MemoryOpenFile(string arg1, FileAccess arg2)
    {
      if (!this.IsFile(arg1)) throw new FileNotFoundException("", "arg1");


      var entry = GetEntry(arg1);
      switch (arg2)
      {
        case FileAccess.Read:
          return new MemoryStream(entry.Data);
        case FileAccess.Write:
          return new MemoryFileSystemEntryStream(entry);
      }
      throw new NotSupportedException("memory filesystem can only open either in read mode or write mode");
    }

    private string MemoryNormalizePath(string arg)
    {
      var result = RelativePathUtility.Normalize(arg);
      return result;
    }
    void RemoveEntry(string path)
    {
      if (!HasEntry(path)) return;
      entries.Remove(path);
    }
    private void MemoryDeleteFile(string path)
    {
      if (!this.IsFile(path)) throw new FileDoesNotExistException(path);
      var entry = GetEntry(path);
      RemoveEntry(path);
    }

    private void MemoryDeleteDirectory(string path, bool recurse)
    {
      if (!IsDirectory(path)) throw new DirectoryDoesNotExistException(path);
      var entry = GetEntry(path);
      var children = GetEntries(path);
      if (!recurse && children.Count() > 0) throw new DirectoryIsNotEmptyException(path);

      foreach (var child in children)
      {
        if (IsDirectory(child)) DeleteDirectory(child, true);
        else Delete(child);
      }
      RemoveEntry(path);
    }

    private Stream MemoryCreateFile(string path)
    {
      if (Exists(path)) throw new PathExistsException(path);
      var directory = NormalizePath(Path.GetDirectoryName(path));
      if (!IsDirectory(directory)) throw new DirectoryDoesNotExistException(directory);
      var parent = GetEntry(directory);

      var entry = new MemoryFileSystemEntry() { IsDirectory = false };

      AddEntry(path, entry);
      return OpenFile(path, FileAccess.Write);

    }

    private void MemoryCreateDirectory(string path)
    {
      if (Exists(path)) throw new PathExistsException(path);
      if (path == "~")
      {
        if (IsDirectory("~/")) return;
        AddEntry("~/", new MemoryFileSystemEntry() { IsDirectory = true });
        return;
      }
      var currentPath = path;

      while (currentPath != "~")
      {
        currentPath = currentPath.Substring(0, currentPath.LastIndexOf('/'));
        if (!IsDirectory(currentPath))
        {
          MemoryCreateDirectory(currentPath);
        }
      }

      AddEntry(path, new MemoryFileSystemEntry() { IsDirectory = true });

    }

    void AddEntry(string path, MemoryFileSystemEntry entry)
    {
      entries[path] = entry;
    }

    private bool IsDirectoryMemory(string arg)
    {
      var entry = GetEntry(arg);
      if (entry == null) return false;
      return entry.IsDirectory;
    }

    private bool ExistsMemory(string arg)
    {
      return HasEntry(arg);
    }
    private bool HasEntry(string path)
    {
      return entries.ContainsKey(path);
    }
    private MemoryFileSystemEntry GetEntry(string path)
    {
      if (!HasEntry(path)) return null;
      return entries[path];
    }
    private MemoryFileSystemEntry GetEntryOrThrow(string path)
    {
      if (!HasEntry(path)) throw new PathNotFoundException("memory filesystem could not find path", path);
      return GetEntry(path);
    }

    private string GetMemoryCacheKey(string arg)
    {
      if (!this.IsFile(arg)) throw new FileNotFoundException("memory filesystem could not find file", arg);
      var entry = GetEntry(arg);
      return entry.CacheKey;
    }



    private IEnumerable<string> GetMemoryEntries(string path)
    {
      if (!IsDirectory(path)) throw new DirectoryDoesNotExistException(path);
      if (!path.EndsWith("/")) path = path + "/";
      var result = entries.Keys
        .Where(entry =>
        {
          var res =
          entry.StartsWith(path) &&
          entry.IndexOf('/', path.Length) == -1 &&
          entry != path;
          return res;
        }).ToArray(); ;


      return result;
    }

  }
}
