using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{

  public abstract class AbstractManagedProperty<T> : AbstractManagedProperty, IManagedProperty<T>
  {
    protected AbstractManagedProperty(string name, bool isReadable, bool isWritable)
      : base(name, isReadable, isWritable, typeof(T))
    {

    }
    protected abstract void Consume(T value);
    protected abstract T Produce();

    protected sealed override void ConsumeValue(object value)
    {
      if (!(value is T)) throw new ArgumentException("AbstractManagedObject<T>.ConsumeValue expects a value to be castable to T");
      Consume((T)value);
    }
    protected sealed override object ProduceValue()
    {
      return Produce();
    }

    public new T Value
    {
      get
      {
        return (T)base.Value;
      }
      set
      {
        base.Value = value;
      }
    }
  }
}
