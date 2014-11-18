using System;
using System.Collections.Generic;

namespace Core
{









  /// <summary>
  /// a dictionary
  /// </summary>

  public interface IContext : IDisposable
  {
    IContextProvider Provider { get; }
    IContextService Service { get; }
    ContextDescriptor Descriptor { get; }


    Guid ContextId { get; }
    object Get(string key);
    void Set(string key, object value);
    bool ContainsKey(string key);
    IEnumerable<string> Keys { get; }
  }


}