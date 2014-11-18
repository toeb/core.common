using System;

namespace Core.Values
{
  /// <summary>
  /// represents a value which is a composite of a (not necessarily different) sink and source
  /// </summary>
  public class CompositeValue :  AbstractValue
  {
    public ISink Sink { get; private set; }
    public ISource Source { get; private set; }

    protected CompositeValue(ISource source, ISink sink, ValueInfo info)
      :base(info)
    {
      if (sink.SinkInfo.ValueType != source.SourceInfo.ValueType) throw new ArgumentException("currently sink and source must have the same datatype to be combined into one value");
      Sink = sink;
      Source = source;
      source.ValueChanged += SourceValueChanged;
    }

    private void SourceValueChanged(object sender, ValueChangeEventArgs args)
    {
      NotifyValueChanged(this,args);
    }
    public CompositeValue(ISink sink, ISource source) :
      this(source,sink,new ValueInfo(
        source == null? false:source.SourceInfo.IsReadable,
        sink==null?false:sink.SinkInfo.IsWriteable,
        sink.ConnectorInfo.ValueType,
        sink.ConnectorInfo.OnlyExactType
        )
      )
    {

    }


    protected override void ConsumeValue(object value)
    {
      Sink.Consume(value);
    }

    protected override object ProduceValue()
    {
      return Source.Produce();
    }
  }
}
