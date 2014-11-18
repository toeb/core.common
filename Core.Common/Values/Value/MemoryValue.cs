using System;
using System.ComponentModel;

namespace Core.Values
{
  public class MemoryValue : AbstractValue
  {
    public MemoryValue( bool isReadable, bool isWritable, Type valueType, bool exactType) : base(isReadable, isWritable, valueType, exactType) { }
    private object value;


    protected override object ProduceValue()
    {
      return value;
    }
    protected override void ConsumeValue(object value)
    {
      this.value = value;
    }
  }

  public class MemoryValue<T> : MemoryValue, IValue<T>
  {
    public MemoryValue(bool isReadable, bool isWritable, Type valueType, bool exactType)
      : base( isReadable, isWritable, valueType, exactType)
    {
      if (!typeof(T).IsAssignableFrom(valueType)) throw new ArgumentException("valueType must be assignable to typeof(T)");
    }

    public override bool IsValidValue(object value)
    {
      if (value != null && !(value is T)) return false;
      return base.IsValidValue(value);
    }

    public new T Value
    {
      get
      {
        var val = base.Value;
        if (val == null) return default(T);
        return (T)base.Value;
      }
      set
      {
        base.Value = value;
      }
    }
  }
}
