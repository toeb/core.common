using System;
using System.Linq;
using System.Collections.Generic;

namespace Core.Modules.Messaging
{
  public interface IMessageTracker<T> : IMessageTracker
  {
    new IEnumerable<T> Messages { get; }

  }

  public interface IMessageTracker : IDisposable
  {

    IEnumerable<MessageRequest> Requests { get; }
    IEnumerable<object> Messages { get; }
    void StartTracking();
    void StopTracking();
    void Reset();
  }

  public static class IMessageTrackerExtensions
  {
    public static T GetLastMessage<T>(this IMessageTracker<T> tracker)
    {
      return tracker.Messages.LastOrDefault();
    }
    public static object GetLastMessage(this IMessageTracker tracker)
    {
      return tracker.Messages.LastOrDefault();
    }
    public static MessageRequest GetLastRequest(this IMessageTracker tracker)
    {
      return tracker.Requests.LastOrDefault();
    }
  }

}