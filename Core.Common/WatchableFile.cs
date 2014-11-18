using System;
using System.IO;

namespace Core
{
  public class WatchableFile : NotifyPropertyChangedBase, IDisposable
  {
    private bool eventsEnabled = false;
    public bool EventsEnabled { get { return eventsEnabled; } set { ChangeIfDifferentAndCallback(ref eventsEnabled, value, EventsEnabledChanging, EventsEnabledChanged); } }

    private void EventsEnabledChanged(bool oldValue, bool newValue)
    {
      watcher.EnableRaisingEvents = newValue;
    }

    private void EventsEnabledChanging(bool oldValue, bool newValue)
    {
    }

    public static implicit operator WatchableFile(string path)
    {
      return new WatchableFile(path);
    }
    private FileSystemWatcher watcher;
    private string path;
    public string Path { get { return path; } set { TransformAndChange(ref path, value, PathTransform, PathChanging, PathChanged, "Path"); } }
    public WatchableFile() { }
    public WatchableFile(string path) { Path = path; }
    private void PathChanged(string oldValue, string newValue)
    {
      if (newValue != null) StartWatching(newValue);
    }

    private void StartWatching(string path)
    {
      if (watcher != null) StopWatching(path);
      var directory = System.IO.Path.GetDirectoryName(path);
      var filename = System.IO.Path.GetFileName(path);
      watcher = new FileSystemWatcher(directory, filename);
      watcher.Created += Created;
      watcher.Deleted += Deleted;
      watcher.Changed += Changed;
      EventsEnabled = true;
    }


    private void Created(object sender, FileSystemEventArgs e)
    {

      if (FileCreated != null) FileCreated(this);
    }

    private void Deleted(object sender, FileSystemEventArgs e)
    {

      if (FileDeleted != null) FileDeleted(this);
    }

    private void Changed(object sender, FileSystemEventArgs e)
    {
      if (FileChanged != null) FileChanged(this);
    }

    private void PathChanging(string oldValue, string newValue)
    {
      if (oldValue != null) StopWatching(oldValue);
    }

    private void StopWatching(string path)
    {
      if (watcher == null) return;
      watcher.EnableRaisingEvents = false;
      watcher.Dispose();
      watcher = null;
    }

    private string PathTransform(string input)
    {
      input = System.IO.Path.GetFullPath(input);
      return input;
    }

    public event Action<WatchableFile> FileChanged;
    public event Action<WatchableFile> FileCreated;
    public event Action<WatchableFile> FileDeleted;

    public void Dispose()
    {
      if (watcher != null) watcher.Dispose();
    }
  }
}
