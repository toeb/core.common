using Core.Modules.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Messaging
{
  public static class IApplicationMessagingExtensions
  {
    /// <summary>
    /// returns a messagtracker for application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IMessageTracker<T> TrackMessages<T>(this IApplication app)
    {
      var service = app.GetService<IMessagePipeline>();
      return service.TrackMessages<T>();
    }
    /// <summary>
    /// reutrns a message tracker for the application (usefull for unittesting)
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IMessageTracker TrackMessages(this IApplication app)
    {
      var service = app.GetService<IMessagePipeline>();
      return service.TrackMessages();
    }
    /// <summary>
    /// gets the default message pipeline of the application
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IMessagePipeline GetMessagePipeline(this IApplication app)
    {
      return app.GetService<IMessagePipeline>();
    }
    /// <summary>
    /// returns the default messaging service of the application
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IMessagingService GetMessagingService(this IApplication app)
    {
      return app.GetService<IMessagingService>();
    }

  }
}
