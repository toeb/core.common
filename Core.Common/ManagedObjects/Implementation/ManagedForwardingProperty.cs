using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{

  public class ManagedForwardingProperty : AbstractManagedProperty
  {
    public ISource Source { get; set; }
    public ISink Sink { get; set; }
    private ManagedForwardingProperty(ISource source, ISink sink, ManagedPropertyInfo info)
      : base(info)
    {
      //check if compatible source and sink
      if (sink.ConnectorInfo.ValueType != source.ConnectorInfo.ValueType) throw new ArgumentException();
      Source = source;
      Sink = sink;
      Source.ValueChanged += UnderlyingValueChanged;
      
    }
    ~ManagedForwardingProperty()
    {
      Source.ValueChanged -= UnderlyingValueChanged;
    }


    private void UnderlyingValueChanged(object sender, ValueChangeEventArgs args)
    {
      NotifyValueChanged(args);
    }
    
    public ManagedForwardingProperty(string name, ISource source, ISink sink)
      : this(source,sink,new ManagedPropertyInfo(
        name,
        source!=null?source.SourceInfo.IsReadable:false,
        sink!=null?sink.SinkInfo.IsWriteable:false,
        sink.SinkInfo.ValueType,
        false
        ))
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
