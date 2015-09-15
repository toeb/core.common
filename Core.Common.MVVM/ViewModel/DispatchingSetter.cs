using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.MVVM.ViewModel
{
  public class DispatchingSetter:DelegatingPropertySetter
  {
    private IDispatcher dispatcher;
    public DispatchingSetter(PropertySetterDelegate inner, IDispatcher dispatcher) : base(inner) { this.dispatcher = dispatcher; }
    protected override bool Set(object @object, string key, Type type, object value)
    {
      bool result = false;
      dispatcher.Dispatch(() => result = Inner(@object, key, type, value));
      return result;
    }
  }
}
