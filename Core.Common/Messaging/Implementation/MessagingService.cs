using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  /// <summary>
  /// Simple Messaging Service
  /// 
  /// maintains a set of message handlers which are chosen depending on the message request
  /// </summary>
  public class MessagingService : IMessagingService, IMessagePipeline, IDisposable
  {
    public MessagingService()
    {
      HandlerSet = new List<IMessageHandler>();
    }

    public IList<IMessageHandler> HandlerSet{ get; set; }


    public void AddMessageHandler(IMessageHandler handler)
    {
      HandlerSet.Add(handler);
    }

    public void RemoveMessageHandler(IMessageHandler handler)
    {
      HandlerSet.Remove(handler);
    }

    public async Task<MessageResponse> SendMessageAsync(MessageRequest request)
    {
      if (request.Message == null) throw new ArgumentException("request's may not be null", "request");


      var cloned = request.CloneInternal();

      // call first handler to successfully handle message
      MessageResponse response = null;
      foreach (var handler in MessageHandlers)
      {
        response = await handler.SendMessageAsyncEnsureConsistentResult(cloned);
        if (response != MessageResponse.Unhandled) break;
      }
      if (response != null)
      {
        response = response.Clone();
        response.Request = request;
      }
      return response ?? MessageResponse.Unhandled;
    }


    public IEnumerable<IMessageHandler> MessageHandlers
    {
      get { return HandlerSet; }
    }


    

    public void InsertMessageHandler(int index, IMessageHandler handler)
    {
      HandlerSet.Insert(index, handler);
    }

    public void Dispose()
    {
    
    }
  }
}