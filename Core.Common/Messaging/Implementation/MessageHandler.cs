using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public static class MessageHandler
  {
    public static IMessageHandler CreateHandler(MessageHandlerDelegate handler)
    {
      return CreateAsyncHandler(async request => handler(request));
    }
    public static IMessageHandler CreateAsyncHandler(AsyncMessageHandlerDelegate handler)
    {
      return new DelegateMessageHandler(handler);
    }
    public static IMessageHandler CreateAsyncHandler<T>(AsyncMessageHandlerDelegate<T> handler)
    {
      return new DelegateMessageHandler<T>(handler);
    }

    public static IMessageHandler CreateAsyncHandler<TIn, TOut>(AsyncMessageHandlerDelegate<TIn, TOut> handler)
    {
      return new DelegateMessageHandler<TIn, TOut>(handler);
    }
    public static IMessageHandler CreateHandler<T>(MessageHandlerDelegate<T> handler)
    {
      return new DelegateMessageHandler<T>(async (request, message) => { return handler(request, message); });
    }
  }
}