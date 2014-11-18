using System;
using System.ComponentModel;

namespace Core.Values
{

  public abstract class AbstractOneWayBinding : BinaryBinding, IManualBinding, IOneWayBinding
  {
    public AbstractOneWayBinding() { }
    public AbstractOneWayBinding(ISource source, ISink sink, bool enable = true) : base(source, sink, enable) { }
    
    
    public abstract void Sync();

    protected  override void Bind()
    {
      Sync();
    }

    protected override void Unbind()
    {

    }
    public new ISink Sink
    {
      get
      {
        return (ISink)base.Sink;
      }
      set
      {
        base.Sink = value;
      }
    }

    public new ISource Source
    {
      get
      {
        return (ISource)base.Source;
      }
      set
      {
        base.Source = value;
      }
    }
  }
  public class SimpleManualOneWayBinding : AbstractOneWayBinding
  {
    public SimpleManualOneWayBinding() { }
    public SimpleManualOneWayBinding(ISource source, ISink sink, bool enable = true) : base(source, sink, enable) { }
    
    
    public override void Sync()
    {
      Sink.Value = Source.Value;
    }
    
  }
}
