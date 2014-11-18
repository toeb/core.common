using System;
using System.IO;

namespace Core.Values
{
  public abstract class AbstractFileSink : AbstractSink
  {
    private string path;
    public string Path { get { return path; } set { TransformAndChange(ref path, value, PathTransform, PathChanging, PathChanged, "Path"); } }

    private void PathChanged(string oldValue, string newValue)
    {

    }

    private void PathChanging(string oldValue, string newValue)
    {

    }

    private string PathTransform(string input)
    {

      return System.IO.Path.GetFullPath(input);
    }
    public int MaxRepeats { get; set; }
    protected AbstractFileSink(string path, SinkInfo info) : base(info) { Path = path; MaxRepeats = 10; }

    protected abstract void WriteFile(Stream stream, object value);
    public override void ConsumeValue(object value)
    {
      Stream stream = null;
      Action action;
      if (!File.Exists(Path))
      {
        action = () => stream = File.Create(Path);
        action.TryRepeatException(MaxRepeats);
      }
      if (stream == null)
      {
        action = () => stream = File.OpenWrite(Path);
        action.TryRepeatException(MaxRepeats);

      }
      WriteFile(stream, value);
      stream.Flush();
      stream.Close();
      stream.Dispose();
      OnFileWasWritten();
    }

    protected virtual void OnFileWasWritten() { }
  }
}
