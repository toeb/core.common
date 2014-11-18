using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Core.Resources
{
  public class AssemblyResourceService :
    ResourceRepositoryBase<Guid, IAssemblyResources>,
    IAssemblyResourceService
  {

    private IDictionary<Guid, Assembly> accessedAssemblies = new Dictionary<Guid, Assembly>();
    private ISet<IResourceHandler> resourceHandlers = new HashSet<IResourceHandler>();

    public IAssemblyResources GetResources(Assembly assembly)
    {
      var id = assembly.GetGuid();
      accessedAssemblies[id] = assembly;
      return Require(id);
    }


    public void AddResourceHandler(IResourceHandler handler)
    {
      resourceHandlers.Add(handler);
    }

    public void RemoveResourceHandler(IResourceHandler handler)
    {
      resourceHandlers.Remove(handler);
    }

    public IEnumerable<IResourceHandler> ResourceHandlers { get { return resourceHandlers; } }

    protected override IAssemblyResources ConstructResource(Guid key)
    {
      var assembly = accessedAssemblies[key];

      var instance =new AssemblyResources(assembly, this);
      instance.Parse();
      return instance;
    }
  }
}
