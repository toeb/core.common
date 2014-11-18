using System;
using System.Runtime.CompilerServices;
namespace Core.Modules.Messaging
{
  public class MessageRequest
  {
    public object Origin { get; set; }
    /// <summary>
    /// the message object
    /// </summary>
    public object Message { get; set; }
    /// <summary>
    /// Parameters used to descript the Message Request
    /// </summary>
    public object Parameters { get; set; }

    public static MessageRequest CreateFromMethod(object message, [CallerMemberName] string callerName = null)
    {
      var result = new MessageRequest();
      result.Origin = callerName;
      result.Message = message;
      return result;
    }
    public static MessageRequest CreateFromMethod([CallerMemberName] string callerName = null)
    {
      var result = new MessageRequest();
      result.Origin = callerName;
      return result;
    }

    public MessageRequest Parent { get; private set; }

    internal MessageRequest CloneInternal() { return Clone(); }

    protected virtual MessageRequest Clone()
    {
      var clone = this.MemberwiseClone() as MessageRequest;
      clone.Parent = this;
      return clone;
    }
  }

}