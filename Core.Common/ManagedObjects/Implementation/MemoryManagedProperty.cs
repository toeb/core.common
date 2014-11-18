using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{

  public class MemoryManagedProperty : AbstractManagedProperty
  {
    private object value;
    public MemoryManagedProperty(ManagedPropertyInfo info, object initialValue)
      : base(info)
    {
      value = initialValue;
    }
    public MemoryManagedProperty(string name, bool isReadable, bool isWritable, Type type, object initialValue)
      : base(name, isReadable, isWritable, type)
    {
      value = initialValue;
    }
    protected override void ConsumeValue(object value)
    {
      this.value = value;
    }
    protected override object ProduceValue()
    {
      return value;
    }
  }
}
