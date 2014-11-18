using Core.Identification;
using Core.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{


  public class KeyValueRespositoryAssignableIdentity<TId, T> : KeyValueRepository<TId, T> where T : IIdentityAssignable<TId>
  {
    public KeyValueRespositoryAssignableIdentity(
      IKeyValueStore<TId, T> store,
      IIdentifierProvider<TId> idProvider,
      IEqualityComparer<TId> comparer
      )
      : base(store, idProvider, new IdentifiableIdentityAccessor<TId, T>(), comparer)
    {

    }
  }

}
