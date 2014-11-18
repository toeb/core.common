using System;
using System.ComponentModel;

namespace Core.Values
{
  using ProduceValueDelegate = ProduceValueDelegate<object>;
  public class DelegateSource : AbstractSource
  {
    private ProduceValueDelegate producer;
    private CanProduceDelegate canProduce;
    public DelegateSource(ProduceValueDelegate producer, CanProduceDelegate canProduce, Type valueType, bool onlyExact)
      : base(producer != null, valueType, onlyExact)
    {
      Producer = producer;
      CanProduceDelegate = canProduce;
    }
    protected override object ProduceValue()
    {
      if (Producer == null) throw new InvalidOperationException("cannot produce value because no producer delegate is set");
      return Producer();
    }
    public override bool CanProduce()
    {
      if (CanProduceDelegate == null) return true;
      return CanProduceDelegate();
    }
    public ProduceValueDelegate Producer { get { return producer; } set { ChangeIfDifferentAndCallback(ref producer, value, ProducerChanging, ProducerChanged); } }

    public CanProduceDelegate CanProduceDelegate { get { return canProduce; } set { ChangeIfDifferentAndCallback(ref canProduce, value, CanProduceDelegateChanging, CanProduceDelegateChanged); } }

    private void ProducerChanged(ProduceValueDelegate oldValue, ProduceValueDelegate newValue)
    {
      if (newValue != null) Info.IsReadable = true;
    }

    private void ProducerChanging(ProduceValueDelegate oldValue, ProduceValueDelegate newValue)
    {
      if (newValue == null) Info.IsReadable = false;
    }
    private void CanProduceDelegateChanged(CanProduceDelegate oldValue, CanProduceDelegate newValue)
    { }

    private void CanProduceDelegateChanging(CanProduceDelegate oldValue, CanProduceDelegate newValue)
    {
    }


  }
  public class DelegateSource<T> : DelegateSource, ISource<T>
  {
    private ProduceValueDelegate<T> producer;
    public DelegateSource(ProduceValueDelegate<T> producer, CanProduceDelegate canProduce, Type valueType, bool onlyExact)
      : base(null, null, valueType, onlyExact)
    {
      Producer = producer;
      CanProduceDelegate = canProduce;
    }

    public new ProduceValueDelegate<T> Producer
    {
      get { return producer; }
      set
      {
        if (value == producer) return;
        if (value == null) base.Producer = null;
        else base.Producer = TypedProduce;
        producer = value;
      }
    }

    private object TypedProduce()
    {
      return Producer();
    }


    public new T Value
    {
      get { return (T)base.Value; }
    }


  }
}
