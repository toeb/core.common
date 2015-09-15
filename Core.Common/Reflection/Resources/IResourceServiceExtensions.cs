using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Linq;

namespace Core.Common.Reflect.Resources
{
  public static class IResourceServiceExtensions
  {
  
    public static string[] GetPackUris(this IResourceService resourceService, Assembly assembly)
    {
      var assemblyResourceManager = resourceService.GetResourceManager(assembly);
      var resourceManager = assemblyResourceManager.ResourceManager;
      if (resourceManager == null) return new string[0];
      var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
  
  
      var files =
        from entry in resourceSet.OfType<DictionaryEntry>()
        let fileName = (string)entry.Key
        select ("/" + assembly.GetName().Name + ";component/" + fileName).ToLowerInvariant();
  
      return files.ToArray();
    }
  
    public static string[] GetAllPackUris(this IResourceService resourceService)
    {
      return resourceService.GetAllResourceManagers().SelectMany(arm => resourceService.GetPackUris(arm.Assembly)).ToArray();
    }
  
  }
}
