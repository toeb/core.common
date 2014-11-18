using System;
using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public delegate MessageResponse MessageHandlerDelegate(MessageRequest request);
  public delegate MessageResponse MessageHandlerDelegate<T>(MessageRequest request, T message);
  public delegate Task<MessageResponse> AsyncMessageHandlerDelegate(MessageRequest request);
  public delegate Task<MessageResponse> AsyncMessageHandlerDelegate<T>(MessageRequest request, T message);
  public delegate Task<TOut> AsyncMessageHandlerDelegate<TIn,TOut>(MessageRequest request, TIn message);

  

  public class DelegateMessageHandler : IMessageHandler
  {
    public DelegateMessageHandler(AsyncMessageHandlerDelegate handler)
    {
      if (handler == null) throw new ArgumentNullException("handler");
      this.DelegateHandler = handler;
    }

    
    public async Task<MessageResponse> SendMessageAsync(MessageRequest request)
    {
      var result = await DelegateHandler(request);
      return result;
    }

    public AsyncMessageHandlerDelegate DelegateHandler { get; set; }
  }

  public class DelegateMessageHandler<T> : AbstractMessageHandler<T>
  {


    public DelegateMessageHandler(AsyncMessageHandlerDelegate<T> handler)
    {
      if (handler == null) throw new ArgumentNullException("handler");
      this.DelegateHandler = handler;
    }


    public AsyncMessageHandlerDelegate<T> DelegateHandler { get; set; }
    protected async override Task<MessageResponse> SendMessageAsync(MessageRequest request, T message)
    {
      var result = await DelegateHandler(request, message);
      return result;
    }
  }


  public class DelegateMessageHandler<TIn,TOut> : AbstractMessageHandler<TIn>
  {


    public DelegateMessageHandler(AsyncMessageHandlerDelegate<TIn,TOut> handler)
    {
      if (handler == null) throw new ArgumentNullException("handler");
      this.DelegateHandler = handler;
    }


    public AsyncMessageHandlerDelegate<TIn,TOut> DelegateHandler { get; set; }
    protected async override Task<MessageResponse> SendMessageAsync(MessageRequest request, TIn message)
    {
      try
      {
        var result = await DelegateHandler(request, message);
        return request.CreateReponse(result);
      }
      catch (Exception exception)
      {
        return request.CreateReponse(exception);
      }
    }
  }
}