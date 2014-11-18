using System;
using System.ComponentModel;

namespace Core.Values
{
  public abstract class AbstractSource : ConnectorBase, ISource
  {
    protected abstract object ProduceValue();

    protected AbstractSource(SourceInfo info):base(info)
    {
      if (info == null) info = new SourceInfo(true, typeof(object), false);
      Info = info;
      SourceInfo = info;
    }

    protected AbstractSource(bool isReadable, Type valueType, bool onlyExact)
      :this(new SourceInfo(isReadable,valueType,onlyExact))
    {
    }
    protected SourceInfo Info
    {
      get;
      set;
    }
    public ISourceInfo SourceInfo
    {
      get;
      protected set;
    }

    public object Produce()
    {
      if (!SourceInfo.IsReadable) throw new InvalidOperationException("Source is currently unreadable");
      if (!CanProduce()) throw new InvalidOperationException("Source currently cannot produce a value");

      State = SourceState.Producing;
      var product  = ProduceValue();
      if (CanProduce()) State = SourceState.Ready;
      else State = SourceState.Dry;
      RaiseValueProduced(product);
      return product;
    }

    protected void NotifyCanProduce()
    {
      if (CanProduce()) State = SourceState.Ready;
    }
    internal void NotifyValueChanged()
    {
      RaiseValueChanged();
    }
    protected void RaiseValueChanged()
    {
      RaisePropertyChanged("Value");
      if (ValueChanged != null) ValueChanged(this, new ValueChangeEventArgs(ChangeState.Changed));

    }
    protected void RaiseValueChanged(object newValue)
    {
      RaisePropertyChanged("Value");
      if (ValueChanged != null) ValueChanged(this, new ValueChangeEventArgs(ChangeState.Changed,newValue));

    }
    protected void RaiseValueChanged(object oldvalue, object newvalue) {
      RaisePropertyChanged("Value");
      if (ValueChanged != null) ValueChanged(this, new ValueChangeEventArgs(ChangeState.Changed,oldvalue, newvalue));
    }
    protected void RaiseValueProduced(object producedValue) {
      if (ValueProduced != null) ValueProduced(this, new ValueProducedEventArgs(producedValue));
    }
    protected void RaiseValueProduced()
    {
      if (ValueProduced != null) ValueProduced(this, new ValueProducedEventArgs());
    }

    public object Value
    {
      get { return Produce(); }
    }


    public event ValueChangeDelegate ValueChanged;
    public event ValueProducedDelegate ValueProduced;
    


    public virtual bool CanProduce()
    {
      return true;
    }




    private SourceState state = SourceState.Dry;
    public SourceState State
    {
      get { return state; }
      private set {
        ChangeIfDifferentAndCallback(ref state, value, StateChanging, StateChanged);
      }
    }

    private void StateChanging(SourceState oldValue, SourceState newValue)
    {
      OnBeforeStateChange(oldValue, newValue);
    }

    protected virtual void OnBeforeStateChange(SourceState oldValue, SourceState newValue) { }
    

    private void StateChanged(SourceState oldValue, SourceState newValue)
    {
      OnSourceStateChanged(newValue);
      RaisePropertyChanged("State");
    }

    protected virtual void OnSourceStateChanged(SourceState newValue) { }


    public virtual void Pull(ISink sink, Merge.IMergeStrategy strategy)
    {
      if (!strategy.CanMerge(this, sink))  throw new InvalidOperationException();
      strategy.Merge(this, sink);
    }


  }
}
