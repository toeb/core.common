using System;
using System.ComponentModel;
using Core.Common.Reflect;
namespace Core.Values
{
  /// <summary>
  /// abstract base class for values values only need to pass the correct valueinfo and implement ConsumeValue and ProduceValue methods

  /// </summary>
  public abstract class AbstractValue : ConnectorBase, IValue, INotifyPropertyChanged
  {

    /// <summary>
    /// Subclasses must override 
    /// gets called when value is to be consumed
    /// normally the subclass would somehow store the value
    /// </summary>
    /// <param name="value"></param>
    protected abstract void ConsumeValue(object value);
    /// <summary>
    /// gets called when the value needs to be produced
    /// subclasses must retrieve the value from storage
    /// </summary>
    /// <returns></returns>
    protected abstract object ProduceValue();

    /// <summary>
    /// default implementation calls produce and consume
    /// subclasses might want to cache the results
    /// </summary>
    public virtual object Value
    {
      get
      {
        return Produce();
      }
      set
      {
        Consume(value);
      }
    }

    #region Notification methods

    protected void NotifyValueChanged(object oldValue, object newValue)
    {
      NotifyValueChanged(new ValueChangeEventArgs(ChangeState.Changed, oldValue, newValue));
    }
    protected void NotifyValueChanged(object newValue)
    {
      NotifyValueChanged(new ValueChangeEventArgs(ChangeState.Changed, newValue));
    }
    protected void NotifyValueChanged()
    {
      NotifyValueChanged(new ValueChangeEventArgs(ChangeState.Changed));
    }
    protected void NotifyValueChanged(ValueChangeEventArgs args)
    {
      if (ValueChanged != null) ValueChanged(this, args);
      OnValueChanged();
      RaisePropertyChanged("Value");
    }
    /// <summary>
    /// extension point valled whenever the value is changed
    /// </summary>
    protected virtual void OnValueChanged(){}
    protected virtual void OnBeforeValueConsumed(object newValue) { }
    protected virtual void OnValueConsumed(object value) { }
    protected virtual void OnValueProduced(object value) { }
    #endregion

    #region Constructors

    protected AbstractValue(bool isReadable, bool isWritable, Type valueType, bool onlyExact) : this(new ValueInfo(isReadable, isWritable, valueType, onlyExact)) { }

    protected AbstractValue(ValueInfo info)
      : base(info)
    {
      Info = info;
      Info.Subscribe(ValueTypeName, OnValueTypeChanged);
      Info.Subscribe(OnlyExactTypeName, OnOnlyExactTypeChanged);
    }
    #endregion

    #region ValueInfo
    /// <summary>
    /// convenience accessor for ValueInfo
    /// </summary>
    protected ValueInfo Info
    {
      get;
      private set;
    }


    public ISourceInfo SourceInfo { get { return Info; } }
    public IValueInfo ValueInfo { get { return Info; } }
    public ISinkInfo SinkInfo { get { return Info; } }


    private void OnOnlyExactTypeChanged(object sender, ValueChangeEventArgs args)
    {
      if (!ConnectorInfo.OnlyExactType) return;
      if (ConnectorInfo.ValueType.IsExactlyAssignableFromValue(Value)) return;
      ResetValue();
    }

    private void OnValueTypeChanged(object sender, ValueChangeEventArgs args)
    {
      if (!SourceInfo.IsReadable) return;
      if (ConnectorInfo.ValueType.IsAssignableFromValue(this.Value)) return;
      ResetValue();
    }

    private void ResetValue()
    {
      if (ProduceValue() == null) return;
      ConsumeValue(null);
      RaisePropertyChanged("Value");
    }

    #endregion

    private void RaiseValueProduced(object value)
    {
      OnValueProduced(value);
      if (ValueProduced != null) ValueProduced(this, value);
    }
    private void RaiseValueConsumed(object value)
    {
      OnValueConsumed(value);
      if (ValueConsumed != null) ValueConsumed(this, value);
    }
    
    #region Produce and Consume
    public object Produce()
    {
      if (!SourceInfo.IsReadable) throw new InvalidOperationException("value is not readable");
      var value =  ProduceValue();
      RaiseValueProduced(value);
      return value;
    }
    private bool EqualityCheck(object lhs, object rhs)
    {
      if (object.ReferenceEquals(lhs, rhs)) return true;
      if (lhs != null) return lhs.Equals(rhs);
      if (rhs != null) return rhs.Equals(lhs);

      return true;
    }

    public void Consume(object value)
    {
      if (!SinkInfo.IsWriteable) throw new InvalidOperationException("Value is readonly, Calling set will not work");
      if (!ConnectorInfo.IsValidValue(value)) throw new ArgumentException("Invalid value", "value");
      if (SourceInfo.IsReadable)
      {
        var oldValue = ProduceValue();
        if (EqualityCheck(oldValue, value)) return;
        OnBeforeValueConsumed(value);
        ConsumeValue(value);
        RaiseValueConsumed(value);
      }
      else
      {
        ConsumeValue(value);
        RaiseValueConsumed(value);
      }
    }

    public bool CanProduce()
    {
      return SourceInfo.IsReadable;
    }
    public virtual bool CanConsume(object value)
    {
      return IsValidValue(value);
    }


    #endregion

    #region events
    public event ValueChangeDelegate ValueChanged;
    public event ValueProducedDelegate ValueProduced;
    public event ValueConsumedDelegate ValueConsumed;


    #endregion

    #region pull and push
    public virtual void Pull(ISink sink, Merge.IMergeStrategy strategy)
    {
      if (!strategy.CanMerge(this, sink)) throw new InvalidOperationException("could not push because merge strategy is not appplicable");
      strategy.Merge(this, sink);
    }


    public virtual void Push(ISource source, Merge.IMergeStrategy strategy)
    {
      if (!strategy.CanMerge(source, this))throw new InvalidOperationException("could not push because merge strategy is not appplicable");
      strategy.Merge(source, this);
    }
    #endregion

    #region fields
    private static readonly string ValueTypeName = "ValueType";
    private static readonly string OnlyExactTypeName = "OnlyExactType";
    #endregion
  }
}
