using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{

  public class MemoryManagedProperty<T> : AbstractManagedProperty<T>
  {
    public MemoryManagedProperty(string name, T initialValue) : base(name, true, true) { value = initialValue; }
    T value;
    protected override void Consume(T value)
    {
      this.value = value;
    }
    protected override T Produce()
    {
      return value;

    }

    public new T Value { get { return (T)base.Value; } set { base.Value = value; } }
  }
}
