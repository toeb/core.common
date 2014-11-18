using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public static class IMessageHandlerExtensions
  {
    /// <summary>
    /// returns a task returning MessageResponse.Unhandled if the messagehandler did not handle the message
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static async Task<MessageResponse> SendMessageAsyncEnsureConsistentResult(this IMessageHandler self, MessageRequest request)
    {
      var task = self.SendMessageAsync(request);
      if (task == null) return MessageResponse.Unhandled;
      var result = await task;
      if (result == null) return MessageResponse.Unhandled;
      return result;
    }
  }
}