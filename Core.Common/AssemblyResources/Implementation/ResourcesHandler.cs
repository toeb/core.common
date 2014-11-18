using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Core.Resources
{
  [Export(typeof(IResourceHandler))]
  public class ResourcesHandler : IResourceHandler
  {
    [ImportMany]
    public IEnumerable<IResourceHandler> Handlers { get; set; }


    public int Priority
    {
      get { return 0; }
    }

    public IEnumerable<IManagedResource> Parse(string resourceName, string resourceKey, IProjectInfo info, object input,
      Assembly assembly)
    {
      if (!string.IsNullOrEmpty(resourceKey)) return ManagedResource.None;
      if (!resourceName.EndsWith(".resources")) return ManagedResource.None;
      var stream = input as Stream;
      if (stream == null) return ManagedResource.None;


      var result =
        new ResourceReader(stream)
         .OfType<DictionaryEntry>()
         .Select(entry => new { key = (string)entry.Key, entry.Value })
         .SelectMany(
           entry =>
             Handlers.OrderBy(handler => handler.Priority)
             .Select(handler => handler.Parse(resourceName, entry.key, info, entry.Value, assembly))
             .SkipWhile(resources => !resources.Any())
             .FirstOrDefault(resources => resources != null) ?? ManagedResource.None
         )
         .ToArray();

      return result;
    }
  }
}
