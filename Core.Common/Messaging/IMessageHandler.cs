using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public interface IMessageHandler
  {
    
    /// <summary>
    /// handle the message return null or a task with null result to indicate the message cannot be handled
    /// throw an exception if the message looked like it could be handled but was faulted
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<MessageResponse> SendMessageAsync(MessageRequest request);
  }
}