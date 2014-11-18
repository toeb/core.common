using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Core.Context;

namespace Core
{
  /// <summary>
  /// a service for providing the correct context to any other service at the correct time
  /// 
  /// </summary>
  public interface IContextService
  {
    /// <summary>
    /// gets the context described by descriptor
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    IContext GetContext(ContextDescriptor descriptor);

    IContext GetContextById(Guid id);
    IContext RequireContext(ContextDescriptor descriptor);
    
    void ReleaseContext(IContext context);
  }

  public static class IContextServiceExtensions
  {
    /// <summary>
    /// returns the current request context
    /// </summary>
    /// <param name="contextService"></param>
    /// <returns></returns>
    public static IContext GetRequestContext(this IContextService contextService)
    {
      return contextService.GetContext(ContextDescriptor.Request);
    }
    public static string GetRequestUserName(this IContextService contextService)
    {
      var context = contextService.GetRequestContext();
      var username = context.Get<string>(Constants.RequestUserName);
      return username;
    }

  }
}
