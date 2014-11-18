using System;
using System.Collections.Generic;

namespace Core
{


  public class ContextService : IContextService
  {
    private Dictionary<string, IContextProvider> providers = new Dictionary<string, IContextProvider>();

    public void AddContextProvider(IContextProvider provider)
    {
      providers[provider.ContextDescriptor.ContextKey] = provider;

    }
    public void RemoveContextProvider(IContextProvider provider)
    {
      if (!Has(provider.ContextDescriptor)) return;
      providers.Remove(provider.ContextDescriptor.ContextKey);

    }

    public bool Has(ContextDescriptor descriptor)
    {
      return providers.ContainsKey(descriptor.ContextKey);

    }

    public IContext GetContext(ContextDescriptor descriptor)
    {
      if (!Has(descriptor)) return null;
      return providers[descriptor.ContextKey].GetContext();

    }

    private IDictionary<Guid,IContext> openContexts = new Dictionary<Guid, IContext>();
    public IContext RequireContext(ContextDescriptor descriptor)
    {
      var context = new SimpleContext();
      context.ContextId = Guid.NewGuid();
      context.Descriptor = descriptor ;
      var provider = providers[descriptor.ContextKey];
      context.Provider = provider;
      context.Service = this;

      lock (openContexts)
      {
        openContexts[context.ContextId] = context;
      }
      return context;
     
    }

    public void ReleaseContext(IContext context)
    {
      context.Dispose();
      lock (openContexts)
      {
        openContexts.Remove(context.ContextId);
      }

    }


    public IContext GetContextById(Guid id)
    {
      lock (openContexts)
      {
        if (!openContexts.ContainsKey(id)) return null;
        return openContexts[id];
      }
    }
  }
}