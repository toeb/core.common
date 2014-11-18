
using System;
using System.ComponentModel.Composition;

namespace Core.Converters
{
  [Export]
  [Export(typeof(IConverter))]
  [Export(typeof(IConverter<,>))]
  public class TToTConverter<T> : AbstractConverter<T, T>
  {
    public override T Convert(T f)
    {
      return f;
    }
  }


}
