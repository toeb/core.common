using System.ComponentModel.Composition;
using Core.Modules;

namespace Core.Modules.Messaging
{
  [Module]
  public class MessagingModule
  {
    [Export]
    [Export(typeof(IMessagingService))]
    [Export(typeof(IMessagePipeline))]
    MessagingService MessagingService { get; set; }

    [ActivationCallback]
    void Activate()
    {
      MessagingService = new MessagingService(); 
    }
  }
}