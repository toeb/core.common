using System;
using System.ComponentModel;

namespace Core.Values
{
  public static class Value
  {

    public static IValue ToValue<T>(this T initialValue)
    {
      return Value.Memory(initialValue);
    }
    public static IValue<T> Memory<T>(T initialValue = default(T), bool isReadable = true, bool isWritable= true, Type valueType = null, bool onlyExact = false)
    {
      valueType = valueType ?? typeof(T);
      if (!typeof(T).IsAssignableFrom(valueType)) throw new ArgumentException("value type must be assignable to typeof(T)");
      var result=  new MemoryValue<T>( isReadable, isWritable,valueType, onlyExact);
      result.Value = initialValue;
      return result;
    }
    public static IValue<T> Source<T>(T value)
    {
      return  Delegate<T>(() => value);
    }
    public static IValue<T> Sink<T>(SetterDelegate<T> setter)
    {
      return Delegate(setter);
    }
    public static IValue<T> ConstSource<T>(T value)
    {
      var clone = value.DeepClone();
      return Delegate<T>(() => clone.DeepClone());
    }

    public static IValue<T> Delegate<T>(GetterDelegate<T> getter, SetterDelegate<T> setter=null)
    {
      return new TypedDelegateValue<T>(getter, setter);
    }
    public static IValue<T> Delegate<T>(SetterDelegate<T> setter)
    {
      return Delegate(null, setter);
    }

  }
}
