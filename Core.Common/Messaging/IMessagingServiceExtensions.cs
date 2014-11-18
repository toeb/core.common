using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core.Modules.Messaging{
  public static class IMessagingServiceExtensions
  {
    /// <summary>
    /// sends the specified object as a message
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task<MessageResponse> SendMessageAsync(this IMessagingService self, object message)
    {
      var request = new MessageRequest();
      request.Message = message;
      var response = await self.SendMessageAsync(request);
      return response;
    }
    /// <summary>
    /// sends a message with the specified parameters
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static async Task<MessageResponse> SendMessageAsync(this IMessagingService self, object message, object parameters)
    {
      var request = new MessageRequest();
      request.Message = message;
      request.Parameters = parameters;
      var response = await self.SendMessageAsync(request);
      return response;
    }

    /// <summary>
    /// throws if message was not handled
    /// </summary>
    /// <param name="self"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<MessageResponse> SendMessageAsyncThrowOnFail(this IMessagingService self,MessageRequest request )
    {
      var response = await self.SendMessageAsync(request);
      if (response == MessageResponse.Unhandled) throw new InvalidOperationException("no message handle could handle message");
      return response;
    }

    
  }
}