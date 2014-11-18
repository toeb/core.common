using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Resources
{
  public interface IAssemblyResources : IIdentifiable<Guid>
  {

    Assembly Assembly { get; }
    IEnumerable<IManagedResource> Resources { get; }
  }
}
