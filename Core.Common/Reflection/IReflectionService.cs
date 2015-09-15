using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace Core.Common.Reflect
{
  public interface IReflectionService
  {
    IEnumerable<Assembly> Assemblies { get; }
    IEnumerable<Type> Types { get; }
  
  }
}
