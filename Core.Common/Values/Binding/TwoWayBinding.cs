using System;
using System.ComponentModel;

namespace Core.Values
{

  public interface ITwoWayBinding<TA, TB>
    where TA:ISink,ISource
    where TB: ISink,ISource
  {
    TB Source { get; set; }
    TA Sink { get; set; }
  }
  class TwoWayBinding : BinaryBinding, ITwoWayBinding<IValue,IValue>
  {
    public TwoWayBinding(IValue source, IValue sink, bool enable = true) : base(source, sink, enable) { }

    AutoOneWayBinding sourceToSink = new AutoOneWayBinding();
    AutoOneWayBinding sinkToSource = new AutoOneWayBinding();

    protected override void Bind()
    {
      sourceToSink.Source = Source;
      sourceToSink.Sink = Sink;
      sourceToSink.Enable();
      sinkToSource.Source = Sink;
      sinkToSource.Sink = Source;
      sinkToSource.Enable();
    }
    protected override void Unbind()
    {
      sourceToSink.Disable();
      sourceToSink.Source = null;
      sourceToSink.Sink = null;
      sinkToSource.Disable();
      sinkToSource.Source = null;
      sinkToSource.Sink = null;

    }


    public new IValue Source
    {
      get
      {
        return (IValue)base.Source;
      }
      set
      {
        base.Source = (IValue)value;
      }
    }

    public new IValue Sink
    {
      get
      {
        return (IValue)base.Sink;
      }
      set
      {
        base.Sink = (IValue)value;
      }
    }
  }
}
