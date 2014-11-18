using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identification
{
  public delegate void SetIdentity<TId, T>(T item, TId id);
  public delegate TId GetIdentity<TId, T>(T item);
  public class DelegateIdentityAccessor< T,TId> : IIdentityAccessor<T, TId>
  {
    public DelegateIdentityAccessor(GetIdentity<TId, T> get, SetIdentity<TId, T> set)
    {
      this.Getter = get;
      this.Setter = set;
    }
    protected DelegateIdentityAccessor()
    {

    }


    public SetIdentity<TId, T> Setter { get; protected set; }

    public GetIdentity<TId, T> Getter { get; protected set; }

    public TId GetIdentity(T item)
    {
      return Getter(item);
    }

    public void SetIdentity(T item, TId id)
    {
      Setter(item, id);
    }

    

  }
  public class PropertyIdentityAccessor<T,TId> : IIdentityAccessor< T,TId>
  {
    public PropertyIdentityAccessor(PropertyInfo property)
    {
      if (!typeof(T).IsAssignableFrom(property.DeclaringType)) throw new ArgumentException("property must be a property of T ");
      if (typeof(TId) != property.PropertyType) throw new ArgumentException("property's type must be of TId");
      Property = property;
    }
    public TId GetIdentity(T item)
    {
      return (TId)Property.GetValue(item);
    }

    public void SetIdentity(T item, TId id)
    {
      Property.SetValue(item, id);
    }

    public PropertyInfo Property { get; private set; }
  }
  public class IdentifiableIdentityAccessor<TId, T> : IIdentityAccessor<T, TId> where T : IIdentityAssignable<TId>
  {

    public TId GetIdentity(T item)
    {
      return item.Id;
    }

    public void SetIdentity(T item, TId id)
    {
      item.Id = id;
    }
  }

}
