using System.Collections.Generic;

namespace Core.Modules.Messaging
{
  /// <summary>
  /// message pipeline interface 
  /// </summary>
  public interface IMessagePipeline
  {
    /// <summary>
    /// returns all message handlers
    /// </summary>
    IEnumerable<IMessageHandler> MessageHandlers { get; }

    /// <summary>
    /// inserts a messagehandler at the specified index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="handler"></param>
    void InsertMessageHandler(int index, IMessageHandler handler);

    /// <summary>
    /// adds a message handler to the pipeline
    /// </summary>
    /// <param name="handler"></param>
    void AddMessageHandler(IMessageHandler handler);
    /// <summary>
    /// removes a messagehandler from the pipeline
    /// </summary>
    /// <param name="handler"></param>
    void RemoveMessageHandler(IMessageHandler handler);
  }
}