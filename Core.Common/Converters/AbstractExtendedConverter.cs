using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{
  [DebuggerDisplay("{From} => {To}")]
  public struct Conversion
  {
    public Conversion(IFormat from, IFormat to)
    {
      From = from;
      To = to;
    }
    public IFormat From;
    public IFormat To;
  }

  public abstract class AbstractExtendedConverter : IExtendedConverter
  {
    protected AbstractExtendedConverter()
    {

    }
    protected void AddConversion(IFormat from, IFormat to)
    {
      conversions.Add(new Conversion(from, to));
    }


    private IList<Conversion> conversions = new List<Conversion>();
    public IEnumerable<Conversion> Conversions { get { return conversions; } }

  


    protected abstract ConversionResponse DoConvert(ConversionRequest request);

    public ConversionResponse Convert(ConversionRequest request)
    {
      var hasConversion = Conversions.Any(conversion =>
        conversion.From.IsAssignableFrom(request.SourceFormat)
        && conversion.To.IsAssignableTo(request.TargetFormat)
        );

      if (!hasConversion) return request.Error("no conversion available");


      return DoConvert(request);

    }
  }
}
