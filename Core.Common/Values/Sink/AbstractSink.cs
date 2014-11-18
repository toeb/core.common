using System;
using System.ComponentModel;

namespace Core.Values
{
  public abstract class AbstractSink : ConnectorBase, ISink
  {
    protected AbstractSink(bool isWritable, Type valueType, bool onlyExact) : this(new SinkInfo(isWritable, valueType, onlyExact)) { }
    protected AbstractSink(SinkInfo sinkInfo)
      : base(sinkInfo)
    {
      if (sinkInfo == null) sinkInfo = new SinkInfo(true, typeof(object), false);
      SinkInfo = sinkInfo;
      Info = sinkInfo;
    }

    public abstract void ConsumeValue(object value);

    public virtual bool CanConsume(object value)
    {
      return true;
    }
    public ISinkInfo SinkInfo
    {
      get;
      protected set;
    }
    protected SinkInfo Info
    {
      get;
      set;
    }
    public void Consume(object value)
    {
      if (!SinkInfo.IsWriteable) throw new InvalidOperationException("sink is currently not writable");
      if (!SinkInfo.IsValidValue(value)) throw new ArgumentException("value is not valid - see SinkInfo");
      if (!CanConsume(value)) throw new ArgumentException("value is not consumable atm");

      ConsumeValue(value);
      RaiseValueConsumed(value);

    }


    private void RaiseValueConsumed(object value)
    {
      if (ValueConsumed == null) return;
      ValueConsumed(this, new ValueConsumedEventArgs(value));
    }




    public virtual object Value
    {
      set { Consume(value); }
    }


    public event ValueConsumedDelegate ValueConsumed;



    public virtual void Push(ISource source, Merge.IMergeStrategy strategy)
    {
      if (!strategy.CanMerge(source, this)) throw new InvalidOperationException();
      strategy.Merge(source, this);
    }


  }
}
