using Core.Converters;
using Core.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{
  /// <summary>
  /// Base class for File based key value stores.  
  /// Requires dependencies to be injected
  /// </summary>
  /// <typeparam name="TKey"></typeparam>
  /// <typeparam name="TValue"></typeparam>
  public abstract class FileKeyValueStore<TKey, TValue> : MemoryKeyValueStore<TKey, TValue>
  {
    #region Dependencies

    [Import]
    IFileSystem FileSystem { get; set; }
    [Import]
    IConverter<TKey, string> KeyToString { get; set; }
    [Import]
    IConverter<string, TKey> StringToKey { get; set; }
    #endregion

    /// <summary>
    /// the directory for storing and loading
    /// </summary>
    private string directory;

    
    /// <summary>
    /// the watcher 
    /// </summary>
    private FileSystemWatcher watcher;

    protected abstract string FileExtension { get; }

    public abstract TValue ReadFileEntry(Stream stream, TKey key);
    public abstract void WriteFileEntry(Stream stream, TKey key, TValue value);


    public string Directory
    {
      get { return directory; }
      set
      {

        if (directory == value) return;
        
        // if directory does not exist create it. if name exists but is a file throw an exception
        if (!FileSystem.Exists(value)) FileSystem.CreateDirectory(value);
        else if (!FileSystem.IsDirectory(value)) throw new InvalidOperationException("cannot use specified path as directory because it is a file");        
        
        directory = value;
        RemoveWatcher();
        StartWatcher();
        UpdateIndex();
      }
    }
    void StartWatcher() {
      watcher = new FileSystemWatcher(Directory, "*." + FileExtension);
      watcher.Renamed += FileRenamed;
      watcher.Changed += FileChanged;
      watcher.Created += FileCreated;
      watcher.Deleted += FileDeleted;
      watcher.EnableRaisingEvents = true;
    }
    void RemoveWatcher()
    {
      if (watcher != null)
      {
        watcher.Renamed -= FileRenamed;
        watcher.Changed -= FileChanged;
        watcher.Created -= FileCreated;
        watcher.Deleted -= FileDeleted;
        watcher.Dispose();
      }
    }
    public void UpdateIndex()
    {
      foreach (var item in FileSystem.GetEntries(directory))
      {
        UpdateIndex(Path.GetFileName(item));
      }
    }
    public void UpdateIndex(string file)
    {
      if (FileSystem.IsDirectory(file)) return;
      var filename = file;
      var id = FilenameToKey(filename);
      if (id.Equals(default(TKey))) return;
      RegisterEntry(id, null);

    }


    private void FileDeleted(object sender, FileSystemEventArgs e)
    {
      var id = FilenameToKey(e.Name);
      if (object.Equals(id, default(TKey))) return;
      Delete(id);
      RaiseValueChanged(id, Change.Deleted);
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
      var id = FilenameToKey(e.Name);
      if (id.Equals(default(TKey))) return;
      RegisterEntry(id, null);
      RaiseValueChanged(id, Change.Added);
    }

    private void FileChanged(object sender, FileSystemEventArgs e)
    {
      var id = FilenameToKey(e.Name);
      if (id.Equals(default(TKey))) return;
      RaiseValueChanged(id, Change.Modified);
    }

    private void FileRenamed(object sender, RenamedEventArgs e)
    {
      var oldId = FilenameToKey(e.OldName);
      var newId = FilenameToKey(e.Name);
      Delete(oldId);
      if (newId.Equals(default(TKey))) return;
      RegisterEntry(newId, null);
      RaiseValueChanged(newId, Change.Added);
    }


    public override object Pack(TKey id, TValue value)
    {
      var path = KeyToPath(id);
      using (var stream = FileSystem.Exists(path) ? FileSystem.OpenFile(path, FileAccess.Write) : FileSystem.CreateFile(path))
      {
        WriteFileEntry(stream, id, value);
        stream.Close();
      }
      return null;
    }
    public override TValue Unpack(TKey id, object value)
    {
      var path = KeyToPath(id);
      using (var stream = FileSystem.OpenFile(path, FileAccess.Read))
      {
        return ReadFileEntry(stream, id);
      }
    }

    public TKey FilenameToKey(string filename)
    {
      if (!filename.EndsWith("." + FileExtension)) return default(TKey);
      var idString = filename.Substring(0, filename.Length -("." + FileExtension).Length);
      var id = StringToKey.Convert(idString);
      return id;
    }
    public string KeyToFilename(TKey id)
    {
      var filename = KeyToString.Convert(id) + "." + FileExtension;
      return filename;
    }

    protected override void OnStore(TKey key, TValue value)
    {
      base.OnStore(key, value);
    }
    public string KeyToPath(TKey key)
    {
      var filename = KeyToFilename(key);
      var path = Directory + Path.DirectorySeparatorChar+ filename;
      return path;
    }
    protected override void OnDelete(TKey key)
    {
      var path = KeyToPath(key);
      if (!FileSystem.Exists(path)) return;
      FileSystem.Delete(path);
    }


    #region unneeded abstract methods

    // these methods will never be called
    public override object Pack(TValue value)
    {
      throw new NotImplementedException();
    }

    public override TValue Unpack(object packedObject)
    {
      throw new NotImplementedException();
    }

    #endregion

  }
}
