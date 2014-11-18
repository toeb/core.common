using System;
using System.Collections.Generic;

namespace Core
{
  public interface IPropertyStore : ITypedValueMap
  {
    object this[string key] { get; set; }
  }
}
