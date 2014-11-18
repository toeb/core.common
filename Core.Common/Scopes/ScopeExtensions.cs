using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
  public static class ScopeExtensions
  {
    public static TScope New<TScope>(this TScope scope, Action<TScope> configure= null) 
      where TScope : AbstractScope, new()
    {
      var instance = new TScope();// System.Activator.CreateInstance<TScope>();
      instance.SetParent(scope);
      if(configure!=null) configure(instance);
      return instance;
    }
    public static TChildScope New<TChildScope, TParentScope>(this TParentScope scope, TypeArgument<TChildScope> child)
      where TChildScope : TParentScope, new()
      where TParentScope : AbstractScope
    {
      var instance = new TChildScope();
      instance.SetParent(scope);
      return instance;
    }

    public static TScope Configure<TScope>(this TScope scope, Action<TScope> configure) where TScope : IScope
    {
      configure(scope);
      return scope;
    }

    
  }
}
