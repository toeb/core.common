using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{

  public class DelegateResource<TKey, TResource> : ResourceRepositoryBase<TKey, TResource>
    where TResource : IIdentifiable<TKey>
  {
    public DelegateResource(ProduceValueDelegate<TResource, TKey> construct)
    {
      this.ConstructionDelegate = construct;
    }


    protected override TResource ConstructResource(TKey key)
    {
      return ConstructionDelegate(key);
    }

    private ProduceValueDelegate<TResource, TKey> ConstructionDelegate { get; set; }
  }
}
