using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace Core.Resources
{

  public class FileResourceHandler : IResourceHandler
  {
    public  IEnumerable<IManagedResource> Parse(string resourceName, string resourceKey, IProjectInfo info, object input, Assembly assembly)
    {
      if (string.IsNullOrEmpty(resourceName)) yield break;
      if(string.IsNullOrEmpty(resourceKey))yield break;
      var stream = input as Stream;
      if(stream == null)yield break;

      resourceName = resourceName+resourceKey.Replace('/','.').Replace('\\','.');
      var projectDir = info.ProjectDir;
      var result = new FileResource(resourceName, stream, info.ProjectDir, resourceKey, assembly);

      yield return result;

    }

    public int Priority
    {
      get { return 3; }
    }
  }
}
