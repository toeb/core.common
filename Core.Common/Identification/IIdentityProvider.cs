using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  [InheritedExport]
  public interface IIdentityProvider
  {
    object GetIdentity(object subject);
  }
  [InheritedExport]
  public interface IIdentityProvider<T> : IIdentityProvider
  {
    object GetIdentity(T subject);
  }
  [InheritedExport]
  public interface IIdentityProvider<T, TId> : IIdentityProvider<T>
  {
    new TId GetIdentity(T subject);
  }




  public class IdentifieableIdentityProvider<T, TId> : IIdentityProvider<T, TId> where T : IIdentifiable<TId>
  {

    public TId GetIdentity(T subject)
    {
      return subject.Id;
    }

    object IIdentityProvider<T>.GetIdentity(T subject)
    {
      return GetIdentity(subject);
    }

    public object GetIdentity(object subject)
    {
      return GetIdentity(subject);
    }
  }

}
