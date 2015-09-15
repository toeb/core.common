using Core.Common.Reflect.Resources;
using System;
using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{
  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class ResourceProvider
  {
    private object context;
    public object Context { get { return context; } set { context = value; } }
  
  
    public ResourceProvider()
    {
    }
    [Import]
    public IResourceService ResourceService { get; set; }
    public object this[string key]
    {
      get
      {
        return ResourceService.GetResource(Context, key);
  
  
        //var resources = type.Assembly.GetManifestResourceNames();// AppDomain.CurrentDomain.GetAssemblies().Where(asm=>!asm.IsDynamic).SelectMany(asm =>  asm.GetManifestResourceNames()).ToArray();
  
        throw new NotImplementedException();
      }
    }
  }




}
