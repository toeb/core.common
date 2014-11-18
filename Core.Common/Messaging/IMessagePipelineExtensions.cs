namespace Core.Modules.Messaging
{
  public static class IMessagePipelineExtensions
  {
    public static IMessageTracker<T> TrackMessages<T>(this IMessagePipeline pipeline)
    {
      var tracker = new MessageTracker<T>(pipeline);
      tracker.StartTracking();
      return tracker;
    }
    public static IMessageTracker TrackMessages(this IMessagePipeline pipeline)
    {
      var tracker = new MessageTracker(pipeline);
      tracker.StartTracking();
      return tracker;
    }
  }
}