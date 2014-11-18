
namespace Core.Values
{

  public class DelegateFileValue<T> : AbstractFileValue, IFileValue<T>
  {
    public DelegateFileValue(WatchableFile file, FromStreamDelegate<T> read, ToStreamDelegate<T> write, ValueInfo info)
      : base(
        new DelegateFileSource<T>(read, file, new SourceInfo(true, typeof(T), false)),
        new DelegateFileSink<T>(file.Path, write, new SinkInfo(true, typeof(T), false)),
        info
        )
    {

    }
    public DelegateFileValue(string path, FromStreamDelegate<T> read, ToStreamDelegate<T> write)
      : this(path, read, write, new ValueInfo(true, true, typeof(T), false))
    {

    }

    public new T Value
    {
      get
      {
        return (T)base.Value;
      }
      set
      {
        base.Value = value;
      }
    }


  }
}
