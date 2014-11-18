
namespace Core.Modules.Messaging
{
  public class MessageResponse
  {
    /// <summary>
    /// the request which was used to generate this response
    /// </summary>
    public MessageRequest Request{get;set;}
    public object Payload { get; set; }
    public object Origin { get; set; }


    public static readonly MessageResponse Unhandled = new MessageResponse();

    public MessageResponse Parent { get; private set; }

    internal MessageResponse Clone()
    {

      var clone = this.MemberwiseClone() as MessageResponse;
      clone.Parent = this;
      return clone;
    }
  }
}