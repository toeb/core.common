using System.Collections.Generic;
using System.Linq;

namespace Core.Modules.Messaging
{
  class MessageTracker<T> : MessageTracker, IMessageTracker<T>
  {
    public MessageTracker(IMessagePipeline pipeline)
      : base(pipeline)
    {

    }
    public new IEnumerable<T> Messages { get { return base.Messages.OfType<T>(); } }

    protected override bool KeepMessage(MessageRequest request)
    {
      return request.Message is T;
    }
  }
}