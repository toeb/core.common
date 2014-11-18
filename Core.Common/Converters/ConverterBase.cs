
using System;
namespace Core.Converters
{

  public abstract class AbstractConverter<TFrom, TTo> : IConverter<TFrom, TTo>
  {
    public bool CanConvert(Type from, Type to)
    {
      return typeof(TFrom).IsAssignableFrom(from) && typeof(TTo).IsAssignableFrom(to);
    }

    object IConverter.Convert(object from)
    {
      var typedFrom = (TFrom)from;
      return this.Convert(typedFrom);
    }

    public abstract TTo Convert(TFrom f);

    public virtual string Format
    {
      get { return "no-format"; }
    }
  }

 
}
