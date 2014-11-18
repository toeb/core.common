using System.Collections.Generic;
using System.Reflection;

namespace Core.Resources
{
  public interface IResourceHandler
  {
    int Priority { get; }

    IEnumerable<IManagedResource> Parse(
      string resourceName,
      string resourceKey,
      IProjectInfo info,
      object input,
      Assembly assembly

    );
  }
}
