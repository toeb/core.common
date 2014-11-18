using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identifiers
{
  public abstract class AbstractIdentifierProvider<TId> : IIdentifierProvider<TId>
  {
    public abstract TId CreateIdentifier();

    public Type GetIdentifierType()
    {
      return typeof(TId);
    }
    protected virtual void FreeIdentifier(TId id) { }
    /// <summary>
    /// Subclasses must implement this. It must return a identifier unique to this ProviderIdentity
    /// </summary>
    /// <returns></returns>
    object IIdentifierProvider.CreateIdentifier()
    {
      return CreateIdentifier();
    }


    void IIdentifierProvider.FreeIdentifier(object id)
    {
      FreeIdentifier((TId)id);
    }

    /// <summary>
    /// the default implementation returns the guid of this type.  subclasses may wish to change this
    /// </summary>
    public virtual Guid ProviderIdentity
    {
      get { return this.GetType().GUID; }
    }


    public virtual TId DefaultId
    {
      get { return default(TId); }
    }

    object IIdentifierProvider.DefaultId
    {
      get { return this.DefaultId; }
    }
  }
}
