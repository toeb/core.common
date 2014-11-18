using Core.Identification;
using Core.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Store.KeyValue
{

  public class KeyValueRepositoryGuidIdentityAssignable<T> : KeyValueRespositoryAssignableIdentity<Guid, T> where T : IIdentityAssignable<Guid>
  {
    
    public KeyValueRepositoryGuidIdentityAssignable(IKeyValueStore<Guid, T> store) : base(store, new GuidIdentifierProvider(), new GuidEqualityComparer()) { }

  }


}
