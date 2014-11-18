using System;
using System.ComponentModel;

namespace Core.Values
{
  public static class Binding
  {
    public static IBinding TwoWay(IValue source, IValue sink,bool enable = false)
    {
      return new TwoWayBinding(source, sink, enable);
    }
    public static IManualBinding OneWayManual(ISource source, ISink sink)
    {
      return new SimpleManualOneWayBinding(source, sink, false);
    }
    public static IBinding OneWay(ISource source, ISink sink,bool enable = false)
    {
      return new AutoOneWayBinding(source, sink, enable);
    }

  }
}
