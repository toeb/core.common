using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{

  public abstract class ResourceRepositoryBase<TKey, TResource> : AbstractResourceRepository<TKey, TResource>
   where TResource : IIdentifiable<TKey>
  {
    private IDictionary<TKey, TResource> resources = new Dictionary<TKey,TResource>();

    protected override void StoreResource(TResource resource)
    {
      resources[resource.Id] = resource;
    }
    public override bool Has(TKey key)
    {
      return resources.ContainsKey(key);
    }
    protected override IEnumerable<TResource> GetResources()
    {
      return resources.Values;
    }
    protected override IEnumerable<TKey> GetKeys()
    {
      return resources.Keys;
    }
    protected override bool IsIdentifiedBy(TResource resource, TKey key)
    {
      return resource.Id.Equals(key);
    }
  }
}
