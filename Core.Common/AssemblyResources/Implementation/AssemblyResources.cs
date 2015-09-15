using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using Core.Common.Reflect;
namespace Core.Resources
{
  public class AssemblyResources :
    ResourceRepositoryBase<string, IManagedResource>,
    IAssemblyResources
  {
    private AssemblyResourceService assemblyResourceService;


    public AssemblyResources(Assembly assembly, AssemblyResourceService assemblyResourceService)
    {
      this.Assembly = assembly;
      this.assemblyResourceService = assemblyResourceService;
    }

    public Assembly Assembly
    {
      get;
      private set;
    }

    public Guid Id
    {
      get { return Assembly.GetGuid(); }
    }

    protected override IManagedResource ConstructResource(string key)
    {
      throw new NotImplementedException();
    }

    
    IEnumerable<IResourceHandler> ResourceHandlers
    {
      get { return assemblyResourceService.ResourceHandlers; }
    }

    IEnumerable<IManagedResource> ParseResource(string name, Assembly assembly, IProjectInfo info)
    {

      var stream = assembly.GetManifestResourceStream(name);
      if (stream == null) return null;


      var resources = ResourceHandlers
        .OrderBy(handler => handler.Priority)
        .Select(handler => handler.Parse(name, null, info, stream, assembly))
        .SkipWhile(result => !result.Any())
        .FirstOrDefault() ?? ManagedResource.None;
      return resources;
    }
    public void Parse()
    {
      var resources = ParseAssembly(Assembly);
      foreach (var resource in resources)
      {
        this.StoreResource(resource);
      }
    }
    public IEnumerable<IManagedResource> ParseAssembly(Assembly assembly)
    {
      var info = this.ProjectInfo = assembly.GetProjectInfo();
      var resourceNames = assembly.GetManifestResourceNames();
      var resources = resourceNames
        .SelectMany(name => ParseResource(name, assembly, info) ?? ManagedResource.None)
        .ToArray();
      return resources;


    }

    public IProjectInfo ProjectInfo { get; set; }
  }

}
