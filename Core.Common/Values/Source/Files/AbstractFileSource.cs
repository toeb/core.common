using System;
using System.IO;

namespace Core.Values
{
  public abstract class AbstractFileSource : AbstractSource, IDisposable
  {
    public string Path { get { return watchableFile.Path; } set { watchableFile.Path = value; } }
    WatchableFile watchableFile;
    protected AbstractFileSource(WatchableFile watchableFile, SourceInfo info)
      : base(info)
    {
      if (watchableFile == null) throw new ArgumentNullException("watchableFile");
      this.watchableFile = watchableFile;
      watchableFile.FileChanged += FileChanged;
      watchableFile.FileCreated += FileCreated;
      watchableFile.FileDeleted += FileDeleted;
    }

    private void FileDeleted(WatchableFile obj)
    {
      RaiseValueChanged();
    }

    private void FileCreated(WatchableFile obj)
    {
      RaiseValueChanged();
    }

    private void FileChanged(WatchableFile obj)
    {
      RaiseValueChanged();
    }

    public bool WatchingEnabled { get { return watchableFile.EventsEnabled; } set { watchableFile.EventsEnabled = value; } }
    protected abstract object ReadFile(Stream stream);
    protected virtual object FileMissingValue { get { return null; } }
    protected override object ProduceValue()
    {
      if (!File.Exists(Path)) return FileMissingValue;
      Stream stream = null;
      Action openfile = () => stream = File.OpenRead(Path);
      openfile.TryRepeatException();
      var value = ReadFile(stream);
      stream.Close();
      stream.Dispose();
      OnFileWasRead();
      return value;
    }

    protected virtual void OnFileWasRead() { }

    public void Dispose()
    {
      watchableFile.Dispose();
    }
  }
}
