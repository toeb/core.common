using System;
using System.ComponentModel;

namespace Core.Values
{
  public static class Sink
  {

    public static ISink<T> Delegate<T>(ConsumeValueDelegate<T> consumer)
    {
      return new DelegateSink<T>(consumer, null, typeof(T), false);
    }
  }
}
