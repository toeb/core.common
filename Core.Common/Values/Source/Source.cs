using System;
using System.ComponentModel;

namespace Core.Values
{
  public delegate void NotifyValueChangedDelegate();
  public static class Source
  {
    public static ISource<T> Delegate<T>(ProduceValueDelegate<T> producer, out NotifyValueChangedDelegate notifier)
    {
      var result = new DelegateSource<T>(producer, null, typeof(T), false);
      notifier = () => result.NotifyValueChanged();
      return result;
    }
    
    public static ISource<T> Delegate<T>(ProduceValueDelegate<T> producer)
    {
      var result = new DelegateSource<T>(producer, null, typeof(T), false);
    
      return result;
    }
    public static ISource<T> Const<T>(T value,  bool noReference = false)
    {
      if (noReference)
      {
        value = value.DeepClone();

        return Delegate(() => value.DeepClone());
      }
      else
      {
        return Delegate(() => value);
      }
    }
   
  }
}
