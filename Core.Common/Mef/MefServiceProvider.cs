using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Core
{
  [Export(typeof(IServiceProvider))]
  public class MefServiceProvider : IServiceProvider
  {
    [Import("ServiceContainer")]
    public CompositionContainer ServiceContainer { get; set; }
    public object GetService(Type serviceType)
    {
      throw new NotImplementedException();
    }
  }



}
