using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
namespace Core.Resources
{

  public class EmbeddedResourceHandler : IResourceHandler
  {
    static EmbeddedResourceHandler()
    {
      ignoredExtensions = new HashSet<string>(new[] { ".min.js", ".min.js.map", ".min.map", ".min.css" });
    }
    private static ISet<string> ignoredExtensions;
    public static ISet<string> IgnoredExtensions { get { return ignoredExtensions; } }
    public int Priority
    {
      get { return 2; }
    }
    public static string GetIgnoredExtension(string relativeResourceName)
    {

      var extension = IgnoredExtensions.Where(ext => relativeResourceName.EndsWith(ext)).OrderBy(ext => ext.Length).FirstOrDefault();
      return extension??"";
    }
    public static string RelativeResourceNameToRelativePath(string relativeResourceName)
    {
      var ignoredExtension = GetIgnoredExtension(relativeResourceName);

      relativeResourceName = relativeResourceName.Replace('.', '/');
      var index = relativeResourceName.LastIndexOf('/');
      if (index >= 0)
      {
        var builder = new StringBuilder(relativeResourceName);
        builder[index] = '.';
        relativeResourceName = builder.ToString();
      }
      relativeResourceName =relativeResourceName.Substring(0, relativeResourceName.Length - ignoredExtension.Length) + ignoredExtension;
      return relativeResourceName;

    }


    public virtual IEnumerable<IManagedResource> Parse(string resourceName, string resourceKey, IProjectInfo info, object input,
      Assembly assembly)
    {
      // only embedded resources except the resource manifest
      if (!string.IsNullOrEmpty(resourceKey)) yield break;
      if (resourceName.EndsWith(".resources")) yield break;
      var stream = input as Stream;
      if (stream == null) yield break;
      string relativePath = "";
      if (resourceName.Contains("/")|| resourceName.Contains("\\"))
      {
        relativePath = resourceName.Replace('\\', '/');
        if(resourceName.StartsWith("/")) relativePath = resourceName.Substring(1);
        
      }
      else
      {
        if (!resourceName.StartsWith(info.AssemblyName)) yield break;

        var relativeResourceName = resourceName.Substring(info.AssemblyName.Length);
        if (relativeResourceName.StartsWith(".")) relativeResourceName = relativeResourceName.Substring(1);
        relativePath = RelativeResourceNameToRelativePath(relativeResourceName);
      }
      var projectDir = info.ProjectDir;


      var result = new FileResource(resourceName, stream, info.ProjectDir, relativePath, assembly);

      yield return result;
    }
  }
}
