using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{

  public class DelegateManagedProperty : AbstractManagedProperty
  {
    public DelegateManagedProperty(string name, ProduceValueDelegate<object> getter, ConsumeValueDelegate<object> setter, Type valueType):
      base(name, getter != null, setter != null, valueType ?? typeof(object))
    {
      Getter = getter;
      Setter = setter;
    }
    protected override void ConsumeValue(object value)
    {
      Setter(value);
    }

    protected override object ProduceValue()
    {
      return Getter();
    }

    private ConsumeValueDelegate<object> Setter { get; set; }
    private ProduceValueDelegate<object> Getter { get; set; }
  }

  public class DelegateManagedProperty<T> : AbstractManagedProperty<T>
  {
    public DelegateManagedProperty(string name, ProduceValueDelegate<T> getter, ConsumeValueDelegate<T> setter)
      : base(name, getter != null, setter != null)
    {
      if (getter == null && setter == null) throw new ArgumentException("getter and setter mey not both be null");
      this.Getter = getter;
      this.Setter = setter;
    }
    protected override void Consume(T value)
    {
      Setter(value);
    }
    protected override T Produce()
    {
      return Getter();
    }

    private ConsumeValueDelegate<T> Setter { get; set; }
    private ProduceValueDelegate<T> Getter { get; set; }
  }
}
