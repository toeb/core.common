using System;
using System.ComponentModel;

namespace Core.Values
{
  using ConsumeValueDelegate = ConsumeValueDelegate<object>;
  using CanConsumeDelegate = CanConsumeValueDelegate<object>;
  public class DelegateSink : AbstractSink
  {
    private ConsumeValueDelegate consumer;
    private CanConsumeDelegate canConsume;
    public DelegateSink(ConsumeValueDelegate consume, CanConsumeDelegate canConsume, Type valueType, bool onlyExact)
      : base(consume != null, valueType, onlyExact)
    {
      Consumer = consume;
      CanConsumeDelegate = canConsume;
    }
    public override void ConsumeValue(object value)
    {
      if (Consumer == null) throw new InvalidOperationException("No consumer delegate exists");
      Consumer(value);
    }
    public override bool CanConsume(object value)
    {
      if (CanConsumeDelegate == null) return base.CanConsume(value);
      return CanConsumeDelegate(value);
    }

    public CanConsumeDelegate CanConsumeDelegate { set { ChangeIfDifferentAndCallback(ref canConsume, value, CanConsumeCallbackChanging, CanConsumeCallbackChanged); } get { return canConsume; } }

    
    private void CanConsumeCallbackChanged(CanConsumeDelegate oldValue, CanConsumeDelegate newValue)
    {
    }

    private void CanConsumeCallbackChanging(CanConsumeDelegate oldValue, CanConsumeDelegate newValue)
    {
    }


    public ConsumeValueDelegate Consumer { get { return consumer; } set { ChangeIfDifferentAndCallback(ref consumer, value, ConsumerChanging, ConsumerChanged); } }

    private void ConsumerChanged(ConsumeValueDelegate oldValue, ConsumeValueDelegate newValue)
    {
      if (newValue != null) Info.IsWriteable = true;
    }

    private void ConsumerChanging(ConsumeValueDelegate oldValue, ConsumeValueDelegate newValue)
    {
      if (newValue == null) Info. IsWriteable = false;
    }
  }

  public class DelegateSink<T> : DelegateSink, ISink<T>
  {
    public DelegateSink(ConsumeValueDelegate<T> consumer, CanConsumeValueDelegate<T> canConsume, Type valueType, bool onlyExact)
      : base(null, null, valueType ?? typeof(T), onlyExact)
    {
      if (!typeof(T).IsAssignableFrom(valueType)) throw new ArgumentException("valueType must be assignable to typeof(T)");
      Consumer = consumer;
      CanConsumeDelegate = canConsume;
    }
    private ConsumeValueDelegate<T> consumer;
    private CanConsumeValueDelegate<T> canConsume;
    ConsumeValueDelegate<T> Consumer
    {
      get { return consumer; }
      set
      {
        if (value == consumer) return;
        if (value == null) base.Consumer = null;
        else base.Consumer = ConsumeWrapper;
        consumer = value;
      }
    }

    private void ConsumeWrapper(object value)
    {
      if (!(value is T)) throw new InvalidCastException("the value passed is not of type T");
      Consumer((T)value);

    }
    CanConsumeValueDelegate<T> CanConsumeDelegate
    {
      get { return canConsume; }

      set
      {
        if (value == canConsume) return;
        if (value == null) base.CanConsumeDelegate = null;
        else base.CanConsumeDelegate = CanConsumerWrapper;
        this.canConsume = value;
      }
    }

    private bool CanConsumerWrapper(object value)
    {
      if (!(value is T)) throw new InvalidCastException("the value passed is not of type T");
      return CanConsumeDelegate((T)value);
    }



    public new T Value
    {
      set { base.Value = value ; }
    }


  }
}
