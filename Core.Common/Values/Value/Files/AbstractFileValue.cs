
namespace Core.Values
{
  public abstract class AbstractFileValue : CompositeValue, IFileValue
  {
    AbstractFileSource source;
    AbstractFileSink sink;

    public string Path { set { sink.Path = value;/*path is also set in soruce over events*/ } get { return sink.Path; } }


    protected AbstractFileValue(AbstractFileSource source, AbstractFileSink sink, ValueInfo info)
      : base(
        source, sink, info
        )
    {
      source.Subscribe("Path", SourcePathChanged);
      sink.Subscribe("Path", SinkPathChanged);
    }

    private void SinkPathChanged(object sender, ValueChangeEventArgs args)
    {
      source.Path = sink.Path;
      RaisePropertyChanged("Path");
    }

    private void SourcePathChanged(object sender, ValueChangeEventArgs args)
    {
      sink.Path = source.Path;
      RaisePropertyChanged("Path");
    }
    private void FileValueChanged(object sender, ValueChangeEventArgs args)
    {
      NotifyValueChanged();
    }
  }
}
