using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{

  public abstract class AbstractExtendedConverter<TFrom, TTo> : AbstractExtendedConverter
  {

    protected AbstractExtendedConverter()
    {
      AddConversion(Formats.Default<TFrom>(), Formats.Default<TTo>());
    }
    protected AbstractExtendedConverter(string fromFormatName, string toFormatName)
    {
      AddConversion(Formats.Create<TFrom>(fromFormatName), Formats.Create<TTo>(toFormatName));
    }

    protected abstract TTo Convert(TFrom value, ConversionRequest request);

    protected sealed override ConversionResponse DoConvert(ConversionRequest request)
    {
      if (request.Simulate) return request.SimulateSuccess();

      var result = Convert((TFrom)request.Source, request);
      return request.Success(result);
    }
  }
}
