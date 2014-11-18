using System;
using System.ComponentModel.Composition;

namespace Core.Converters
{
  [Export]
  [Export(typeof(IConverter))]
  public class ToStringConverter<TFrom> : AbstractConverter<TFrom, string>
  {
    public override string Convert(TFrom f)
    {
      return f.ToString();
    }
  }
}
