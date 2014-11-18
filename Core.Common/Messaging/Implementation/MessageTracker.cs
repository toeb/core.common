using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace Core.Modules.Messaging
{
  class MessageTracker : IMessageHandler, IMessageTracker
  {
    public MessageTracker(IMessagePipeline pipeline)
    {
      this.MessagePipeline = pipeline;
    }

    List<MessageRequest> requests = new List<MessageRequest>();
    public IEnumerable<object> Messages { get { return requests.Select(req => req.Message); } }
    protected virtual bool KeepMessage(MessageRequest request)
    {
      return true;
    }
    public async Task<MessageResponse> SendMessageAsync(MessageRequest request)
    {
      if (KeepMessage(request)) requests.Add(request);
      return request.CreateUnhandledResponse();
    }

    public IMessagePipeline MessagePipeline { get; set; }

    public IEnumerable<MessageRequest> Requests
    {
      get { return requests; ; }
    }


    public void StartTracking()
    {
      MessagePipeline.InsertMessageHandler(0,this);
    }

    public void StopTracking()
    {
      MessagePipeline.RemoveMessageHandler(this);
    }

    public void Dispose()
    {
      StopTracking();
    }


    public void Reset()
    {
      this.requests.Clear();      
    }
  }
}