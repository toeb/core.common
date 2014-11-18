using System.Diagnostics;

namespace Core
{
  [DebuggerDisplay("Contest: {ContextKey}")]
  public class ContextDescriptor
  {
    public static readonly ContextDescriptor Persistent = new ContextDescriptor("Persistent", null);
    public static readonly ContextDescriptor AppDomain = new ContextDescriptor("AppDomain", Persistent);
    public static readonly ContextDescriptor Application = new ContextDescriptor("Application", AppDomain);
    public static readonly ContextDescriptor Module = new ContextDescriptor("Module", Application);
    public static readonly ContextDescriptor Request = new ContextDescriptor("Request", Request);
    public static readonly ContextDescriptor Thread = new ContextDescriptor("Thread", Thread);
    
    private ContextDescriptor(string contextKey, ContextDescriptor parentContext)
    {
      ParentContext = parentContext;
      ContextKey = contextKey;
    }
    
    public ContextDescriptor ParentContext { get; private set; }

    public static implicit operator ContextDescriptor(string contextKey)
    {
      switch (contextKey)
      {
        case "Persistent":
          return Persistent;

        case "AppDomain":
          return AppDomain;

        case "Application":
          return Application;

        case "Request":
          return Request;

        case "Thread":
          return Thread;
        case "Module":
          return Module;

      }
      return null;
    }


    public string ContextKey { get; private set; }
  }
}