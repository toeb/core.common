using System;
using System.Runtime.CompilerServices;

namespace Core.Modules.Messaging
{
  public static class MessageRequestExtensions
  {
    public static MessageResponse CreateReponse(this MessageRequest request, object payload = null, [CallerMemberName] string caller = null)
    {
      return new MessageResponse() { Request = request, Payload = payload, Origin = caller };
    }
    public static MessageResponse CreateUnhandledResponse(this MessageRequest request,[CallerMemberName] string caller = null)
    {
      return MessageResponse.Unhandled;
    }

  }
}
