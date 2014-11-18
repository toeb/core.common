using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;

namespace Core.Resources
{
  public class StringResourceHandler : IResourceHandler
  {


    public int Priority
    {
      get { return 3; }
    }

    public IEnumerable<IManagedResource> Parse(string resourceName, string resourceKey, IProjectInfo info, object input, Assembly assembly)
    {
      var value = input as string;
      if (value == null) yield break;

      resourceName = resourceName+resourceKey.Replace('/','.').Replace('\\','.');
      yield return new StringResource(resourceName, value, assembly);
    }
  }
}
