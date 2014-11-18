using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Strings;
namespace Core.Converters
{
  [DebuggerDisplay("convert {SourceFormat} => {TargetFormat}")]
  public class ConversionRequest : ScopeBase<ConversionRequest>
  {
    public override string ToString()
    {
      return string.Format("convert {0}=>{1}", SourceFormat, TargetFormat);
    }
    /// <summary>
    /// Set to true if you do not actually want to convert
    /// </summary>
    public bool Simulate { get { return Get<bool>(); } set { Set(value); } }

    public IFormat SourceFormat { get { return Get<IFormat>(); } set { Set(value); } }
    public IFormat TargetFormat { get { return Get<IFormat>(); } set { Set(value); } }
    public object Source { get { return Get<object>(); } set { Set(value); } }    
    public object ExistingValue { get { return Get<object>(); } set { Set(value); } }

    public Type SourceType { get { return SourceFormat.Type; } }
    public Type TargetType { get { return TargetFormat.Type; } }
    public void ConversionError(string message)
    {
      var msg = @"Conversion Error: {message}
 types: {from}=>{to}".FormatWith(new { message, from = SourceFormat.ToString(), to = TargetFormat.ToString() });
      throw new ConversionErrorException(msg);
    }
  }

  public static class ConversionRequestExtensions
  {
    public static ConversionResponse Error(this ConversionRequest request,string message)
    {
      return new ConversionResponse(request).Error(message);
    }
    public static T Simulate<T>(this T request, bool simulate = true)where T: ConversionRequest
    {
      request.Simulate = simulate;
      return request;
    }
    public static ConversionResponse SimulateSuccess(this ConversionRequest request)
    {
      if (!request.Simulate)
      {
        return request.Error("no result value was generated");
      }
      return request.CreateResponse();
    }
    public static ConversionResponse Success(this ConversionRequest request, object result)
    {
      var response = request.CreateResponse();
      response.Result = result;
      return response;
    }
    public static ConversionResponse Success(this ConversionRequest request, ConversionResponse subResponse)
    {
        var result = request.Success(subResponse.Result);
        result.InnerResponse = subResponse;
        return result;
    }
    public static ConversionResponse Error(this ConversionRequest request, ConversionResponse subResponse, string message)
    {
      var result = request.Error(message);
      result.InnerResponse = subResponse;
      return result;
    }
    public static ConversionResponse Response(this ConversionRequest request, ConversionResponse subRespnse)
    {
      var result = request.CreateResponse();
      result.InnerResponse = subRespnse;
      if (result.InnerResponse.HasError) result.Error("see inner response error");
      return result;
    }
    public static ConversionResponse CreateResponse(this ConversionRequest request)
    {
      return new ConversionResponse(request) ;
    }

  }


}
