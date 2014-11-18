using System.ComponentModel.Composition;
using Core.Modules;
using Core.Modules.Applications;

namespace Core.Resources
{
  [Core.Modules.Module]  
  public class ResourcesModule
  {
    [ModuleInstance]
    IModuleInstance ModuleInstance{get;set;}


    [Export(typeof(IAssemblyResourceService))]
    AssemblyResourceService ResourceService
    {
      get;
      set;
    }

    [ActivationCallback]
    void Activate()
    {
      ResourceService = new AssemblyResourceService();
      ResourceService.AddResourceHandler(new EmbeddedResourceHandler());
      ResourceService.AddResourceHandler(new FileResourceHandler());
      ResourceService.AddResourceHandler(new StringResourceHandler());

    }
  }
}