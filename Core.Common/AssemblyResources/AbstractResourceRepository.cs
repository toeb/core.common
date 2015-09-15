using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{


  public abstract class AbstractResourceRepository<TKey, TResource> : NotifyPropertyChangedBase, IResourceRepository<TKey, TResource>
  {
    protected abstract TResource ConstructResource(TKey key);
    protected abstract void StoreResource(TResource resource);
    protected abstract IEnumerable<TKey> GetKeys();
    protected abstract bool IsIdentifiedBy(TResource resource, TKey key);
    protected virtual IEnumerable<TResource> GetResources()
    {
      return Keys.Select(k => Retrieve(k));
    }
    protected void NotifyResourceAdded(TResource resource)
    {
      RaisePropertyChanged(ResourcesName);
    }
    private readonly string ResourcesName = "Resources";

    public virtual bool Has(TKey key)
    {
      return Resources.Any(r => IsIdentifiedBy(r, key));
    }

    public TResource Require(TKey key)
    {
      if (Has(key)) return Retrieve(key);
      var resource = ConstructResource(key);
      StoreResource(resource);
      return resource;
    }
    public IEnumerable<TResource> Resources
    {
      get { return GetResources(); }
    }


    public virtual TResource Retrieve(TKey key)
    {
      return Resources.Single(r => IsIdentifiedBy(r, key));
    }


    public IEnumerable<TKey> Keys
    {
      get { return GetKeys(); }
    }
  }
}
