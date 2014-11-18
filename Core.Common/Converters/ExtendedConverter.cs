using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{


  public static class ExtendedConverter
  {
    public static IEnumerable<IExtendedConverter> Converters { get; private set; }
    static ExtendedConverter()
    {
      Converters = Core.ReflectionService.Instance.AllTypes
        .Where(type => type.ImplementsInterface<IExtendedConverter>() && !type.IsAbstract && !type.IsInterface)
        .Select(type => System.Activator.CreateInstance(type) as IExtendedConverter)
        .ToArray();
    }

    public static bool CanConvert(this ConversionRequest request, Type from, Type to)
    {
      var converter = request.Find(from, to);
      return converter != null;
    }


    public static IExtendedConverter Find(this ConversionRequest request, ConversionRequest subRequest)
    {
      return Find(subRequest);
    }
    public static IExtendedConverter Find(this ConversionRequest request, Type from, Type to)
    {
      return Find(from, to);
    }

    public static IExtendedConverter Find(ConversionRequest request)
    {
      return Converters.FirstOrDefault(c => c.CanConvert(request));
    }
    public static IExtendedConverter Find(Type from, Type to)
    {
      return Find(Request(from, to));
    }
    public static IExtendedConverter Find<TFrom, TTo>()
    {
      return Find(typeof(TFrom), typeof(TTo));
    }

    public static object Convert(object from, Type fromType, Type toType)
    {
      var request = Request(from, fromType, null, toType);
      var converter = Find(request);
      if (converter == null) throw new InvalidOperationException("did not find converter from "+fromType+" to "+toType);
      var response = converter.Convert(request);
      return response.Result;
    }

    public static T Convert<T>(object from, Type fromType = null)
    {
      if (fromType == null) fromType = from.GetType();
      var res = Convert(from, fromType, typeof(T));
      return (T)res;
    }
    public static ConversionRequest Derive(this ConversionRequest request)
    {
      return request.New();
    }
    public static ConversionRequest Derive(this ConversionRequest request, Action<ConversionRequest> configure)
    {
      return request.Derive().Configure(configure);
    }
    public static ConversionRequest Derive(this ConversionRequest request, object from, IFormat fromFormat, object to, IFormat toFormat)
    {
      return request.New(ctx => { ctx.SourceFormat = fromFormat; ctx.TargetFormat = toFormat; });
    }
    public static ConversionRequest Derive(this ConversionRequest request, object from, Type fromType, object to, Type toType)
    {
      return request.Derive(from, request.SourceFormat.Derive(fromType), to, request.TargetFormat.Derive(toType));
    }
    public static ConversionRequest Derive(this ConversionRequest request, object from, Type toType, Type fromType=null)
    {
      if (fromType == null) fromType = from.GetType();
      return request.Derive(from, fromType, null, toType);
    }



    public static ConversionRequest Request(object from, IFormat fromFormat, object to, IFormat toFormat)
    {
      return new ConversionRequest() { SourceFormat = fromFormat, TargetFormat = toFormat, Source = from, ExistingValue = to };
    }
    public static ConversionRequest Request(object from, Type fromType, object to, Type toType)
    {
      return Request(from, Formats.Default(fromType), to, Formats.Default(toType));
    }
    public static ConversionRequest Request<TFrom, TTo>(TFrom from, TTo to)
    {
      return Request(from, typeof(TFrom), to, typeof(TTo));
    }
    public static ConversionRequest Request(object from, Type to)
    {
      if (from == null) throw new ArgumentNullException();
      var fromType = from.GetType();
      return Request(from, fromType, null, to);
    }
    public static ConversionRequest Request(Type from, Type to, string formatFrom, string formatTo)
    {
      return Request(null, Formats.Create(from, formatFrom), null, Formats.Create(to, formatTo));
    }
    public static ConversionRequest Request(Type from, Type to)
    {
      return Request(null, from, null, to);
    }
  }
}
