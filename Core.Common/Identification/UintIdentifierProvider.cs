using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identification
{
  public class UintIdentifierProvider : Core.Identifiers.AbstractIdentifierProvider<uint>
  {
    private uint id = 1;
    private Guid providerIdentity;
    public UintIdentifierProvider(Guid id) { providerIdentity = id; }
    public override Guid ProviderIdentity
    {
      get { return providerIdentity; }
    }
    public override uint DefaultId
    {
      get
      {
        return 0;
      }
    }
    public override uint CreateIdentifier()
    {
      return id++;
    }
  }
}
