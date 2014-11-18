using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{

  public class LogEntry
  {
    public string Message { get; set; }
    public bool Error { get; set; }
  }
  public class ConversionResponse : ScopeBase<ConversionResponse>
  {
    public static explicit operator bool(ConversionResponse response)
    {
      return response.HasError;
    }

    private ICollection<LogEntry> log = new List<LogEntry>();
    internal ConversionResponse(ConversionRequest request)
    {
      Log = log;
      Request = request;
      
    }
    public ConversionRequest Request { get { return Get<ConversionRequest>(); } set { Set(value); } }
    public IEnumerable<LogEntry> Log { get { return Get<IEnumerable<LogEntry>>(); } private set { Set(value); } }
    internal void AddLogMessage(string message)
    {
      var logMessage = new LogEntry() { Message = message };
      log.Add(logMessage);
    }
    internal void AddErrorMessage(string message)
    {
      var errorMessage = new LogEntry() { Error = true, Message = message };
      log.Add(errorMessage);
    }
    public bool HasError { get { return Log.Any(msg => msg.Error); } }

    public ConversionResponse InnerResponse { get { return Get<ConversionResponse>(); } set { Set(value); } }

    public object Result { get { return Get<object>(); } set { Set(value); } }
  }

  public class CompositeConversionResponse : ConversionResponse
  {
    private ICollection<ConversionResponse> responses = new List<ConversionResponse>();
    public CompositeConversionResponse(ConversionRequest request):base(request)
    {
      Responses = responses;
    }
    internal void AddResponse(ConversionResponse childResponse)
    {
      responses.Add(childResponse);
    }
    public IEnumerable<ConversionResponse> Responses { get { return Get<IEnumerable<ConversionResponse>>(); } private set { Set(value); } }
  }

  public static class ConversionResponseExtensions
  {
    public static T Error<T>(this T response, string message) where T : ConversionResponse
    {
      response.AddErrorMessage(message);

      return response;
    }
    public static T Log<T>(this T response, string message) where T : ConversionResponse
    {

      response.AddLogMessage(message);

      return response;
    }
  }
  public interface IExtendedConverter
  {
    //bool CanConvert(ConversionRequest request);
    ConversionResponse Convert(ConversionRequest request);
  }

  public static class IExtendedConverterExtensions
  {
    public static bool CanConvert(this IExtendedConverter @this, ConversionRequest request)
    {
      if (!request.Simulate) request = request.Derive(req => req.Simulate = true);
      var response = @this.Convert(request);
      return !response.HasError;
    }
  }

}
