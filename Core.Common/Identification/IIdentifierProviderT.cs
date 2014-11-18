using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identifiers
{

  public interface IIdentifierProvider<TId> : IIdentifierProvider
  {
    /// <summary>
    /// Subclasses must implement this. It must return a identifier unique to this ProviderIdentity
    /// </summary>
    /// <returns></returns>
    new TId CreateIdentifier();
    new TId DefaultId { get; }
  }
}
