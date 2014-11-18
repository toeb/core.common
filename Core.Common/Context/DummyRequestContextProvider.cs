using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Context
{
  /// <summary>
  /// a dummy request context provider
  /// usefull for unit tests
  /// </summary>
  public class DummyRequestContextProvider : IContextProvider
  {

    public DummyRequestContextProvider(IContextService service) { this.Service = service; }
    public ContextDescriptor ContextDescriptor
    {
      get { return ContextDescriptor.Request; }
    }

    public IContext GetContext()
    {
      if (Context == null)
      {
        Context = Service.RequireContext(ContextDescriptor);
        Context.Set("core:serverurl", "http://localhost:8080");
        Context.Set("core:requesturl", "http://localhost:8080/");
      }
      return Context;
    }

    /// <summary>
    /// call to simulate simulate a new request
    /// </summary>
    public void Reset()
    {
      if (Context == null) return;
      Service.ReleaseContext(Context);
      Context = null;
    }
    public IContext Context { get; set; }

    public IContextService Service { get; set; }
  }
}
