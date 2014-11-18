using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{
  public delegate T ProduceValueDelegate<out T, in TInput>(TInput intput);

  public interface IResourceRepository<TKey, TResource>
  {
    bool Has(TKey key);
    TResource Require(TKey key);
    IEnumerable<TKey> Keys { get; }
    IEnumerable<TResource> Resources { get; }
    TResource Retrieve(TKey key);
  }

}
