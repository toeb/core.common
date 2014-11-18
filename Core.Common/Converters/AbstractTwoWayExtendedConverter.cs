using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions.Reflection;
namespace Core.Converters
{
  

  public abstract class AbstractTwoWayExtendedConverter<TA, TB> : AbstractExtendedConverter
  {
    protected abstract TB Convert(TA from, ConversionRequest request);
    protected abstract TA Convert(TB from, ConversionRequest request);
    protected AbstractTwoWayExtendedConverter()
    {
      var formatA = Formats.Default<TA>();
      var formatB = Formats.Default<TB>();

      AddConversion(formatA, formatB);
      AddConversion(formatB, formatA);
    }
    protected AbstractTwoWayExtendedConverter(string aFormat, string bFormat)
    {
      var formatA = Formats.Create<TA>(aFormat);
      var formatB = Formats.Create<TB>(bFormat);

      AddConversion(formatA, formatB);
      AddConversion(formatB, formatA);
    }
    protected sealed override ConversionResponse DoConvert(ConversionRequest request)
    {
      try
      {
        if (typeof(TA).IsSuperclassOfOrSameClass(request.SourceType) && request.TargetType.IsSuperclassOfOrSameClass(typeof(TB)))
        {
          if (request.Simulate) return request.SimulateSuccess();
          var result = Convert((TA)request.Source, request);
          return request.Success(result);
        }
        else if (typeof(TB).IsSuperclassOfOrSameClass(request.SourceType) && request.TargetType.IsSuperclassOfOrSameClass(typeof(TA)))
        {
          if (request.Simulate) return request.SimulateSuccess();
          var result = Convert((TB)request.Source, request);
          return request.Success(result);
        }
        else
        {
          return request.Error("cannot convert");
        }
      }
      catch (ConversionErrorException exception)
      {
        return request.Error(exception.Message);
      }
    }
  }
}
