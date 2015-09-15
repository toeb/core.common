using Core.Values;
using System;
using System.IO;
using System.Threading;
using Core;
using Core.Trying;
namespace Core.Values
{


  public abstract class FileObjectValue : AbstractValue, IDisposable
  {
    private string path;
    private const string PathName = "Path";
    private FileSystemWatcher watcher;
    protected FileObjectValue(ValueInfo info) : base(info) { }

    protected abstract object Deserialize(Stream stream);
    protected abstract void Serialize(Stream stream, object data);

    /// <summary>
    /// the path of the file object
    /// </summary>
    public string Path
    {
      get { return path; }
      set { TransformAndChange(ref path, value, TransformPath, PathChanging, PathChanged, PathName); }
    }

    ~FileObjectValue()
    {
      if (watcher != null) RemoveWatch(path);
    }

    private string TransformPath(string input)
    {
      if (string.IsNullOrEmpty(input)) return null;
      input = System.IO.Path.GetFullPath(input);
      return input;
    }

    private void PathChanging(string oldValue, string newValue) { }

    private void PathChanged(string oldValue, string newValue)
    {
      if (oldValue != null)
      {
        RemoveWatch(oldValue);
      }
      if (newValue != null)
      {
        SetupWatch(newValue);
      }
      else
      {

      }
    }

    private void SetupWatch(string path)
    {
      var file = System.IO.Path.GetFileName(path);
      var dir = System.IO.Path.GetDirectoryName(path);
      watcher = new FileSystemWatcher(dir, file);
      watcher.Deleted += FileDeleted;
      watcher.Created += FileCreated;
      watcher.Changed += FileChanged;
      watcher.EnableRaisingEvents = true;
    }

    private void FileChanged(object sender, FileSystemEventArgs e)
    {
      NotifyValueChanged();
    }

    private void FileCreated(object sender, FileSystemEventArgs e)
    {
      //NotifyValueChanged();
    }

    private void FileDeleted(object sender, FileSystemEventArgs e)
    {
      NotifyValueChanged();
    }

    

    private string Read(string path)
    {
      string content = "";
      Action action = () => content = File.ReadAllText(path);
      action.TryRepeatException();
      return content;
    }

    private void Write(object value)
    {
      if (Path == null) return;
      Action action = () =>
      {
        using (var stream = File.Exists(Path) ? File.OpenWrite(Path) : File.Create(Path))
        {
          stream.SetLength(0);
          Serialize(stream, value);
          stream.Close();
        }
      };
      action.TryRepeatException();
    }
    
    private object Read()
    {
      if (Path == null) return null;
      object value = null;
      if (File.Exists(Path))
      {
        Action action = () =>
        {
          if (path == null)
          {
            value = null;
            return;
          }

          using (var stream = File.OpenRead(Path))
          {
            value = Deserialize(stream);
            stream.Close();
          }
        };
        action.TryRepeatException();
      }
      else
      {
        value = null;
      }
      return value;
    }

    private void RemoveWatch(string path)
    {
      if (watcher == null) return;
      watcher.EnableRaisingEvents = false;
      watcher.Dispose();
      watcher = null;
    }

    
    object value;



    protected override object ProduceValue()
    {
      return Read();
    }

    protected override void ConsumeValue(object value)
    {
      Write(value);
    }

    public void Dispose()
    {
      if(watcher==null)return;
      watcher.EnableRaisingEvents = false;
      watcher.Dispose();
    }
  }
}
