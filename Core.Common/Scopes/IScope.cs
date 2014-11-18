using System;
using System.Collections.Generic;

namespace Core
{
  public interface IScope : IValueMap
  {
    IScope Parent { get; }
    object this[string key] { get; set; }
  }

  public interface IScope<TParentScope> where TParentScope : IScope
  {
    new TParentScope Parent { get; }
  }


}
