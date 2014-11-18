using Core.Modules.Messaging;
using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public abstract class AbstractMessageHandler<T> : IMessageHandler
  {

    protected abstract Task<MessageResponse> SendMessageAsync(MessageRequest request, T message);
    


    async Task<MessageResponse> IMessageHandler.SendMessageAsync(MessageRequest request)
    {
      if (!(request.Message is T)) return MessageResponse.Unhandled;
      var result = await SendMessageAsync(request, (T) request.Message);
      return result;
    }

  }
}