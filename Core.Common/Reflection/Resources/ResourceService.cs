using Core.Common.Reflect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Core.Common.Reflect.Resources
{
  public interface IResourceFinder
  {
    object FindResource(object context, object identifier);
  }
  [Export(typeof(IResourceService))]
  public class ResourceService : IResourceService, IResourceFinder
  {

    [Import]
    IReflectionService ReflectionService { get; set; }

    [ImportMany]
    IResourceFinder[] ResourceHandlers
    {
      get;
      set;
    }

    public object GetResource(object context, object identifier)
    {


      foreach (var finder in ResourceHandlers ?? Enumerable.Empty<IResourceFinder>())
      {
        if (finder == this) continue;
        var resource = finder.FindResource(context, identifier);
        if (resource != null) return resource;
      }

      return this.FindResource(context, identifier);
    }


    public AssemblyResourceManager[] GetAllResourceManagers()
    {


      return ReflectionService.Assemblies.Select(asm => GetResourceManager(asm)).Where(rm => rm != null).ToArray();

    }
    private static IDictionary<Assembly, AssemblyResourceManager> resourceManagers = new Dictionary<Assembly, AssemblyResourceManager>();
    public AssemblyResourceManager GetResourceManager(Assembly assembly)
    {
      lock (resourceManagers)
      {
        if (resourceManagers.ContainsKey(assembly)) return resourceManagers[assembly];

        var resourcesName = assembly.GetName().Name + ".g";
        if (assembly.IsDynamic)
        {
          resourceManagers[assembly] = null;
          return null;
        }
        if (!assembly.GetManifestResourceNames().Contains(resourcesName + ".resources"))
        {
          resourceManagers[assembly] = null;
          return null;

        }
        var manager = new System.Resources.ResourceManager(resourcesName, assembly);
        var assemblyResourceManager = new AssemblyResourceManager()
        {
          Assembly = assembly,
          ResourceManager = manager
        };
        resourceManagers[assembly] = assemblyResourceManager;
        return assemblyResourceManager;
      }
    }

    static string[] packUris = null;

    public object FindResource(object context, object identifier)
    {
      packUris= packUris ?? this.GetAllPackUris();
      if (!(identifier is string)) return null;
      var id = identifier as string;
      id = id.ToLowerInvariant();

      var result = packUris.FirstOrDefault(uri => uri.Equals(id))
        ??
        packUris.FirstOrDefault(uri => uri.EndsWith(id))
        ??
        packUris.FirstOrDefault(uri => uri.Contains(id));
      return result;

    }
  }
}
