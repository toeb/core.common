using System;

namespace Core.Identifiers
{
  public interface IIdentifierProvider
  {
    object DefaultId { get; }
    Guid ProviderIdentity { get; }
    Type GetIdentifierType();
    /// <summary>
    /// Subclasses must implement this. It must return a identifier unique to this ProviderIdentity
    /// </summary>
    /// <returns></returns>
    object CreateIdentifier();
    void FreeIdentifier(object id);
  }
}
